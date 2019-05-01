using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Infastructure;
using BaselFinalProjectApp.Models.PageModel.AccountPageModel;
using BaselFinalProjectApp.Models.PageModel.HomePageModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Components
{
    [ViewComponent(Name = "AccountMenuDashboard")]
    public class AdminDashboardViewComponent : ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;

        public AdminDashboardViewComponent(BaselDbContext baselDbContext)
        {
            _baselDbContext = baselDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                int? cultures = HttpContext.Session.GetInt32("culture");

                if (cultures == null)
                {
                    cultures = GetLanguage
                        .GetLangId();
                }

                List<AccountMenuLanguage> accountMenuLanguages = await _baselDbContext.AccountMenus
                    .SelectMany(a => a.AccountMenuLanguages
                        .Where(x => x.LanguageId == cultures)).Include(x => x.AccountMenu)
                            .ToListAsync();

                return View(accountMenuLanguages);
            }
            catch (Exception)
            {

                throw;
            }

            
        }
    }
}
