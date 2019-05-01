using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Extentions
{
    public static class ControllerExtention
    {
        public static void SetValidateMessage(this Controller controller,IEnumerable<IdentityError> identityErrors)
        {
            foreach (IdentityError error in identityErrors)
            {
                controller.ModelState.AddModelError("",error.Description);
            }
        }
    }
}
