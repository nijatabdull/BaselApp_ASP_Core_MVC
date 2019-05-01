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
    public class ProductOptionViewComponent : ViewComponent
    {
        private readonly BaselDbContext _baselDbContext;
        public ProductOptionViewComponent(BaselDbContext baselDbContext)
        {
            _baselDbContext = baselDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {                       
            int? ProductId = HttpContext.Session.GetInt32("ProductId");
          
            Product products = await _baselDbContext
                .Products.Include(x => x.Images)
                    .Where(x => x.Id == ProductId).Where(z => z.Stock > 0)
                    .Include(x => x.ProductLanguages).Include(z => z.Measures)
                        .Include(z => z.Reviews).Include(x => x.ProductColors)
                            .Include(x => x.Category).ThenInclude(z => z.CategoryLanguages)
                            .Include(x=>x.ProductWishLists)
                                .SingleOrDefaultAsync();

            List<Color> colors = await _baselDbContext.Colors
                .Include(z=>z.ColorLanguages).ToListAsync();

            return View(products);
        }
    }
}
