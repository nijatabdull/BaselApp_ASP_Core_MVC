using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Infastructure;
using BaselFinalProjectApp.Models.PageModel.HomePageModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Components
{
    public class BaselLogoViewComponent : ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;
        public BaselLogoViewComponent(BaselDbContext baselDbContext)
        {
            _baselDbContext = baselDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                PageLogo pageLogo = await _baselDbContext.PageLogos.FirstOrDefaultAsync();

                return View(pageLogo);
            }
            catch (Exception exp)
            {
                return View(exp.Message);
            }
        }
    }
}
