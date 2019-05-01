using BaselFinalProjectApp.Models.MainModel;
using BaselFinalProjectApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class AdminData
    {
        public AdminData()
        {
            AppUser = new AppUser();
            RegisterModel = new RegisterModel();
        }

        public AppUser AppUser { get; set; }
        public RegisterModel RegisterModel { get; set; }
    }
}
