using BaselFinalProjectApp.Models.MainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class ColorData
    {
        public ColorData()
        {
            ColorsModel = new ColorsModel();
            ColorLanguages = new List<ColorLanguage>();
        }

        public ColorsModel ColorsModel { get; set; }
        public List<ColorLanguage> ColorLanguages { get; set; }
    }
}
