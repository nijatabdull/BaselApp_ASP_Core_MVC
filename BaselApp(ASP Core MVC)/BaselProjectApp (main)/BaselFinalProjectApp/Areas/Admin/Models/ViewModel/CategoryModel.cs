using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required]
        public string EngName { get; set; }
        [Required]
        public string AzName { get; set; }
        [Required]
        public string RuName { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}
