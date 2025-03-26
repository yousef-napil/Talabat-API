using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDataSeed
    {
        public static async Task SeedDataAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser
                {
                    DisplayName = "Yousef Napil",
                    Email = "Yousefnapil995@gmail.com",
                    UserName = "Yousefnapil995",
                    PhoneNumber = "0799999999",
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");
            }
        }
    }
}
