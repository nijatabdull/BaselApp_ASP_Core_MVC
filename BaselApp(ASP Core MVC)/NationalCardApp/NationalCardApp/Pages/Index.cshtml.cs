using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NationalCardApp.Model.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace NationalCardApp.Pages
{
    [Produces("application/json")]
    public class IndexModel : PageModel
    {
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        [BindProperty]
        public string UserTotalPrice { get; set; }
        [BindProperty]
        public string UserId { get; set; }

        [HttpGet]
        [Route("Index/{data?}")]
        public IActionResult OnGet(string userName, string userLastName, string userTotalPrice,string userId)
        {
            if (userName!= null && userLastName!=null && userTotalPrice != null)
            {
                UserName = userName;
                UserLastName = userLastName;
                UserTotalPrice = userTotalPrice;
                UserId = userId;

                return Page();
            }
            return Page();
        }

        [BindProperty]
        public CardViewModel CardViewModel { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = new
                    {
                        cardNumber = CardViewModel.CardNumber,
                        cardValidTHRU = CardViewModel.ValidTHRU,
                        cardCVV = CardViewModel.CVV,
                        totalPrice = UserTotalPrice
                    };

                    string jsonData = JsonConvert.SerializeObject(data);

                    var restClient = new RestClient("https://localhost:44388/api/values/");

                    var restRequest = new RestRequest
                    {
                        Method = Method.POST,
                        RequestFormat = DataFormat.Json
                    };

                    restRequest.AddBody(jsonData);

                    IRestResponse restResponse = restClient.Execute(restRequest);

                    var content = restResponse.Content;

                    string info = JsonConvert.DeserializeObject<string>(content);

                    if (info == "Card")
                    {
                        ModelState.AddModelError("", "Cart information is incorrect");
                        return Page();
                    }
                    else
                        if (info == "Low Balance" || info == "Payed" || info == "Not Found TotalPrice")
                        {
                            return Redirect("https://localhost:44381/Cart/OrderCheckout/?" + "result=" + info + "&userId=" + UserId);
                        }
                    else
                    {
                        if (info!=null)
                        {
                            ModelState.AddModelError("", info);
                        }
                        return Page();
                    }
                }
                catch (Exception exp)
                {
                    ModelState.AddModelError("", exp.Message);
                    return Page();
                }            
            }
            return Page();
        }
    }
}