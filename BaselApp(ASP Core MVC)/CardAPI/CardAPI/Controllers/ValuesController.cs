using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaselFinalProjectApp.Models;
using CardAPI.DAL;
using CardAPI.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly CardDbContext _dbContext;

        public ValuesController(CardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            return "";
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody] string values)
        {
            try
            {
                if (values != null)
                {
                    CardViewModel cardViewModel = JsonConvert.DeserializeObject<CardViewModel>(values);

                    Card card = _dbContext.Cards
                        .Where(x => x.Number == cardViewModel.CardNumber)
                            .Where(x => x.ValidTHRU == cardViewModel.CardValidTHRU)
                                .Where(x => x.CVV == cardViewModel.CardCVV)
                                    .SingleOrDefault();

                    if (cardViewModel.TotalPrice != null)
                    {
                        string[] total = cardViewModel.TotalPrice.Split(",");

                        decimal totalPrice = Convert.ToDecimal(total[0]);

                        if (card != null)
                        {
                            if (card.Balance >= totalPrice)
                            {
                                decimal current = card.Balance;

                                current = current - totalPrice;

                                card.Balance = current;

                                _dbContext.SaveChanges();

                                return "Payed"; //go basel
                            }
                            else
                            {
                                return "Low Balance"; //go basel
                            }
                        }
                        return "Card"; //go card
                    }
                    return "Not Found TotalPrice"; //go basel
                }
                return "Card";
            }
            catch (Exception exp)
            {
                return exp.Message; 
            }           
        }
    }
}
