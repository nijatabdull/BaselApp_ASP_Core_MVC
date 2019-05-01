using BaselFinalProjectApp.Data;
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
    public class RelateProductViewComponent : ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;
        public RelateProductViewComponent(BaselDbContext baselDbContext)
        {
            _baselDbContext = baselDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {                     
            int? CategoryId = HttpContext.Session.GetInt32("CatId");

            List<Product> products = await _baselDbContext
                .Products.Include(x => x.Images)
                    .Where(x => x.CategoryId == CategoryId)
                        .Where(x=>x.Stock>0).Include(z=>z.ProductLanguages)
                            .ToListAsync();

            return View(products);
        }
    }
}
