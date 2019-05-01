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
    public class AccountMenusViewComponent : ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;
        private readonly UserManager<AppUser> _userManager;

        public AccountMenusViewComponent(BaselDbContext baselDbContext,
                                    UserManager<AppUser> roleManager)
        {
            _baselDbContext = baselDbContext;
            _userManager = roleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<AccountMenuLanguage> menu = await _baselDbContext.AccountMenus
               .SelectMany(a => a.AccountMenuLanguages.Where(z=>z.LanguageId==1))
                    .Include(x => x.AccountMenu)
                        .ToListAsync();

            return View(menu);
        }
    }
}
