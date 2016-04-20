using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;

namespace IdServer.Config
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>() {
                 
                new InMemoryUser
	            {
	                Username = "alice",
	                Password = "alice",                    
	                Subject = "b05d3546-6ca8-4d32-b95c-77e94d705ddf",
                    Claims = new[]
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Alice"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Smith"),
                        new Claim(Constants.ClaimTypes.Address, "1, Main Street, Antwerp, Belgium"),
                        new Claim("role", "PayingUser")                  
                    }
	             }
            };
        }
    }

}
