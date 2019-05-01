using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;

namespace BaselFinalProjectApp.Infastructure
{
    public static class GetLanguage
    {
        private static IHttpContextAccessor _contextAccessor;
        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        private static int LangId = 1;
        public static int GetLangId()
        {
            var requestCulture = _contextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();

            string name = requestCulture.RequestCulture.UICulture.Name;

            return GetId(name);
        }
        public static int GetLangId(string name)
        {
            return GetId(name);
        }


        private static int GetId(string name)
        {
            if (name == "en")
                LangId = 1;
            else if (name == "az")
                LangId = 2;
            else if (name == "ru")
                LangId = 3;
            else
                LangId = 1;

            return LangId;
        }
    }
}
