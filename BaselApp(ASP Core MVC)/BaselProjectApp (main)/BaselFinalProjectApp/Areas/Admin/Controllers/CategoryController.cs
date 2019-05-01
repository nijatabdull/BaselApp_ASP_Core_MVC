using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaselFinalProjectApp.Areas.Admin.Models.ViewModel;
using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Models.MainModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaselFinalProjectApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private BaselDbContext _baselDb { get; set; }

        public CategoryController(BaselDbContext baselDb)
        {
            _baselDb = baselDb;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<CategoryLanguage> categoryLanguages = _baselDb.Categories
                .SelectMany(x=>x.CategoryLanguages)
                    .Where(x=>x.LanguageId==1)
                        .Include(x=>x.Category)
                            .ToList();

            return View(categoryLanguages);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                List<CategoryLanguage> categoryLanguages = new List<CategoryLanguage>()
                {
                    new CategoryLanguage()
                    {
                        LanguageId=1,
                        Name = categoryModel.EngName
                    },
                    new CategoryLanguage()
                    {
                        LanguageId=2,
                        Name = categoryModel.AzName
                    },
                    new CategoryLanguage()
                    {
                        LanguageId=3,
                        Name = categoryModel.RuName
                    }
                };

                Category category = new Category()
                {
                    Action = categoryModel.Action,
                    Controller = categoryModel.Controller,
                    CategoryLanguages = categoryLanguages
                };

                await _baselDb.Categories.AddAsync(category);

                await _baselDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            CategoryLanguage categoryLanguage = _baselDb.Categories
                .SelectMany(x => x.CategoryLanguages)
                    .Where(x => x.LanguageId == 1)
                        .Include(x => x.Category)
                            .Where(x => x.CategoryId == id)
                                .SingleOrDefault();

            return View(categoryLanguage);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id,string name)
        {
            if (id != null)
            {
                _baselDb.Categories.Remove(await _baselDb.Categories.FindAsync(id));

                _baselDb.Categories.Select(x => x.CategoryLanguages
                    .Remove(_baselDb.Categories.Where(a => a.Id == id)
                        .SelectMany(z => z.CategoryLanguages).SingleOrDefault()));

                 await _baselDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Delete");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            List<CategoryLanguage> categoryLanguages = _baselDb.Categories
                .SelectMany(x => x.CategoryLanguages)
                        .Include(x => x.Category)
                            .Where(x => x.CategoryId == id)
                                .ToList();

            CategoryData categoryData = new CategoryData()
            {
                CategoryLanguages = categoryLanguages,
                CategoryModel = new CategoryModel()
            };

            return View(categoryData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                List<CategoryLanguage> categoryLanguages = _baselDb.Categories
                    .SelectMany(x => x.CategoryLanguages)
                        .Include(x => x.Category)
                            .Where(x => x.CategoryId == categoryModel.Id)
                                .ToList();

                foreach (CategoryLanguage item in categoryLanguages)
                {
                    if (item.LanguageId == 1)
                    {
                        item.Name = categoryModel.EngName;
                    }
                    if (item.LanguageId == 2)
                    {
                        item.Name = categoryModel.AzName;
                    }
                    if (item.LanguageId == 3)
                    {
                        item.Name = categoryModel.RuName;
                    }
                }

                Category category = _baselDb.Categories.FindAsync(categoryModel.Id).Result;

                category.Action = categoryModel.Action;
                category.Controller = categoryModel.Controller;

                await _baselDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Edit");
        }
    }
}