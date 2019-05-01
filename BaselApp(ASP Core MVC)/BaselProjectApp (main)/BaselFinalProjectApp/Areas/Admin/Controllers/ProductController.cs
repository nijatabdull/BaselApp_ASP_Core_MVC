using BaselFinalProjectApp.Areas.Admin.Models.ViewModel;
using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Models.MainModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private BaselDbContext _baselDb { get; set; }

        public ProductController(BaselDbContext baselDb)
        {
            _baselDb = baselDb;
        }

        [HttpGet]
        public IActionResult Index()
        {
           List<ProductLanguage> productLanguages = _baselDb.Products
                .SelectMany(x => x.ProductLanguages)
                    .Where(z=>z.LanguageId==1)
                        .Include(x=>x.Product)
                            .ToList();

            List<ColorLanguage> colorLanguages = _baselDb.Colors
                        .SelectMany(x => x.ColorLanguages
                            .Where(z => z.LanguageId == 1))
                                .Include(x => x.Color)
                                    .ToList();

            List<CategoryLanguage> categoryLanguages = _baselDb.Categories
                .Include(x => x.Products)
                    .SelectMany(x => x.CategoryLanguages)
                        .Where(z => z.LanguageId == 1)
                            .Include(x => x.Category)
                                .ToList();

            List<ProductColor> productColors = _baselDb.Products
                .SelectMany(x => x.ProductColors)
                    .Include(x => x.Product)
                        .Include(x => x.Color)
                            .ToList();

            List<Product> products = _baselDb.Products.Include(x => x.Images).ToList();

            return View(productLanguages);
        }

        [HttpGet]
        public IActionResult Create()
        {
            List<Color> colors = _baselDb.Colors.ToList();

            List<ColorModel> colorModels = new List<ColorModel>();

            foreach (Color item in colors)
            {
                colorModels.Add(new ColorModel()
                {
                    IsOptionSelected = false,
                    OptionId = item.Id,
                    OptionName = item.ColorCode
                });
            }

            List<CategoryLanguage> categoryLanguages = _baselDb.Categories
                .SelectMany(x => x.CategoryLanguages)
                .Where(x=>x.LanguageId==1)
                    .Include(x => x.Category)
                        .ToList();

            ProductCreateData productCreateData = new ProductCreateData()
            {
                ColorModels = colorModels,
                ProductModelCreate = new ProductModelCreate(),
                CategoryLanguages = categoryLanguages
            };

            return View(productCreateData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModelCreate productModelCreate,List<ColorModel> colorModels)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<ProductLanguage> productLanguages = new List<ProductLanguage>()
                    {
                        new ProductLanguage()
                        {
                            LanguageId = 1,
                            Name = productModelCreate.EngName,
                            Detail = productModelCreate.EngDetail,
                            About = productModelCreate.EngAbout,
                            Benefit = productModelCreate.EngBenefit
                        },
                        new ProductLanguage()
                        {
                            LanguageId = 2,
                            Name = productModelCreate.AzName,
                            Detail = productModelCreate.AzDetail,
                            About = productModelCreate.AzAbout,
                            Benefit = productModelCreate.AzBenefit
                        },
                        new ProductLanguage()
                        {
                            LanguageId = 3,
                            Name = productModelCreate.RuName,
                            Detail = productModelCreate.RuDetail,
                            About = productModelCreate.RuAbout,
                            Benefit = productModelCreate.RuBenefit
                        }
                    };

                    List<ProductColor> productColors = new List<ProductColor>();

                    foreach (ColorModel item in colorModels)
                    {
                        if (item.IsOptionSelected)
                        {
                            productColors.Add(new ProductColor()
                            {
                                ColorId = item.OptionId
                            });
                        }
                    }

                    List<Measure> measures = new List<Measure>();

                    measures.Add(new BaselFinalProjectApp.Models.MainModel.Measure()
                    {
                        Size = productModelCreate.Size,
                        SKU = productModelCreate.SKU,
                        Age = productModelCreate.Age,
                        Chest = productModelCreate.Chest,
                        Height = productModelCreate.Height,
                        Hip = productModelCreate.Hip,
                        Waist = productModelCreate.Waist
                    });

                    string ImageName = string.Empty;

                    List<Image> images = new List<Image>();

                    foreach (IFormFile item in productModelCreate.Image)
                    {
                        if (item.ContentType.Contains("image/"))
                        {
                            ImageName = DateTime.Now.ToString("yyyyMMddHHmmssss") + item.FileName;

                            string fileLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\lib\\image", ImageName);

                            using (FileStream fileStream = new FileStream(fileLocation, FileMode.Create))
                            {
                                await item.CopyToAsync(fileStream);
                            }

                            images.Add(new Image()
                            {
                                FileName = ImageName
                            });                            
                        }
                    }
                    Product product = new Product()
                    {
                        CategoryId = productModelCreate.CategoryId,
                        IsActivated = true,
                        Images = images,
                        Price = productModelCreate.Price,
                        Measures = measures,
                        Stock = productModelCreate.Stock,
                        ProductColors = productColors,
                        ProductLanguages = productLanguages
                    };

                    _baselDb.Products.Add(product);

                    await _baselDb.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception exp)
                {
                    return View(exp.Message);
                }                    
            }
            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            List<ProductLanguage> productLanguages = _baselDb.Products
                .SelectMany(x => x.ProductLanguages)
                    .Where(z => z.LanguageId == 1)                    
                        .Include(x => x.Product)
                        .Select(x => x.Product)
                            .Where(x=>x.Id==id).
                            SelectMany(x=>x.ProductLanguages)
                            .Where(x=>x.LanguageId==1)
                                .ToList();

            List<ColorLanguage> colorLanguages = _baselDb.Colors
                        .SelectMany(x => x.ColorLanguages
                            .Where(z => z.LanguageId == 1))
                                .Include(x => x.Color)
                                    .ToList();

            List<CategoryLanguage> categoryLanguages = _baselDb.Categories
                .Include(x => x.Products)
                    .SelectMany(x => x.CategoryLanguages)
                        .Where(z => z.LanguageId == 1)
                            .Include(x => x.Category)
                                .ToList();

            List<ProductColor> productColors = _baselDb.Products
                .SelectMany(x => x.ProductColors)
                    .Include(x => x.Product)
                        .Include(x => x.Color)
                            .ToList();


            List<Product> products = _baselDb.Products.Include(x => x.Images).ToList();

            return View(productLanguages);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, string name)
        {
            List<ProductLanguage> productLanguages = await _baselDb.Products
                  .SelectMany(x => x.ProductLanguages)
                      .Where(z => z.LanguageId == 1)
                          .Include(x => x.Product)
                          .Select(x => x.Product)
                              .Where(x => x.Id == id)
                              .SelectMany(x => x.ProductLanguages)
                                  .ToListAsync();

            List<ProductColor> productColor = await _baselDb.Products
                .Where(x => x.Id == id)
                    .SelectMany(x => x.ProductColors)
                            .ToListAsync();

            List<Product> products = _baselDb.Products.Include(x=>x.Images).ToList();

            _baselDb.Products
                .Remove(productLanguages.Select(x=>x.Product).Where(x=>x.Id==id).FirstOrDefault());

            List<Image> images = await _baselDb.Images.Where(x => x.ProductId == id).ToListAsync();

            foreach (Image item in images)
            {
                 _baselDb.Images.Remove(item);
            }         

            List<Measure> measures = await _baselDb.Measures.Where(x => x.ProductId == id).ToListAsync();

            if (measures!=null)
            {
                foreach (Measure item in measures)
                {
                        _baselDb.Measures.Remove(item);
                }
            }

            foreach (ProductColor item in productColor)
            {
                _baselDb.Products.Select(x => x.ProductColors.Remove(item));
            }

            foreach (ProductLanguage item in productLanguages)
            {
                _baselDb.Products.Select(x => x.ProductLanguages.Remove(item));
            }


            List<string> names = await _baselDb.Images.Where(x => x.ProductId == id).Select(x => x.FileName).ToListAsync();

            foreach (var item in names)
            {
                string fileLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\lib\\image", item);

                if (System.IO.File.Exists(fileLocation))
                    System.IO.File.Delete(fileLocation);

                fileLocation = string.Empty;
            }

            _baselDb.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            List<ProductLanguage> productLanguages = _baselDb.Products
                .Where(x=>x.Id==id)
                    .SelectMany(x => x.ProductLanguages)
                            .Include(x => x.Product)
                                .Where(x=>x.ProductId==id)
                                    .ToList();

            List<Measure> measures = _baselDb.Measures.ToList();

            List<Image> images = _baselDb.Products
                .Where(x => x.Id == id)
                    .Include(x => x.Images)
                        .SelectMany(x=>x.Images)
                            .ToList();

            List<CategoryLanguage> categoryLanguages = _baselDb.Categories
                    .SelectMany(x => x.CategoryLanguages)
                        .Where(z => z.LanguageId == 1)
                            .Include(x => x.Category)
                                .ToList();

            List<ColorModel> colorModels = new List<ColorModel>();

            List<Color> allcolors = _baselDb.Colors.ToList();

            List<Color> colors = _baselDb.Products.Where(x => x.Id == id)
                .SelectMany(x => x.ProductColors).Include(x => x.Color)
                            .Select(x => x.Color).ToList();

            foreach (Color item in allcolors)
            {
                bool logic = true;
                foreach (Color color in colors)
                {
                    if (item.Id == color.Id)
                    {
                        colorModels.Add(new ColorModel()
                        {
                            IsOptionSelected = true,
                            OptionId = item.Id,
                            OptionName = item.ColorCode
                        });
                        logic = false;
                    }
                }
                if (logic)
                {
                    colorModels.Add(new ColorModel()
                    {
                        IsOptionSelected = false,
                        OptionId = item.Id,
                        OptionName = item.ColorCode
                    });
                }
            }

            ProductEditData productEditData = new ProductEditData()
            {
                CategoryLanguages = categoryLanguages,
                ProductLanguages = productLanguages,
                ProductModelEdit = new ProductModelEdit(),
                ColorModels = colorModels
            };

            return View(productEditData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModelEdit productModelEdit, List<ColorModel> colorModels)
        {
            if (ModelState.IsValid)
            {
                List<ProductLanguage> productLanguages = await _baselDb.Products
                    .Where(x=>x.Id==productModelEdit.Id)
                        .Include(x => x.ProductLanguages)
                            .SelectMany(x=>x.ProductLanguages)
                                .ToListAsync();

                foreach (ProductLanguage item in productLanguages)
                {
                    if (item.LanguageId==1)
                    {
                        item.Name = productModelEdit.EngName;
                        item.About = productModelEdit.EngAbout;
                        item.Benefit = productModelEdit.EngBenefit;
                        item.Detail = productModelEdit.EngDetail;
                    }
                    if (item.LanguageId == 2)
                    {
                        item.Name = productModelEdit.AzName;
                        item.About = productModelEdit.AzAbout;
                        item.Benefit = productModelEdit.AzBenefit;
                        item.Detail = productModelEdit.AzDetail;
                    }
                    if (item.LanguageId == 3)
                    {
                        item.Name = productModelEdit.RuName;
                        item.About = productModelEdit.RuAbout;
                        item.Benefit = productModelEdit.RuBenefit;
                        item.Detail = productModelEdit.RuDetail;
                    }
                }

                List<Measure> measures = await _baselDb.Products
                    .Where(x => x.Id == productModelEdit.Id)
                        .Include(x => x.Measures).SelectMany(x => x.Measures)
                            .ToListAsync();

                foreach (Measure item in measures)
                {
                    item.Size = productModelEdit.Size;
                    item.SKU = productModelEdit.SKU;
                    item.Height = productModelEdit.Height;
                    item.Hip = productModelEdit.Hip;
                    item.Waist = productModelEdit.Waist;
                    item.Chest = productModelEdit.Chest;
                    item.Age = productModelEdit.Age;
                }

                List<ProductColor> productColors = await _baselDb.Products
                    .Where(x => x.Id == productModelEdit.Id)
                        .Include(x => x.ProductColors)
                            .SelectMany(x => x.ProductColors)
                                .ToListAsync();

                foreach (ProductColor item in productColors)
                {
                    _baselDb.Products.Select(x => x.ProductColors.Remove(item));
                }

                List<ProductColor> newProductColors = new List<ProductColor>();

                foreach (ColorModel item in colorModels)
                {
                    if (item.IsOptionSelected)
                    {
                        newProductColors.Add(new ProductColor()
                        {
                            ColorId = item.OptionId
                        });
                    }
                }

                List<Image> images = await _baselDb.Images.Where(x => x.ProductId == productModelEdit.Id).ToListAsync();

                if (productModelEdit.Image!=null)
                {
                    //Images in DB

                    //Create New Image
                    string ImageName = string.Empty;

                    foreach (IFormFile item in productModelEdit.Image)
                    {
                        if (item.ContentType.Contains("image/"))
                        {
                            ImageName = DateTime.Now.ToString("yyyyMMddHHmmssss") + item.FileName;

                            string fileLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\lib\\image", ImageName);

                            using (FileStream fileStream = new FileStream(fileLocation, FileMode.Create))
                            {
                                await item.CopyToAsync(fileStream);
                            }

                            images.Add(new Image()
                            {
                                FileName = ImageName
                            });
                        }
                    }
                }

                Product product = await _baselDb.Products
                    .Where(x => x.Id == productModelEdit.Id)
                        .SingleOrDefaultAsync();

                product.Images = images;
                product.CategoryId = productModelEdit.CategoryId;
                product.Measures = measures;
                product.Price = productModelEdit.Price;
                product.Stock = productModelEdit.Stock;
                product.ProductColors = newProductColors;
                product.ProductLanguages = productLanguages;

                _baselDb.Products.Update(product);
                await _baselDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Edit");
        }


        [HttpGet]
        public IActionResult Measure(int? id)
        {
            List<Measure> measures = _baselDb.Measures.Where(x => x.ProductId == id).ToList();

            if (measures != null)
                return View(measures);

            return View();
        }
    }
}
