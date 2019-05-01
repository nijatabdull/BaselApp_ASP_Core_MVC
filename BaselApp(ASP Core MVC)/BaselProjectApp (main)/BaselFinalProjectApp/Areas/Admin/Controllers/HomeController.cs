using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaselFinalProjectApp.Areas.Admin.Models.ViewModel;
using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Models.PageModel.AccountPageModel;
using BaselFinalProjectApp.Models.PageModel.HomePageModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaselFinalProjectApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private BaselDbContext _baselDb { get; set; }

        public HomeController(BaselDbContext baselDb)
        {
            _baselDb = baselDb;
        }

        [HttpGet]
        public IActionResult Menu()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MenuEdit(int? id)
        {
            try
            {
                List<MenuLanguage> MenuLanguages = _baselDb.Menus
                    .SelectMany(x => x.MenuLanguages
                            .Where(z => z.Menu.Id == id))
                                .Include(a => a.Menu)
                                    .ToList();

                if (MenuLanguages != null)
                {
                    MenuData menuData = new MenuData()
                    {
                        MenuModel = new MenuModel(),
                        Object = MenuLanguages
                    };

                    return View(menuData);
                }
            }
            catch (Exception exp) { }

            return View();
        }

        [HttpPost]
        public IActionResult MenuEdit(MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                if (menuModel?.Id != null)
                {
                    List<MenuLanguage> MenuLanguages = _baselDb.Menus
                        .SelectMany(x => x.MenuLanguages
                            .Where(z => z.Menu.Id == menuModel.Id))
                                .Include(a => a.Menu)
                                    .ToList();


                    foreach (MenuLanguage item in MenuLanguages)
                    {
                        if (item.LanguageId == 1)
                        {
                            item.Name = menuModel.EngName;
                        }
                        else if (item.LanguageId == 2)
                        {
                            item.Name = menuModel.AzName;
                        }
                        else if (item.LanguageId == 3)
                        {
                            item.Name = menuModel.RuName;
                        }
                    }

                    Menu Menu = MenuLanguages.Select(x => x.Menu).FirstOrDefault();

                    Menu.Action = menuModel.Action;
                    Menu.Controller = menuModel.Controller;

                    _baselDb.SaveChanges();

                }
            }

            return View(nameof(Menu));
        }

        [HttpGet]
        public IActionResult MenuDelete(int? id)
        {
            try
            {
                List<MenuLanguage> MenuLanguages = _baselDb.Menus
                   .SelectMany(x => x.MenuLanguages
                           .Where(z => z.Menu.Id == id))
                               .Include(a => a.Menu)
                                   .ToList();

                return View(MenuLanguages);
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpPost]
        public IActionResult MenuDelete(int? id, string name)
        {
            try
            {
                List<MenuLanguage> MenuLanguages = _baselDb.Menus
                   .SelectMany(x => x.MenuLanguages
                           .Where(z => z.Menu.Id == id))
                               .Include(a => a.Menu)
                                   .ToList();

                _baselDb.Menus
                   .Select(x => x.MenuLanguages
                        .Remove(MenuLanguages
                            .Find(z => z.LanguageId == 1)));

                _baselDb.Menus
                   .Select(x => x.MenuLanguages
                        .Remove(MenuLanguages
                            .Find(z => z.LanguageId == 2)));

                _baselDb.Menus
                   .Select(x => x.MenuLanguages
                        .Remove(MenuLanguages
                            .Find(z => z.LanguageId == 3)));


                _baselDb.Menus
                    .Remove(MenuLanguages
                        .Where(x => x.Menu.Id == id)
                            .Select(x => x.Menu)
                                .FirstOrDefault());

                _baselDb.SaveChanges();

                return View(nameof(Menu));
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpGet]
        public IActionResult MenuCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MenuCreate(MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                List<MenuLanguage> menuLanguages = new List<MenuLanguage>()
                {
                    new MenuLanguage()
                    {
                        LanguageId=1,
                        Name=menuModel.EngName
                    },
                    new MenuLanguage()
                    {
                        LanguageId=2,
                        Name=menuModel.AzName
                    },
                    new MenuLanguage()
                    {
                        LanguageId=3,
                        Name=menuModel.RuName
                    }
                };

                Menu Menu = new Menu()
                {
                    Action = menuModel.Action,
                    Controller = menuModel.Controller,
                    MenuLanguages = menuLanguages
                };

                _baselDb.Menus.Add(Menu);

                _baselDb.SaveChanges();

            }
            return View(nameof(Menu));
        }


        [HttpGet]
        public IActionResult SubMenuEdit(int? id)
        {
            try
            {
                List<SubMenuLanguage> subMenuLanguages = _baselDb.SubMenus
                    .SelectMany(x => x.SubMenuLanguages
                            .Where(z => z.SubMenu.Id == id))
                                .Include(a => a.SubMenu)
                                    .ToList();
                                    

                if (subMenuLanguages != null)
                {
                    MenuData menuData = new MenuData()
                    {
                        MenuModel = new MenuModel(),
                        Object = subMenuLanguages
                    };

                    return View(menuData);
                }
            }
            catch (Exception exp) { }

            return View();
        }

        [HttpPost]
        public IActionResult SubMenuEdit(MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                if (menuModel?.Id!=null)
                {
                    List<SubMenuLanguage> subMenuLanguages = _baselDb.SubMenus
                        .SelectMany(x => x.SubMenuLanguages
                            .Where(z => z.SubMenu.Id == menuModel.Id))
                                .Include(a => a.SubMenu)
                                    .ToList();


                    foreach (SubMenuLanguage item in subMenuLanguages)
                    {
                        if (item.LanguageId==1)
                        {
                            item.Name = menuModel.EngName;
                        }
                        else if (item.LanguageId == 2)
                        {
                            item.Name = menuModel.AzName;
                        }
                        else if (item.LanguageId == 3)
                        {
                            item.Name = menuModel.RuName;
                        }
                    }                    

                    SubMenu subMenu = subMenuLanguages.Select(x=>x.SubMenu).FirstOrDefault();

                    subMenu.Action = menuModel.Action;
                    subMenu.Controller = menuModel.Controller;

                    _baselDb.SaveChanges();

                }
            }

            return View(nameof(Menu));
        }

        [HttpGet]
        public IActionResult SubMenuDelete(int? id)
        {
            try
            {
                List<SubMenuLanguage> subMenuLanguages = _baselDb.SubMenus
                   .SelectMany(x => x.SubMenuLanguages
                           .Where(z => z.SubMenu.Id == id))
                               .Include(a => a.SubMenu)
                                   .ToList();

                return View(subMenuLanguages);
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpPost]
        public IActionResult SubMenuDelete(int? id,string name)
        {
            try
            {
                List<SubMenuLanguage> subMenuLanguages = _baselDb.SubMenus
                   .SelectMany(x => x.SubMenuLanguages
                           .Where(z => z.SubMenu.Id == id))
                               .Include(a => a.SubMenu)
                                   .ToList();

                _baselDb.SubMenus
                   .Select(x => x.SubMenuLanguages
                        .Remove(subMenuLanguages
                            .Find(z=>z.LanguageId==1)));

                _baselDb.SubMenus
                   .Select(x => x.SubMenuLanguages
                        .Remove(subMenuLanguages
                            .Find(z => z.LanguageId == 2)));

                _baselDb.SubMenus
                   .Select(x => x.SubMenuLanguages
                        .Remove(subMenuLanguages
                            .Find(z => z.LanguageId == 3)));


                _baselDb.SubMenus
                    .Remove(subMenuLanguages
                        .Where(x=>x.SubMenu.Id==id)
                            .Select(x=>x.SubMenu)
                                .FirstOrDefault());

                _baselDb.SaveChanges();

                return View(nameof(Menu));
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpGet]
        public IActionResult SubMenuCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubMenuCreate(MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                List<SubMenuLanguage> subMenuLanguages = new List<SubMenuLanguage>()
                {
                    new SubMenuLanguage()
                    {
                        LanguageId=1,
                        Name=menuModel.EngName
                    },
                    new SubMenuLanguage()
                    {
                        LanguageId=2,
                        Name=menuModel.AzName
                    },
                    new SubMenuLanguage()
                    {
                        LanguageId=3,
                        Name=menuModel.RuName
                    }
                };

                SubMenu subMenu = new SubMenu()
                {
                    MenuId=1,
                    Action=menuModel.Action,
                    Controller=menuModel.Controller,
                    SubMenuLanguages = subMenuLanguages
                };

                _baselDb.SubMenus.Add(subMenu);

                _baselDb.SaveChanges();
                
            }
            return View(nameof(Menu));
        }

        [HttpGet]
        public IActionResult HeadMenuEdit(int? id)
        {
            try
            {
                List<HeadMenuLanguage> headMenuLanguages = _baselDb.HeadMenus
                    .SelectMany(x => x.HeadMenuLanguages
                            .Where(z => z.HeadMenu.Id == id))
                                .Include(a => a.HeadMenu)
                                    .ToList();


                if (headMenuLanguages != null)
                {
                    MenuData menuData = new MenuData()
                    {
                        MenuModel = new MenuModel(),
                        Object = headMenuLanguages
                    };

                    return View(menuData);
                }
            }
            catch (Exception exp) { }

            return View();
        }

        [HttpPost]
        public IActionResult HeadMenuEdit(MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                if (menuModel?.Id != null)
                {
                    List<HeadMenuLanguage> headMenuLanguages = _baselDb.HeadMenus
                     .SelectMany(x => x.HeadMenuLanguages
                             .Where(z => z.HeadMenu.Id == menuModel.Id))
                                 .Include(a => a.HeadMenu)
                                     .ToList();


                    foreach (HeadMenuLanguage item in headMenuLanguages)
                    {
                        if (item.LanguageId == 1)
                        {
                            item.Name = menuModel.EngName;
                        }
                        else if (item.LanguageId == 2)
                        {
                            item.Name = menuModel.AzName;
                        }
                        else if (item.LanguageId == 3)
                        {
                            item.Name = menuModel.RuName;
                        }
                    }

                    HeadMenu headMenu = headMenuLanguages.Select(x => x.HeadMenu).FirstOrDefault();

                    headMenu.Action = menuModel.Action;
                    headMenu.Controller = menuModel.Controller;

                    _baselDb.SaveChanges();

                }
            }

            return View(nameof(Menu));
        }

        [HttpGet]
        public IActionResult HeadMenuDelete(int? id)
        {
            try
            { 
                List<HeadMenuLanguage> headMenuLanguages = _baselDb.HeadMenus
                   .SelectMany(x => x.HeadMenuLanguages
                           .Where(z => z.HeadMenu.Id == id))
                               .Include(a => a.HeadMenu)
                                   .ToList();

                return View(headMenuLanguages);
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpPost]
        public IActionResult HeadMenuDelete(int? id, string name)
        {
            try
            {
                List<HeadMenuLanguage> headMenuLanguages = _baselDb.HeadMenus
                  .SelectMany(x => x.HeadMenuLanguages
                          .Where(z => z.HeadMenu.Id == id))
                              .Include(a => a.HeadMenu)
                                  .ToList();

                _baselDb.HeadMenus
                   .Select(x => x.HeadMenuLanguages
                        .Remove(headMenuLanguages
                            .Find(z => z.LanguageId == 1)));

                _baselDb.HeadMenus
                   .Select(x => x.HeadMenuLanguages
                        .Remove(headMenuLanguages
                            .Find(z => z.LanguageId == 2)));

                _baselDb.HeadMenus
                   .Select(x => x.HeadMenuLanguages
                        .Remove(headMenuLanguages
                            .Find(z => z.LanguageId == 3)));


                _baselDb.HeadMenus
                    .Remove(headMenuLanguages
                        .Where(x => x.HeadMenu.Id == id)
                            .Select(x => x.HeadMenu)
                                .FirstOrDefault());

                _baselDb.SaveChanges();

                return View(nameof(Menu));
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpGet]
        public IActionResult HeadMenuCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HeadMenuCreate(MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                List<HeadMenuLanguage> headMenuLanguages = new List<HeadMenuLanguage>()
                {
                    new HeadMenuLanguage()
                    {
                        LanguageId=1,
                        Name=menuModel.EngName
                    },
                    new HeadMenuLanguage()
                    {
                        LanguageId=2,
                        Name=menuModel.AzName
                    },
                    new HeadMenuLanguage()
                    {
                        LanguageId=3,
                        Name=menuModel.RuName
                    }
                };

                HeadMenu headMenu = new HeadMenu()
                {
                    Action = menuModel.Action,
                    Controller = menuModel.Controller,
                    HeadMenuLanguages = headMenuLanguages
                };

                _baselDb.HeadMenus.Add(headMenu);

                _baselDb.SaveChanges();

            }
            return View(nameof(Menu));
        }


        [HttpGet]
        public IActionResult AccountMenuEdit(int? id)
        {
            try
            {
                List<AccountMenuLanguage> accountMenuLanguages = _baselDb.AccountMenus
                    .SelectMany(x => x.AccountMenuLanguages
                            .Where(z => z.AccountMenu.Id == id))
                                .Include(a => a.AccountMenu)
                                    .ToList();


                if (accountMenuLanguages != null)
                {
                    MenuData menuData = new MenuData()
                    {
                        MenuModel = new MenuModel(),
                        Object = accountMenuLanguages
                    };

                    return View(menuData);
                }
            }
            catch (Exception exp) { }

            return View();
        }

        [HttpPost]
        public IActionResult AccountMenuEdit(MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                if (menuModel?.Id != null)
                {
                    List<AccountMenuLanguage> accountMenuLanguages = _baselDb.AccountMenus
                     .SelectMany(x => x.AccountMenuLanguages
                             .Where(z => z.AccountMenu.Id == menuModel.Id))
                                 .Include(a => a.AccountMenu)
                                     .ToList();


                    foreach (AccountMenuLanguage item in accountMenuLanguages)
                    {
                        if (item.LanguageId == 1)
                        {
                            item.Name = menuModel.EngName;
                        }
                        else if (item.LanguageId == 2)
                        {
                            item.Name = menuModel.AzName;
                        }
                        else if (item.LanguageId == 3)
                        {
                            item.Name = menuModel.RuName;
                        }
                    }

                    AccountMenu accountMenu = accountMenuLanguages.Select(x => x.AccountMenu).FirstOrDefault();

                    accountMenu.Action = menuModel.Action;
                    accountMenu.Controller = menuModel.Controller;

                    _baselDb.SaveChanges();

                }
            }

            return View(nameof(Menu));
        }

        [HttpGet]
        public IActionResult AccountMenuDelete(int? id)
        {
            try
            {
                List<AccountMenuLanguage> accountMenuLanguages = _baselDb.AccountMenus
                    .SelectMany(x => x.AccountMenuLanguages
                            .Where(z => z.AccountMenu.Id == id))
                                .Include(a => a.AccountMenu)
                                    .ToList();

                return View(accountMenuLanguages);
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpPost]
        public IActionResult AccountMenuDelete(int? id, string name)
        {
            try
            {
                List<AccountMenuLanguage> accountMenuLanguages = _baselDb.AccountMenus
                    .SelectMany(x => x.AccountMenuLanguages
                            .Where(z => z.AccountMenu.Id == id))
                                .Include(a => a.AccountMenu)
                                    .ToList();

                _baselDb.AccountMenus
                   .Select(x => x.AccountMenuLanguages
                        .Remove(accountMenuLanguages
                            .Find(z => z.LanguageId == 1)));

                _baselDb.AccountMenus
                   .Select(x => x.AccountMenuLanguages
                        .Remove(accountMenuLanguages
                            .Find(z => z.LanguageId == 2)));

                _baselDb.AccountMenus
                   .Select(x => x.AccountMenuLanguages
                        .Remove(accountMenuLanguages
                            .Find(z => z.LanguageId == 3)));


                _baselDb.AccountMenus
                    .Remove(accountMenuLanguages
                        .Where(x => x.AccountMenu.Id == id)
                            .Select(x => x.AccountMenu)
                                .FirstOrDefault());

                _baselDb.SaveChanges();

                return View(nameof(Menu));
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }

        [HttpGet]
        public IActionResult AccountMenuCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AccountMenuCreate(MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                List<AccountMenuLanguage> accountMenuLanguages = new List<AccountMenuLanguage>()
                {
                    new AccountMenuLanguage()
                    {
                        LanguageId=1,
                        Name=menuModel.EngName
                    },
                    new AccountMenuLanguage()
                    {
                        LanguageId=2,
                        Name=menuModel.AzName
                    },
                    new AccountMenuLanguage()
                    {
                        LanguageId=3,
                        Name=menuModel.RuName
                    }
                };

                AccountMenu accountMenu = new AccountMenu()
                {
                    Action = menuModel.Action,
                    Controller = menuModel.Controller,
                    AccountMenuLanguages = accountMenuLanguages
                };

                _baselDb.AccountMenus.Add(accountMenu);

                _baselDb.SaveChanges();

            }
            return View(nameof(Menu));
        }
    }
}