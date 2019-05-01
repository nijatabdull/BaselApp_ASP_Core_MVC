using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Infastructure;
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
    public class LatestNewsViewComponent:ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;
        public LatestNewsViewComponent(BaselDbContext baselDbContext)
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

            List<LatestNewsLanguage> LatestNewsLanguages = await _baselDbContext.LatesNews
                .SelectMany(a => a.LatestNewsLanguages
                    .Where(x => x.LanguageId == cultures)).Include(x => x.LatestNews)
                        .ToListAsync();

            return View(LatestNewsLanguages);
        }
    }
}
