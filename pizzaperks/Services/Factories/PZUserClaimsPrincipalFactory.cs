using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using pizzaperks.Models;
using System.Security.Claims;

namespace pizzaperks.Services.Factories
{
    public class PZUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<PZUser, IdentityRole>
    {

        public PZUserClaimsPrincipalFactory(UserManager<PZUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }


        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(PZUser user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim("CartId", user.CartId.ToString()));

            return identity;
        }


    }
}
