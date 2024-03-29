﻿using BaselFinalProjectApp.Data;
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
    public class HeadMenuViewComponent:ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;
        public HeadMenuViewComponent(BaselDbContext baselDbContext)
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

            List<HeadMenuLanguage> headMenus = await _baselDbContext.HeadMenus
                .SelectMany(a => a.HeadMenuLanguages
                    .Where(x => x.LanguageId == cultures)).Include(x=>x.HeadMenu)
                        .ToListAsync();

            return View(headMenus);
        }
    }
}
