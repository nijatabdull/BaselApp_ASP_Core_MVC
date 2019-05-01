using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class ProductModelCreate : ProductModel
    {
        [Required]
        [DataType(DataType.Upload)]
        public List<IFormFile> Image { get; set; }        
    }

    public class ColorModel
    {
        public bool IsOptionSelected { get; set; } = false;
        [Required]
        public int OptionId { get; set; }
        public string OptionName { get; set; }
    };
}
