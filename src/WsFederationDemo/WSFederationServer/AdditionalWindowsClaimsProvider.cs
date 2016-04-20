using System.Threading.Tasks;
using IdentityServer.WindowsAuthentication.Services;

namespace WSFederationServer
{
    public class AdditionalWindowsClaimsProvider : ICustomClaimsProvider
    {
        public Task TransformAsync(CustomClaimsProviderContext context)
        {
            return Task.FromResult(0);
        }
    }
}