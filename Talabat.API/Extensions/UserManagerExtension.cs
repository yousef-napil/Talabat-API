using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;

namespace Talabat.API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser?> GetUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(x => x.Address)
                                        .FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }
    }
}
