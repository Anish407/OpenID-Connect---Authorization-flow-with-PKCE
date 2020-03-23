using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ImageGallery.Client.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [HttpGet("Logout")]
        public async Task Logout() { 
            //this will logout from the cookie scheme (clear the cookie from the browser). But we are logged in at the IDP
            // so another request will again get authenticated.
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // this will cause the user to sign out from the IDP. THis is used as the default challenge scheme
            await this.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
