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
    public class HomeSocialAccountViewComponent : ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;
        public HomeSocialAccountViewComponent(BaselDbContext baselDbContext)
        {
            _baselDbContext = baselDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<HomeSocialAccount> homeSocials = await _baselDbContext
                                                        .HomeSocialAccounts
                                                            .ToListAsync();

            return View(homeSocials);
        }
    }
}
