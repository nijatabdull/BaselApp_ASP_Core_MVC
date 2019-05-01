using BaselFinalProjectApp.Areas.Admin.Models.ViewModel;
using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Infastructure;
using BaselFinalProjectApp.Models.MainModel;
using BaselFinalProjectApp.Models.PageModel.AccountPageModel;
using BaselFinalProjectApp.Models.PageModel.HomePageModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaselFinalProjectApp.Models.ViewModels;

namespace BaselFinalProjectApp.Areas.Admin.Components
{
    [Area("Admin")]
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;
        private readonly UserManager<AppUser> _userManager;

        public MainMenuViewComponent(BaselDbContext baselDbContext,
                                    UserManager<AppUser> roleManager)
        {
            _baselDbContext = baselDbContext;
            _userManager = roleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<MenuLanguage> menu = await _baselDbContext.Menus
               .SelectMany(a => a.MenuLanguages.Where(z=>z.LanguageId==1))
                    .Include(x => x.Menu)
                        .ToListAsync();

            return View(menu);
        }
    }
}
