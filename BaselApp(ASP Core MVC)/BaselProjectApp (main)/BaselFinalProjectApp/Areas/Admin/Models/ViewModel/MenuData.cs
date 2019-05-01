using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class MenuData
    {
        public MenuData()
        {
            MenuModel = new MenuModel();
            Object = new Object();
        }

        public MenuModel MenuModel { get; set; }
        public Object Object { get; set; }
    }
}
