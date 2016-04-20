using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

[assembly: OwinStartup(typeof(MVCClient.Startup))]

namespace MVCClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            AntiForgeryConfig.UniqueClaimTypeIdentifier =
                IdentityModel.JwtClaimTypes.Name;

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                ExpireTimeSpan = new TimeSpan(0, 30, 0),
                SlidingExpiration = true
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {

                ClientId = "demohybrid",
                Authority = "http://localhost:9872/identity",
                RedirectUri = "http://localhost:9881/Home/Protected",
                SignInAsAuthenticationType = "Cookies",
                ResponseType = "code id_token token",
                Scope = "openid profile offline_access",
                UseTokenLifetime = false,
                PostLogoutRedirectUri = "http://localhost:9881/Home/Index",

                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    SecurityTokenValidated = n =>
                    {
                        Helpers.TokenHelper.DecodeAndWrite(n.ProtocolMessage.IdToken);
                        Helpers.TokenHelper.DecodeAndWrite(n.ProtocolMessage.AccessToken);

                        var givenNameClaim = n.AuthenticationTicket
                            .Identity.FindFirst(IdentityModel.JwtClaimTypes.GivenName);

                        var newClaimsIdentity = new ClaimsIdentity(
                           n.AuthenticationTicket.Identity.AuthenticationType,
                           IdentityModel.JwtClaimTypes.Name,
                           IdentityModel.JwtClaimTypes.Role);

                        if (givenNameClaim != null)
                        {
                            newClaimsIdentity.AddClaim(givenNameClaim);
                        }
        
                        newClaimsIdentity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                        // create a new authentication ticket, overwriting the old one.
                        n.AuthenticationTicket = new AuthenticationTicket(
                                                 newClaimsIdentity,
                                                 n.AuthenticationTicket.Properties);
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = n =>
                    {
                        // get id token to add as id token hint
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var identityTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

                            if (identityTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = identityTokenHint.Value;
                            }
                        }
                        else if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.AuthenticationRequest)
                        {
                            string existingAcrValues = null;
                            if (n.ProtocolMessage.Parameters.TryGetValue("acr_values", out existingAcrValues))
                            {
                                // add "2fa" - acr_values are separated by a space
                                n.ProtocolMessage.Parameters["acr_values"] = existingAcrValues + " 2fa";
                            }

                            n.ProtocolMessage.Parameters["acr_values"] = "2fa";
                        }
                        return Task.FromResult(0);
                    }                    
                }
            });
        }
    }
}
