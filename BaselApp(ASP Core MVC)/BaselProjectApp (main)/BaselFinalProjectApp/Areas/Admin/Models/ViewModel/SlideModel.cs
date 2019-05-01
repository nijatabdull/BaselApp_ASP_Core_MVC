using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public abstract class SlideModel
    {
        public int Id { get; set; }
        [Required]
        public string EngHeader { get; set; }
        [Required]
        public string AzHeader { get; set; }
        [Required]
        public string RuHeader { get; set; }
        public string EngTitle { get; set; }
        public string AzTitle { get; set; }
        public string RuTitle { get; set; }
        public string EngDescription { get; set; }
        public string AzDescription { get; set; }
        public string RuDescription { get; set; }
        public string EngFooter { get; set; }
        public string AzFooter { get; set; }
        public string RuFooter { get; set; }
    }
}
