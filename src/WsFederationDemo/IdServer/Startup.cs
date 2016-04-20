using System;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdServer.Config;
using Microsoft.Owin;
using Microsoft.Owin.Security.WsFederation;
using Owin;

[assembly: OwinStartup(typeof(IdServer.Startup))]

namespace IdServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/identity", idsrvApp =>
            {
                var corsPolicyService = new DefaultCorsPolicyService { AllowAll = true };

                var idServerServiceFactory = new IdentityServerServiceFactory
                {
                    CorsPolicyService = new Registration<ICorsPolicyService>(corsPolicyService)
                }
                        .UseInMemoryClients(Clients.Get())
                        .UseInMemoryScopes(Scopes.Get())
                        .UseInMemoryUsers(Users.Get());

                idServerServiceFactory.ConfigureDefaultViewService(
                    new DefaultViewServiceOptions { CacheViews = false });

                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "WsFederationDemo",
                    SigningCertificate = LoadCertificate(),
                    RequireSsl = false,
                    Factory = idServerServiceFactory,
                    AuthenticationOptions = new AuthenticationOptions
                    {
                        RememberLastUsername = true,
                        IdentityProviders = ConfigureAdditionalIdProviders
                    },
                    EnableWelcomePage = false,
                    CspOptions = new CspOptions
                    {
                        Enabled = false
                    }
                });
            });
        }

        private static void ConfigureAdditionalIdProviders(IAppBuilder appBuilder, string signInAsType)
        {
            const string wsFederationUrl = "http://localhost:9877/";
            var windowsAuthentication = new WsFederationAuthenticationOptions
            {
                AuthenticationType = "windows",
                Caption = "Windows User",
                SignInAsAuthenticationType = signInAsType,
                MetadataAddress = wsFederationUrl,
                Wtrealm = "urn:win"
            };

            appBuilder.UseWsFederationAuthentication(windowsAuthentication);
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                $@"{AppDomain.CurrentDomain.BaseDirectory}\certificates\idsrv3test.pfx", "idsrv3test");
        }
    }
}
