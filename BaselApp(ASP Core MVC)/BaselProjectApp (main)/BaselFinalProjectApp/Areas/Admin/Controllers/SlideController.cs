using BaselFinalProjectApp.Areas.Admin.Models.ViewModel;
using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Models.PageModel.HomePageModel;
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
    public class SlideController : Controller
    {

        private BaselDbContext _baselDb { get; set; }

        public SlideController(BaselDbContext baselDb)
        {
            _baselDb = baselDb;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<SlideLanguage> slideLanguages = _baselDb.Slides
                .SelectMany(a => a.SlideLanguages
                    .Where(x => x.LanguageId == 1))
                        .Include(x => x.Slide)
                            .ToList();

            return View(slideLanguages);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id!=null)
                {
                     List<SlideLanguage> slideLanguages = _baselDb.Slides
                        .SelectMany(x => x.SlideLanguages
                           .Where(z => z.Slide.Id == id))
                               .Include(a => a.Slide)
                                   .ToList();

                    SlideModelEdit slideModelEdit = new SlideModelEdit()
                    {
                        Id = slideLanguages.Select(x => x.SlideId).FirstOrDefault(),

                        EngHeader = slideLanguages.Where(x => x.LanguageId == 1).Select(x => x.Header).SingleOrDefault(),
                        AzHeader = slideLanguages.Where(x => x.LanguageId == 2).Select(x => x.Header).SingleOrDefault(),
                        RuHeader = slideLanguages.Where(x => x.LanguageId == 3).Select(x => x.Header).SingleOrDefault(),

                        EngTitle = slideLanguages.Where(x => x.LanguageId == 1).Select(x => x.Title).SingleOrDefault(),
                        AzTitle = slideLanguages.Where(x => x.LanguageId == 2).Select(x => x.Title).SingleOrDefault(),
                        RuTitle = slideLanguages.Where(x => x.LanguageId == 3).Select(x => x.Title).SingleOrDefault(),

                        EngDescription = slideLanguages.Where(x => x.LanguageId == 1).Select(x => x.Description).SingleOrDefault(),
                        AzDescription = slideLanguages.Where(x => x.LanguageId == 2).Select(x => x.Description).SingleOrDefault(),
                        RuDescription = slideLanguages.Where(x => x.LanguageId == 3).Select(x => x.Description).SingleOrDefault(),

                        EngFooter = slideLanguages.Where(x => x.LanguageId == 1).Select(x => x.Footer).SingleOrDefault(),
                        AzFooter = slideLanguages.Where(x => x.LanguageId == 2).Select(x => x.Footer).SingleOrDefault(),
                        RuFooter = slideLanguages.Where(x => x.LanguageId == 3).Select(x => x.Footer).SingleOrDefault(),

                        OldImage = slideLanguages.Select(x => x.Slide).Select(z => z.Image).FirstOrDefault()
                    };
                    return View(slideModelEdit);
                }
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SlideModelEdit slideModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (slideModel?.Id != null)
                    {
                        List<SlideLanguage> slideLanguages = _baselDb.Slides
                            .SelectMany(x => x.SlideLanguages
                               .Where(z => z.Slide.Id == slideModel.Id))
                                   .Include(a => a.Slide)
                                       .ToList();

                        foreach (SlideLanguage item in slideLanguages)
                        {
                            if (item.LanguageId == 1)
                            {
                                item.Header = slideModel.EngHeader;
                                item.Title = slideModel.EngTitle;
                                item.Description = slideModel.EngDescription;
                                item.Footer = slideModel.EngFooter;
                            }
                            else if (item.LanguageId == 2)
                            {
                                item.Header = slideModel.AzHeader;
                                item.Title = slideModel.AzTitle;
                                item.Description = slideModel.AzDescription;
                                item.Footer = slideModel.AzFooter;
                            }
                            else if (item.LanguageId == 3)
                            {
                                item.Header = slideModel.RuHeader;
                                item.Title = slideModel.RuTitle;
                                item.Description = slideModel.RuDescription;
                                item.Footer = slideModel.RuFooter;
                            }
                        }

                        Slide slide = slideLanguages.Select(x => x.Slide).FirstOrDefault();

                        string imageName = slideModel.Image == null ? slideModel.OldImage : slideModel.Image.FileName;

                        if (imageName!=null)
                        {
                            if (slide.Image != imageName)
                            {
                                if (slideModel.Image.ContentType.Contains("image/"))
                                {
                                    string ImageName = string.Empty;

                                    string fileLocationDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\lib\\image", slide.Image);

                                    if (System.IO.File.Exists(fileLocationDelete))
                                    {
                                        System.IO.File.Delete(fileLocationDelete);

                                        ImageName = DateTime.Now.ToString("yyyyMMddHHmmssss") + slideModel.Image.FileName;

                                        string fileLocationAdd = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\lib\\image", ImageName);

                                        using (FileStream fileStream = new FileStream(fileLocationAdd, FileMode.Create))
                                        {
                                            await slideModel.Image.CopyToAsync(fileStream);
                                        }

                                        slide.Image = ImageName;

                                        _baselDb.Slides.Update(slide);

                                        await _baselDb.SaveChangesAsync();

                                        return RedirectToAction("Index");
                                    }
                                    else
                                        ModelState.AddModelError("", "File is not exists");
                                }
                                else
                                    ModelState.AddModelError("", "You can upload only image");
                            }
                            await _baselDb.SaveChangesAsync();
                            return RedirectToAction("Index");
                        }
                        else
                            ModelState.AddModelError("", "Image is not found");
                    }
                }
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
            return RedirectToAction("Edit");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            try
            {
                List<SlideLanguage> slideLanguages = _baselDb.Slides
                   .SelectMany(x => x.SlideLanguages
                           .Where(z => z.Slide.Id == id))
                               .Include(a => a.Slide)
                                   .ToList();

                return View(slideLanguages);
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, string name)
        {
            string fileName = string.Empty;

            try
            {
                List<SlideLanguage> slideLanguages = _baselDb.Slides
                   .SelectMany(x => x.SlideLanguages
                           .Where(z => z.Slide.Id == id))
                               .Include(a => a.Slide)
                                   .ToList();

                _baselDb.Slides
                   .Select(x => x.SlideLanguages
                        .Remove(slideLanguages
                            .Find(z => z.LanguageId == 1)));

                _baselDb.Slides
                   .Select(x => x.SlideLanguages
                        .Remove(slideLanguages
                            .Find(z => z.LanguageId == 2)));

                _baselDb.Slides
                   .Select(x => x.SlideLanguages
                        .Remove(slideLanguages
                            .Find(z => z.LanguageId == 3)));


                _baselDb.Slides
                    .Remove(slideLanguages
                        .Where(x => x.Slide.Id == id)
                            .Select(x => x.Slide)
                                .FirstOrDefault());

                fileName = _baselDb.Slides.Find(id).Image;

                string fileLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\lib\\image", fileName);

                if (System.IO.File.Exists(fileLocation))
                    System.IO.File.Delete(fileLocation);
                else
                    ModelState.AddModelError("","File is not exists");

                await _baselDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SlideModelCreate slideModel)
        {
            try
            {
                List<SlideLanguage> slideLanguages = new List<SlideLanguage>()
                {
                    new SlideLanguage()
                    {
                        LanguageId=1,
                        Header = slideModel.EngHeader,
                        Title = slideModel.EngTitle,
                        Description = slideModel.EngDescription,
                        Footer = slideModel.EngFooter
                    },
                    new SlideLanguage()
                    {
                        LanguageId=2,
                        Header = slideModel.AzHeader,
                        Title = slideModel.AzTitle,
                        Description = slideModel.AzDescription,
                        Footer = slideModel.AzFooter
                    },
                    new SlideLanguage()
                    {
                        LanguageId=3,
                        Header = slideModel.RuHeader,
                        Title = slideModel.RuTitle,
                        Description = slideModel.RuDescription,
                        Footer = slideModel.RuFooter
                    }
                };

                string ImageName = string.Empty;

                if (slideModel.Image.ContentType.Contains("image/"))
                {
                    try
                    {
                        ImageName = DateTime.Now.ToString("yyyyMMddHHmmssss") + slideModel.Image.FileName;

                        string fileLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\lib\\image", ImageName);

                        using (FileStream fileStream = new FileStream(fileLocation, FileMode.Create))
                        {
                            await slideModel.Image.CopyToAsync(fileStream);
                        }

                        Slide slide = new Slide()
                        {
                            SlideLanguages = slideLanguages,
                            Image = ImageName
                        };

                        _baselDb.Slides.Add(slide);

                        await _baselDb.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    catch (Exception exp)
                    {
                        ModelState.AddModelError("", exp.Message);
                    }
                }
                else
                    ModelState.AddModelError("", "You can upload only image");
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
            return View();
        }
    }
}
