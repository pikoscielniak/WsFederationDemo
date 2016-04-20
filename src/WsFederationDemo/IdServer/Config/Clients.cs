using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace IdServer.Config
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new Client[]
            {
                  new Client
                {
                    ClientId = "demohybrid",
                    ClientName = "Demo (Hybrid)",
                    Flow = Flows.Hybrid,
                    AllowAccessToAllScopes = true,
                    IdentityTokenLifetime = 3600,
                    AccessTokenLifetime = 3600,
                    RequireConsent = false,
                                   
                    // redirect = URI of the MVC application
                    RedirectUris = new List<string>
                    {
                        "http://localhost:9881/Home/Protected"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                         "http://localhost:9881/Home/Index"
                    },
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };
        }
    }
}
