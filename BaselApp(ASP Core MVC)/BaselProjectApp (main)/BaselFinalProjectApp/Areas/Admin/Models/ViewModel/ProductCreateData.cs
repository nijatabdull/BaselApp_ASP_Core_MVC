using BaselFinalProjectApp.Models.MainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class ProductCreateData
    {
        public ProductCreateData()
        {
            ColorModels = new List<ColorModel>();
            ProductModelCreate = new ProductModelCreate();
            CategoryLanguages = new List<CategoryLanguage>();
        }

        public List<ColorModel> ColorModels { get; set; }
        public ProductModelCreate ProductModelCreate { get; set; }
        public List<CategoryLanguage> CategoryLanguages { get; set; }
    }
}
