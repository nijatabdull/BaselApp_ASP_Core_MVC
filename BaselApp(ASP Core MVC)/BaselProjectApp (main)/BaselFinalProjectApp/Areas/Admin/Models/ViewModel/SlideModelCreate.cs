using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class SlideModelCreate : SlideModel
    {
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
