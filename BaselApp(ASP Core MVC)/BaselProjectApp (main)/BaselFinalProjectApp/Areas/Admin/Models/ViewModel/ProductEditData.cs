using BaselFinalProjectApp.Models.MainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class ProductEditData
    {
        public ProductEditData()
        {
            ColorModels = new List<ColorModel>();
            ProductModelEdit = new ProductModelEdit();
            CategoryLanguages = new List<CategoryLanguage>();
            ProductLanguages = new List<ProductLanguage>();
        }

        public List<ColorModel> ColorModels { get; set; }
        public ProductModelEdit ProductModelEdit { get; set; }
        public List<CategoryLanguage> CategoryLanguages { get; set; }
        public List<ProductLanguage> ProductLanguages { get; set; }
    }
}
