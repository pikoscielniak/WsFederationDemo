using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace IdServer.Config
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
                { 
                    StandardScopes.OpenId,
                    StandardScopes.ProfileAlwaysInclude,
                    StandardScopes.Address,
                    StandardScopes.OfflineAccess
                };
        }
    }
}
