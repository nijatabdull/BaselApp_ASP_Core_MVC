using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Infastructure;
using BaselFinalProjectApp.Models.MainModel;
using BaselFinalProjectApp.Models.ViewData;
using BaselFinalProjectApp.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace BaselFinalProjectApp.Controllers
{
    public class CartController : BaseController
    {
        private BaselDbContext _baselDb { get; set; }
        private UserManager<AppUser> _userManager { get; set; }

        public CartController(BaselDbContext baselDb,
                             UserManager<AppUser> userManager)
        {
            _baselDb = baselDb;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    string name = HttpContext.User.Identity.Name;

                    AppUser appUser = await _baselDb.AppUsers
                            .Where(x => x.UserName == name)
                                .Include(x => x.Cart)
                                    .SingleOrDefaultAsync();


                    if (appUser != null)
                    {
                        if (appUser.Cart!=null)
                        {
                            int CartId = (int)appUser.CartId;

                            HttpContext.Session.SetInt32("CartId", CartId);
                            HttpContext.Session.SetString("UserId", appUser.Id);

                            List<Product> productsAll = await _baselDb.Products
                                               .Include(x => x.ProductCarts)
                                                    .Where(x=>x.ProductCarts.Any(z=>z.CartId==CartId))
                                                       .Include(x => x.ProductLanguages)
                                                   .Include(x => x.Images)
                                               .ToListAsync();


                            if (productsAll != null)
                                return View(productsAll);
                        }
                        
                    }                    
                }
                return View();
            }
            catch (Exception exp)
            {
                return Json(exp.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> OrderList(string id)
        {
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    if (id != null)
                    {
                        AppUser appUser = await _userManager.FindByIdAsync(id);

                        if (appUser != null)
                        {
                            List<ProductCart> productCarts = await _baselDb.Products
                               .Include(x => x.ProductCarts)
                                   .SelectMany(x => x.ProductCarts)
                                       .Where(x => x.CartId == appUser.CartId)
                                           .Include(x => x.Product)
                                               .ThenInclude(z => z.ProductLanguages).AsNoTracking()
                                                   .ToListAsync();

                            OrderData orderData = new OrderData()
                            {
                                OrderModel = new OrderModel(),
                                Products = productCarts
                            };

                            return View(orderData);
                        }
                    }
                }               
            }
            catch (Exception exp)
            {
                return RedirectToAction("ProductList");
            }
            return RedirectToAction("ProductList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderList(OrderModel orderModel)
        {
            if (orderModel!=null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (HttpContext.User.Identity.IsAuthenticated)
                        {
                            AppUser appUser = await _baselDb.AppUsers
                                .Where(x => x.UserName == HttpContext.User.Identity.Name)
                                    .Include(x => x.BillingDetail)
                                        .SingleOrDefaultAsync();

                            if (appUser.BillingDetail == null)
                            {
                                BillingDetail billingDetail = new BillingDetail()
                                {
                                    Apartment = orderModel.Apartment,
                                    StreetAddress = orderModel.StreetAddress,
                                    Town = orderModel.Town
                                };
                                appUser.BillingDetail = billingDetail;
                            }
                            else
                            {
                                appUser.BillingDetail.Apartment = orderModel.Apartment;
                                appUser.BillingDetail.StreetAddress = orderModel.StreetAddress;
                                appUser.BillingDetail.Town = orderModel.Town;
                            }

                            HttpContext.Session.SetString("userTotalPrice", orderModel.TotalPrice.ToString());

                            appUser.Name = orderModel.Name;
                            appUser.Lastname = orderModel.LastName;

                            await _userManager.UpdateAsync(appUser);

                            return Redirect("https://localhost:44394/Index/?" + "userName=" + 
                                orderModel.Name + "&userLastName=" + orderModel.LastName + 
                                    "&userTotalPrice=" + orderModel.TotalPrice + "&userId=" + appUser.Id);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception exp)
                    {

                    }
                }
            }
            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public async Task<IActionResult> OrderCheckout(string result,string userId)
        {
            ViewBag.IsOrdered = false;

            if(result != null && userId != null)
            {
                if (result == "Payed")
                {
                    ViewBag.Result = "Thank you. Your order has been received.";
                    ViewBag.Color = "#7a9c59";
                    ViewBag.IsOrdered = true;

                    try
                    {
                        if (HttpContext.User.Identity.IsAuthenticated)
                        {
                            AppUser appUser = await _baselDb.AppUsers
                                .Where(x => x.UserName == HttpContext.User.Identity.Name)
                                    .Where(x=>x.Id==userId)
                                        .Include(x => x.BillingDetail)
                                            .Include(x=>x.Cart).ThenInclude(x=>x.ProductCarts)
                                                .SingleOrDefaultAsync();

                            List<ProductCart> productCarts = await _baselDb.Carts
                                .Where(x => x.AppUserId == appUser.Id)
                                    .Include(x => x.ProductCarts)
                                        .SelectMany(x => x.ProductCarts)
                                            .Include(x=>x.Product)
                                                .ThenInclude(x=>x.ProductLanguages)
                                                    .ToListAsync();

                            int? cultures = GetLanguage.GetLangId();

                            List<ProductOrder> productOrders = new List<ProductOrder>();

                            foreach (ProductCart item in productCarts)
                            {
                                productOrders.Add(new ProductOrder()
                                {
                                    ProductId = item.ProductId,
                                    ProductCount = item.ProductCount,
                                    ProductName = item.Product.ProductLanguages
                                        .Where(z=>z.LanguageId==cultures)
                                            .Select(z=>z.Name).SingleOrDefault(),
                                    ProductPrice = item.Product.Price
                                });
                            }

                            Random random = new Random();

                            int OrderNumber = random.Next(1, 999999);                           

                            string TotalPrice = HttpContext.Session.GetString("userTotalPrice");

                            if (TotalPrice != null)
                            {
                                List<Order> orders = new List<Order>();

                                string[] total = TotalPrice.Split(",");

                                decimal userTotalPrice = Convert.ToDecimal(total[0]);

                                Order order = new Order()
                                {
                                    AppUserId = appUser.Id,
                                    AppUser = appUser,
                                    Date = DateTime.Now,
                                    Number = OrderNumber,
                                    Status = "Full",
                                    Total = userTotalPrice,
                                    ProductOrders = productOrders
                                };

                                orders.Add(order);

                                appUser.Orders = orders;

                                await _baselDb.Orders.AddAsync(order);

                                OrderCheckOutData orderCheckOutData = new OrderCheckOutData()
                                {
                                    ProductOrders = productOrders,
                                    AppUser = appUser
                                };

                                foreach (ProductCart item in productCarts)
                                {
                                    appUser.Cart.ProductCarts.Remove(item);                                                                           
                                }

                                _baselDb.SaveChanges();

                                return View(orderCheckOutData);
                            }
                        }
                    }
                    catch(Exception exp)
                    {
                        ViewBag.Result = exp.Message;
                        ViewBag.Color = "red";
                        ViewBag.IsOrdered = false;
                    }
                }
                else if (result == "Low Balance")
                {
                    ViewBag.Result = "There is not enough balance on your card";
                    ViewBag.Color = "red";
                    ViewBag.IsOrdered = false;
                }
                else if (result == "Not Found TotalPrice")
                {
                    ViewBag.Result = "Amount not included";
                    ViewBag.Color = "red";
                    ViewBag.IsOrdered = false;
                }
                else
                {
                    ViewBag.Result = "No data found";
                    ViewBag.Color = "red";
                    ViewBag.IsOrdered = false;
                }
                
                return View();
            }
            return RedirectToAction("Error","Home");
        }
    }
}