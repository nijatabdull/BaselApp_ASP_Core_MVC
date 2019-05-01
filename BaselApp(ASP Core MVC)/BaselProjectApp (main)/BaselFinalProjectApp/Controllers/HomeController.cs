using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Infastructure;
using BaselFinalProjectApp.Models.MainModel;
using BaselFinalProjectApp.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Controllers
{
    public class HomeController : BaseController
    {
        private BaselDbContext _baselDb { get; set; }
        private SignInManager<AppUser> _signInManager { get; set; }
        private UserManager<AppUser> _userManager { get; set; }
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(BaselDbContext baselDb,
                                  SignInManager<AppUser> signInManager,
                                  UserManager<AppUser> userManager,
                                  IHostingEnvironment hostingEnvironment)
        {
            _baselDb = baselDb;
            _signInManager = signInManager;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ProductOfCart(int? id)
        {
            try
            {
                if (id!=null)
                {
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        string name = HttpContext.User.Identity.Name;

                        AppUser appUser = await _baselDb.AppUsers
                                .Where(x => x.UserName == name)
                                    .Include(x => x.Cart)
                                        .SingleOrDefaultAsync();

                        Product product = null;

                        product = await _baselDb.Products.Where(x => x.Stock >0)
                                                .Where(x => x.Id == id).SingleOrDefaultAsync();

                        if (await _signInManager.CanSignInAsync(appUser))
                        {
                            if (product != null)
                            {
                                int? CartId = null;

                                if (appUser.Cart != null)
                                {
                                    CartId = appUser.CartId;

                                    Cart cart = _baselDb.Carts.Where(x => x.Id == CartId)
                                        .Include(x => x.ProductCarts).SingleOrDefault();

                                    int ProductId = cart.ProductCarts
                                        .Where(x => x.ProductId == id)
                                            .Select(x => x.ProductId)
                                                .SingleOrDefault();

                                    if (ProductId == id)
                                    {
                                        int ProductCount = cart.ProductCarts
                                            .Where(x => x.ProductId == id)
                                             .Select(x => x.ProductCount)
                                                .SingleOrDefault();

                                        ProductCount++;

                                        cart.ProductCarts.Where(x => x.ProductId == id)
                                            .Select(c => { c.ProductCount = ProductCount; return c; }).ToList();

                                    }
                                    else
                                    {
                                        ProductCart productCart = new ProductCart()
                                        {
                                            ProductId = (int)id,
                                            CartId = (int)CartId,
                                            ProductCount = 1
                                        };

                                        cart.ProductCarts.Add(productCart);
                                    }

                                    int productStock = product.Stock;

                                    productStock--;

                                    product.Stock = productStock;

                                    await _baselDb.SaveChangesAsync();
                                }
                                else
                                {
                                    List<ProductCart> productCarts = new List<ProductCart>()
                                    {
                                        new ProductCart()
                                        {
                                            ProductId = (int)id,
                                            ProductCount=1
                                        }
                                    };

                                    Cart cart = new Cart()
                                    {
                                        AppUser = appUser,
                                        ProductCarts = productCarts
                                    };

                                    int productStock = product.Stock;

                                    productStock--;

                                    product.Stock = productStock;

                                    _baselDb.Carts.Add(cart);

                                    _baselDb.SaveChanges();

                                    Cart cartDb = _baselDb.Carts.Where(x => x.AppUserId == appUser.Id).SingleOrDefault();

                                    appUser.CartId = cartDb.Id;
                                    appUser.Cart = cartDb;

                                    await _userManager.UpdateAsync(appUser);

                                }

                                List<Product> productsAll = await _baselDb.Products
                                                .Include(x => x.ProductCarts)
                                                    .Where(x => x.ProductCarts.Any(z => z.CartId == CartId))
                                                        .Include(x => x.ProductLanguages)
                                                    .Include(x => x.Images)
                                                .ToListAsync();

                                if (productsAll != null)
                                {
                                    int? cultures = HttpContext.Session.GetInt32("culture");

                                    if (cultures == null)
                                    {
                                        cultures = GetLanguage
                                             .GetLangId();
                                    }

                                    List<object> list = new List<object>();

                                    foreach (Product item in productsAll)
                                    {
                                        list.Add(new
                                        {
                                            item.Id,
                                            Name = item.ProductLanguages.Where(x => x.LanguageId == cultures).Select(x => x.Name).SingleOrDefault(),
                                            Count = item.ProductCarts.Where(x => x.CartId == CartId).Select(x => x.ProductCount).SingleOrDefault(),
                                            Image = ConvertImage(item.Images.Select(x => x.FileName).FirstOrDefault()),
                                            item.Price
                                        });
                                    }

                                    return new JsonResult(list);
                                }
                            }
                            return Json("Product is not found");
                        }
                    }
                    return Json("Please, Sign In to the Site");
                }
                return Json("Product Id is not found");

            }
            catch (Exception exp)
            {
                return Json(exp.Message);
            }

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteProductOfCart(int? id)
        {
            try
            {
                if (id != null)
                {
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        string name = HttpContext.User.Identity.Name;

                        AppUser appUser = await _baselDb.AppUsers
                                .Where(x => x.UserName == name)
                                    .Include(x => x.Cart).ThenInclude(x=>x.ProductCarts)
                                        .SingleOrDefaultAsync();

                        Product product = null;

                        product = _baselDb.Products.Find(id);

                        if (await _signInManager.CanSignInAsync(appUser))
                        {
                            if (product != null)
                            {
                                int? CartId = null;

                                if (appUser.Cart != null)
                                {
                                    CartId = appUser.CartId;

                                    product.Stock += appUser.Cart.ProductCarts.Where(x=>x.CartId==CartId)
                                            .Where(x=>x.ProductId==id)
                                        .Select(x => x.ProductCount).SingleOrDefault();

                                    appUser.Cart.ProductCarts
                                        .Remove(appUser.Cart.ProductCarts
                                            .Where(x=>x.CartId==CartId)
                                                .Where(x=>x.ProductId==id)
                                                    .SingleOrDefault());

                                    await _baselDb.SaveChangesAsync();
                                    return Json("Product is delete from Cart");
                                }
                            }
                            return Json("Product is not found");
                        }
                    }
                    return Json("Please, Sign In to the Site");
                }
                return Json("Product Id is not found");

            }
            catch (Exception exp)
            {
                return Json(exp.Message);
            }
        }


        [HttpGet]
        public async Task<JsonResult> ProductOfCart()
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
                        int? CartId = appUser.Cart.Id;

                        List<Product> productsAll = await _baselDb.Products
                                            .Include(x=>x.ProductCarts)
                                                .Where(x=>x.ProductCarts.Any(z=>z.CartId==CartId))
                                                    .Include(x => x.ProductLanguages)
                                                .Include(x => x.Images)
                                            .ToListAsync();

                        if (productsAll!=null)
                        {
                            int? cultures = HttpContext.Session.GetInt32("culture");

                            if (cultures == null)
                            {
                                cultures = GetLanguage
                                     .GetLangId();
                            }

                            List<object> list = new List<object>();

                            foreach (Product item in productsAll)
                            {
                                list.Add(new
                                {
                                    item.Id,
                                    Name = item.ProductLanguages.Where(x => x.LanguageId == cultures).Select(x => x.Name).SingleOrDefault(),
                                    Count = item.ProductCarts.Where(x => x.CartId == CartId).Select(x => x.ProductCount).SingleOrDefault(),
                                    Image = ConvertImage(item.Images.Select(x => x.FileName).FirstOrDefault()),
                                    item.Price
                                });
                            }

                            return Json(list);
                        }
                        return Json("Product is not found");
                    }                  
                }
                return Json("Please, Sign In to the Site");
            }
            catch(Exception exp)
            {
                return Json(exp.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchProduct(string search)
        {
            try
            {
                if (search != null && search != "")
                {
                    List<ProductLanguage> productLanguages = await GetProductLanguagesAsync(search);

                    return PartialView("_SearchProduct", productLanguages);
                }
                return PartialView("_SearchProduct", new List<ProductLanguage>());
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchProductNav(string search)
        {
            try
            {

                if (search != null && search != "")
                {
                    List<ProductLanguage> productLanguages = await GetProductLanguagesAsync(search);

                    return PartialView("_SearchProductNav", productLanguages);
                }
                return PartialView("_SearchProductNav", new List<ProductLanguage>());
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductSection(int? page)
        {
            try
            {
                if (page!=null)
                {
                    int pageIndex = (int)page;
                    List<Product> products = await GetProductsAsync(pageIndex);

                    if (products!=null)
                    {
                        return PartialView("_HomeProduct", products);
                    }
                }
                return PartialView("_HomeProduct", new List<Product>());
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }


        private async Task<List<Product>> GetProductsAsync(int page,int productCount=8)
        {
            List<Product> products = await _baselDb
                      .Products.Include(x => x.Images).Where(x => x.Stock > 0)
                        .Skip((page - 1) * productCount).Take(productCount)
                           .Include(x => x.ProductLanguages)
                               .Include(x => x.ProductColors)
                                       .ToListAsync();
            List<Color> colors = await _baselDb.Colors.ToListAsync();

            return products;
        }

        private async Task<List<ProductLanguage>> GetProductLanguagesAsync(string search)
        {
            int? cultures = HttpContext.Session.GetInt32("culture");

            if (cultures == null)
            {
                cultures = GetLanguage
                    .GetLangId();
            }

            List<ProductLanguage> products = await _baselDb.Products.Include(x => x.ProductLanguages)
                .SelectMany(x => x.ProductLanguages).Where(z => z.LanguageId == cultures)
                    .Where(x => x.Name.Contains(search))
                        .Include(x => x.Product).ThenInclude(x => x.Images)
                            .ToListAsync();

            return products;
        }

        private string ConvertImage(string image)
        {
            string path = _hostingEnvironment.WebRootPath + "\\lib\\image\\" + image;

            if (System.IO.File.Exists(path))
            {
                byte[] b = System.IO.File.ReadAllBytes(path);
                return "data:image/jpg;base64," + Convert.ToBase64String(b);
            }

            return "";
        }
    }
}
