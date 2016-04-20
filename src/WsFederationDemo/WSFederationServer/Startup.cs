using System;
using System.Security.Cryptography.X509Certificates;
using IdentityServer.WindowsAuthentication.Configuration;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WSFederationServer.Startup))]

namespace WSFederationServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            const string idSrvUrl = "http://localhost:9872/identity/was";   
            const string wsFederationUri = "http://localhost:9877/";

            app.UseWindowsAuthenticationService(new WindowsAuthenticationOptions
            {
                IdpRealm = "urn:win",
                IdpReplyUrl = idSrvUrl,
                PublicOrigin = wsFederationUri,
                SigningCertificate = LoadCertificate(),
                CustomClaimsProvider = new AdditionalWindowsClaimsProvider()
            });
        }

        static X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                $@"{AppDomain.CurrentDomain.BaseDirectory}\certificates\idsrv3test.pfx", "idsrv3test");
        }
    }
}
