using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Models
{
    public class Card
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(16,ErrorMessage = "The {0} must be {2} characters long.", MinimumLength =16)]
        [RegularExpression("([0-9]+)")]
        public string Number { get; set; }
        [Required]
        [RegularExpression("?:0[1-9]|1[0-2])\\/[0-9]{2}")]
        public string ValidTHRU { get; set; }
        [Required]
        [RegularExpression("/^[0-9]{3,4}$/")]
        public string CVV { get; set; }
        public decimal Balance { get; set; }  
        [Required]
        public bool IsUsed { get; set; }
        public DateTime ActivatedDate { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime Passivated { get; set; }
    }
}
