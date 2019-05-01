using BaselFinalProjectApp.Models.MainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class CategoryData
    {
        public CategoryData()
        {
            CategoryModel = new CategoryModel();
            CategoryLanguages = new List<CategoryLanguage>();
        }

        public CategoryModel CategoryModel { get; set; }
        public List<CategoryLanguage> CategoryLanguages { get; set; }
    }
}
