using BaselFinalProjectApp.Data;
using BaselFinalProjectApp.Models.MainModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaselFinalProjectApp.Infastructure
{
    public class DefaultUser
    {
        private UserManager<AppUser> _userManager { get; set; }
        private RoleManager<IdentityRole> _roleManager { get; set; }

        public DefaultUser(UserManager<AppUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                    BaselDbContext baselDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private IdentityRole IdentityRole { get; set; }

        public DefaultUser()
        {
            IdentityRole = new IdentityRole();
        }

        public async Task SetDefaultUser(IServiceScope serviceScope, BaselDbContext _baselDb)
        {
            if (!_baselDb.Users.Any())
            {
                using (var RoleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>())
                {
                    using (var UserManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>())
                    {
                        bool IsHasRole = await RoleManager.RoleExistsAsync("Admin");

                        if (!IsHasRole)
                        {
                            IdentityRole.Name = "Admin";

                            await RoleManager.CreateAsync(IdentityRole);

                            AppUser appUser = new AppUser()
                            {
                                UserName = "Nicat",
                                Email = "nicata099@gmail.com"
                            };

                            await UserManager
                                    .CreateAsync(appUser, "Nicat12345@");

                            await UserManager.AddToRoleAsync(appUser, "Admin");
                        }

                    }
                }
            }
            
        }
   
    }
}
