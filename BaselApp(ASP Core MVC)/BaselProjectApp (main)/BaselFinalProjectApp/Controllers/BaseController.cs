using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaselFinalProjectApp.Infastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;


namespace BaselFinalProjectApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int LangId { get; set; }
        public IActionResult SetLanguage(string culture, string returnUrl)
        {        
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            LangId = GetLanguage.GetLangId(culture);

            if (culture == null)
                LangId = 1;

            HttpContext.Session.SetInt32("culture", LangId);

            return LocalRedirect(returnUrl);
        }
    }
}