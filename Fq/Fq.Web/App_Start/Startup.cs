using Abp.Owin;
using Fq.Api.Controllers;
using Fq.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Fq.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // System.Data.Entity.Infrastructure.SqlConnectionFactory
           
            app.UseAbp();

            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.MapSignalR();
        }
    }
}