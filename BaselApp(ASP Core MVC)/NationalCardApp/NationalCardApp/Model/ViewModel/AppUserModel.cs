using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalCardApp.Model.ViewModel
{
    [Serializable]
    public class AppUserModel
    {
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string TotalPrice { get; set; }
        public string UserToken { get; set; }
    }
}
