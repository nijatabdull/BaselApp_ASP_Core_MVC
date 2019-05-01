using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Infastructure;
using BaselFinalProjectApp.Models.MainModel;
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
    public class FeatureCategoryViewComponent : ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;
        public FeatureCategoryViewComponent(BaselDbContext baselDbContext)
        {
            _baselDbContext = baselDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        { 

            List<Product> products = await _baselDbContext
               .Products.Include(x => x.Images)
                    .Take(5).Include(x => x.Category)
                        .ThenInclude(z => z.CategoryLanguages)
                                .ToListAsync();
                                
            return View(products);
        }
    }
}
