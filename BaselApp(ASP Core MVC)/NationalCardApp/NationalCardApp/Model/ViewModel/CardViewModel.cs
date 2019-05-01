using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NationalCardApp.Model.ViewModel
{
    public class CardViewModel
    {
        [Required]
        [StringLength(16, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 16)]
        [RegularExpression("([0-9]+)",ErrorMessage ="Please, input correct format")]
        public string CardNumber { get; set; }
        [Required]
        [RegularExpression("(?:0[1-9]|1[0-2])\\/[0-9]{2}", ErrorMessage = "Please, input correct format")]
        public string ValidTHRU { get; set; }
        [Required]
        [RegularExpression("^[0-9]{3,4}$", ErrorMessage = "Please, input correct format")]
        public string CVV { get; set; }
    }
}
