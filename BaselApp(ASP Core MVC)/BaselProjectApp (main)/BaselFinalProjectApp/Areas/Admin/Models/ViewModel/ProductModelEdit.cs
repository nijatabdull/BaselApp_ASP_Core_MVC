using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class ProductModelEdit : ProductModel
    {
        [DataType(DataType.Upload)]
        public List<IFormFile> Image { get; set; }
        public string OldImage { get; set; }
    }
}
