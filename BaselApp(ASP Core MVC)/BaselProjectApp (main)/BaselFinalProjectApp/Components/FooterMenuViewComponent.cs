using BaselFinalProjectApp.Controllers;
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
    [ViewComponent]
    public class FooterMenuViewComponent:ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;
        public FooterMenuViewComponent(BaselDbContext baselDbContext)
        {
            _baselDbContext = baselDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int? cultures = HttpContext.Session.GetInt32("culture");

            if (cultures == null)
            {
                cultures = GetLanguage
                    .GetLangId();
            }

            List<MenuLanguage> menu = await _baselDbContext.Menus
                .SelectMany(a => a.MenuLanguages
                    .Where(x=>x.LanguageId==cultures))
                        .ToListAsync();

            List<SubMenuLanguage> submenu = await _baselDbContext.SubMenus
                .SelectMany(a => a.SubMenuLanguages
                    .Where(x => x.LanguageId == cultures))
                        .ToListAsync();

            MenuSubMenuData menuSubMenu = new MenuSubMenuData()
            {
                SubMenuLanguages= submenu,
                MenuLanguages = menu
            };

            return View(menuSubMenu);
        }
    }
}
