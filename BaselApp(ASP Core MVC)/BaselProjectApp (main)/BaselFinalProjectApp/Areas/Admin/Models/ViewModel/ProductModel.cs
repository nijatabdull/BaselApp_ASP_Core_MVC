using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Areas.Admin.Models.ViewModel
{
    public abstract class ProductModel
    {
        public int Id { get; set; }
        [Required]
        public string EngName { get; set; }
        [Required]
        public string AzName { get; set; }
        [Required]
        public string RuName { get; set; }
        public string EngDetail { get; set; }
        public string AzDetail { get; set; }
        public string RuDetail { get; set; }
        public string EngAbout { get; set; }
        public string AzAbout { get; set; }
        public string RuAbout { get; set; }
        public string EngBenefit { get; set; }
        public string AzBenefit { get; set; }
        public string RuBenefit { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        public string Size { get; set; }
        [Required]
        public string SKU { get; set; }
        public string Chest { get; set; }
        public string Waist { get; set; }
        public string Hip { get; set; }
        public string Height { get; set; }
        public string Age { get; set; }
        [Required]
        public int ColorId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<ColorModel> ColorModels { get; set; }
    }
}
