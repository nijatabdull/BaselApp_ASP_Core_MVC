using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using BaselFinalProjectApp.Models.MainModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaselFinalProjectApp.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace BaselFinalProjectApp.Controllers
{
    public class ShopController : BaseController
    {
        private BaselDbContext _baselDb { get; set; }
        private UserManager<AppUser> _userManager { get; set; }

        public ShopController(BaselDbContext baselDb,
                            UserManager<AppUser> userManager)
        {
            _baselDb = baselDb;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Product(int id)
        {
            try
            {
                HttpContext.Session.SetInt32("CategoryId", id);
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Product(int? current = 1)
        {
            try
            {
                int? id = HttpContext.Session.GetInt32("CategoryId");

                List<Product> products = await GetProductsAsync(current, id);

                return PartialView("_ShopProduct", products);
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpPost]
        public JsonResult Pagination(int? length)
        {
            try
            {
                int? id = HttpContext.Session.GetInt32("CategoryId");

                List<Product> products;

                if (id == 0)
                {
                    products = _baselDb.Products
                        .Where(x => x.Stock > 0).AsNoTracking().ToList();
                }
                else
                {
                    products = _baselDb.Products.Where(x => x.CategoryId == id)
                                   .Where(x => x.Stock > 0).AsNoTracking().ToList();
                }

                length = 8;
                int total = products.Count;

                object data = new
                {
                    length,
                    total
                };

                return Json(data);
            }
            catch (Exception exp)
            {
                return Json(exp.Message);
            }
        }

        [HttpGet]
        public IActionResult ProductOption(int? id)
        {
            try
            {
                if (id != null)
                {
                    int Id = (int)id;
                    HttpContext.Session.SetInt32("ProductId", Id);

                    int CatId = _baselDb.Products
                            .Where(x => x.Id == id)
                                .Select(x => x.CategoryId)
                                    .SingleOrDefault();

                    HttpContext.Session.SetInt32("CatId", CatId);

                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Error","Home");
        }

        [HttpGet]
        public string WishListRefresh()
        {
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    AppUser appUser = _baselDb.AppUsers
                        .Where(x => x.UserName == HttpContext.User.Identity.Name)
                            .Include(x => x.WishList)
                                .ThenInclude(x => x.ProductWishLists)
                                    .SingleOrDefault();

                    int? wishListCount = appUser.WishList.ProductWishLists.Count;

                    return wishListCount.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return "";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> DeleteProductFromWishList(int? id)
        {
            try
            {
                if (id != null)
                {
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        ProductWishList productWishList = await _baselDb.AppUsers
                            .Where(x => x.UserName == HttpContext.User.Identity.Name)
                                .Include(x => x.WishList).ThenInclude(x => x.ProductWishLists)
                                    .Select(x=>x.WishList).SelectMany(x=>x.ProductWishLists)
                                        .Where(x=>x.ProductId==id)
                                        .SingleOrDefaultAsync();

                        AppUser appUser = await _baselDb.AppUsers
                             .Where(x => x.UserName == HttpContext.User.Identity.Name)
                                 .Include(x => x.WishList).ThenInclude(x => x.ProductWishLists)
                                     .SingleOrDefaultAsync();

                        appUser.WishList.ProductWishLists.Remove(productWishList);

                        await _baselDb.SaveChangesAsync();

                        return "Product deleted from WishList";
                    }
                    return "Please, Sign in to Site";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return "";
        }


        [HttpGet]
        public async Task<IActionResult> WishList()
        {
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    AppUser appUser = await _baselDb
                        .AppUsers.Where(x => x.UserName == HttpContext.User.Identity.Name)
                            .SingleOrDefaultAsync();
                        

                    List<Product> products = await _baselDb.WishLists.Where(x => x.Id == appUser.WishListId)
                        .SelectMany(x => x.ProductWishLists)
                            .Include(x => x.Product)
                                .Select(x => x.Product)
                                    .Include(x => x.ProductLanguages).Include(x=>x.Images)                                       
                                        .ToListAsync();

                    return View(products);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> WishListAddProduct(int? id)
        {
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    if (id != null)
                    {
                        AppUser appUser = await _baselDb
                            .AppUsers.Where(x => x.UserName == HttpContext.User.Identity.Name)
                                .Include(x => x.WishList).ThenInclude(x=>x.ProductWishLists)
                                    .SingleOrDefaultAsync();

                        if (appUser!=null)
                        {
                            Product product = await _baselDb.Products
                                .Where(x => x.Id == id)
                                    .SingleOrDefaultAsync();

                            if (product != null)
                            {
                                if (appUser.WishList == null)
                                {
                                    List<ProductWishList> productWishLists = new List<ProductWishList>
                                    {
                                        new ProductWishList()
                                        {
                                            Product = product
                                        }
                                    };

                                    WishList wishList = new WishList
                                    {
                                        AppUser = appUser,
                                        ProductWishLists = productWishLists,
                                        AppUserId = appUser.Id
                                    };

                                    await _baselDb.WishLists.AddAsync(wishList);

                                    await _baselDb.SaveChangesAsync();

                                    WishList newWishList = await _baselDb.WishLists
                                        .Where(x => x.AppUserId == appUser.Id)
                                            .SingleOrDefaultAsync();

                                    appUser.WishListId = newWishList.Id;                                  
                                }
                                else
                                {
                                    if (!appUser.WishList.ProductWishLists
                                        .Any(x => x.ProductId==id))
                                    {
                                        ProductWishList productWishList = new ProductWishList()
                                        {
                                            Product = product,
                                            ProductId = product.Id
                                        };

                                        appUser.WishList.ProductWishLists.Add(productWishList);

                                    }
                                }
                                await _baselDb.SaveChangesAsync();

                                return "Product Added to WishList";
                            }                                                      
                        }
                    }
                }
                else
                {
                    return "Please, Sign in to the Site";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return "";
        }

        [HttpGet]
        public async Task<IActionResult> ProductReview()
        {
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    int? productId = HttpContext.Session.GetInt32("ProductId");

                    List<Review> reviews = await _baselDb.Reviews
                        .Where(x => x.ProductId == productId).Include(x => x.AppUser)
                                .ToListAsync();

                    return PartialView("ProductReview", reviews);

                }
                else               
                    return PartialView("ProductReview", new List<Review>());
                
            }
            catch (Exception)
            {

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductReview(string data,string rate)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (data != null && rate != null)
                {
                    if (byte.TryParse(rate, out byte rating))
                    {
                        try
                        {
                            List<Review> reviews = new List<Review>();

                            Review review = new Review()
                            {
                                Date = DateTime.Now,
                                Comment = data,
                                Rating = rating
                            };

                            reviews.Add(review);

                            AppUser appUser = await _baselDb.AppUsers
                                .Where(x => x.UserName == HttpContext.User.Identity.Name)
                                    .SingleOrDefaultAsync();

                            appUser.Reviews = reviews;

                            int? productId = HttpContext.Session.GetInt32("ProductId");

                            Product product = await _baselDb.Products
                                .Where(x => x.Id == productId)
                                    .SingleOrDefaultAsync();

                            product.Reviews = reviews;

                            _baselDb.Reviews.Add(review);

                            await _baselDb.SaveChangesAsync();

                            return Json("");
                        }
                        catch (Exception)
                        { 
                            throw;
                        }
                    }
                    else
                        return Json("Rating is not found");
                }
                return Json("Please, enter correct information");
            }
            return Json("Please, Sign in to the Site");
        }

        private async Task<List<Product>> GetProductsAsync(int? current,int? id)
        {
            if (current != null)
            {
                try
                {
                    List<Product> products;
                    int productCount = 8;

                    if (id == 0)
                    {
                        products = await _baselDb
                            .Products.Include(x => x.Images).Where(x => x.Stock > 0)
                                .Include(x => x.ProductLanguages)
                                    .Skip(((int)current - 1) * productCount).Take(productCount)
                                        .ToListAsync();
                    }
                    else
                    {
                        products = await _baselDb
                            .Products.Include(x => x.Images).Where(x => x.Stock > 0)
                              .Include(x => x.ProductLanguages)
                                .Where(x => x.CategoryId == id)
                                    .Skip(((int)current - 1) * productCount).Take(productCount)
                                        .ToListAsync();
                    }
                    return products;
                }
                catch (Exception exp)
                {

                }
            }
            return null;
        }
    }
}
