using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Infastructure;
using BaselFinalProjectApp.Models.PageModel.HomePageModel;
using BaselFinalProjectApp.Models.ViewData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Components
{
    public class MenuNavViewComponent : ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;

        public MenuNavViewComponent(BaselDbContext baselDbContext)
        {
            _baselDbContext = baselDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int? cultures = HttpContext.Session.GetInt32("culture");

            if (cultures == null)
            {
                cultures = GetLanguage
                    .GetLangId(); ;
            }

            List<MenuLanguage> menu = await _baselDbContext.Menus
                .SelectMany(a => a.MenuLanguages
                    .Where(x => x.LanguageId == cultures)).Include(x => x.Menu)
                        .ToListAsync();

            List<SubMenuLanguage> submenu = await _baselDbContext.SubMenus
                .SelectMany(a => a.SubMenuLanguages
                    .Where(x => x.LanguageId == cultures))
                        .ToListAsync();

            List<HeadMenuLanguage> headMenuLanguages = await _baselDbContext.HeadMenus
                .SelectMany(a => a.HeadMenuLanguages
                    .Where(x => x.LanguageId == cultures)).Include(x => x.HeadMenu)
                        .ToListAsync();

            MenuSubMenuData menuSubMenu = new MenuSubMenuData()
            {
                SubMenuLanguages = submenu,
                MenuLanguages = menu,
                HeadMenuLanguages = headMenuLanguages
            };

            return View(menuSubMenu);
        }
    }
}
