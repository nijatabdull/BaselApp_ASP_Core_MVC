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
    public class ColorController : Controller
    {
        private BaselDbContext _baselDb { get; set; }

        public ColorController(BaselDbContext baselDb)
        {
            _baselDb = baselDb;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ColorLanguage> colorLanguages = _baselDb.Colors
                .SelectMany(x=>x.ColorLanguages)
                    .Where(x=>x.LanguageId==1)
                        .Include(x=>x.Color)
                            .ToList();

            return View(colorLanguages);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorsModel ColorsModel)
        {
            if (ModelState.IsValid)
            {
                List<ColorLanguage> colorLanguages = new List<ColorLanguage>()
                {
                    new ColorLanguage()
                    {
                        LanguageId=1,
                        Name = ColorsModel.EngName
                    },
                    new ColorLanguage()
                    {
                        LanguageId=2,
                        Name = ColorsModel.AzName
                    },
                    new ColorLanguage()
                    {
                        LanguageId=3,
                        Name = ColorsModel.RuName
                    }
                };

                Color category = new Color()
                {
                    ColorCode = ColorsModel.ColorCode,
                    ColorLanguages = colorLanguages
                };

                await _baselDb.Colors.AddAsync(category);

                await _baselDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            ColorLanguage colorLanguage = _baselDb.Colors
                .SelectMany(x => x.ColorLanguages)
                    .Where(x => x.LanguageId == 1)
                        .Include(x => x.Color)
                            .Where(x => x.ColorId == id)
                                .SingleOrDefault();

            return View(colorLanguage);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id,string name)
        {
            if (id != null)
            {
                _baselDb.Colors.Remove(await _baselDb.Colors.FindAsync(id));

                _baselDb.Colors.Select(x => x.ColorLanguages
                    .Remove(_baselDb.Colors.Where(a => a.Id == id)
                        .SelectMany(z => z.ColorLanguages).SingleOrDefault()));

                 await _baselDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Delete");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            List<ColorLanguage> colorLanguages = _baselDb.Colors
                .SelectMany(x => x.ColorLanguages)
                        .Include(x => x.Color)
                            .Where(x => x.ColorId == id)
                                .ToList();

            ColorData colorData = new ColorData()
            {
                ColorLanguages = colorLanguages,
                ColorsModel = new ColorsModel()
            };

            return View(colorData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ColorsModel colorsModel)
        {
            if (ModelState.IsValid)
            {
                List<ColorLanguage> colorLanguages = _baselDb.Colors
                    .SelectMany(x => x.ColorLanguages)
                        .Include(x => x.Color)
                            .Where(x => x.ColorId == colorsModel.Id)
                                .ToList();

                foreach (ColorLanguage item in colorLanguages)
                {
                    if (item.LanguageId == 1)
                    {
                        item.Name = colorsModel.EngName;
                    }
                    if (item.LanguageId == 2)
                    {
                        item.Name = colorsModel.AzName;
                    }
                    if (item.LanguageId == 3)
                    {
                        item.Name = colorsModel.RuName;
                    }
                }

                Color color = _baselDb.Colors.FindAsync(colorsModel.Id).Result;

                color.ColorCode = colorsModel.ColorCode;

                await _baselDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Edit");
        }
    }
}