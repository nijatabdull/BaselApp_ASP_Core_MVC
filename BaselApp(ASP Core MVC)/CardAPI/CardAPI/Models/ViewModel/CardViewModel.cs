using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardAPI.Models.ViewModel
{
    public class CardViewModel
    {
        public string CardNumber { get; set; }
        public string CardValidTHRU { get; set; }
        public string CardCVV { get; set; }
        public string TotalPrice { get; set; }
    }
}
