using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace ImageGallery.Client
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // to clear default claim type mappings.. 
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                 .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            // create an HttpClient used for accessing the API
            services.AddHttpClient("APIClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44366/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });

            services.AddHttpClient("IDPClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44350/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
               .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, o =>
               {
                   o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                   o.Authority = "https://localhost:44350/";
                   o.ClientId = "imagegalleryclient";
                   o.ClientSecret = "secret";
                   o.ResponseType = "code";
                   o.UsePkce = true;

                   //we can add this wen we want to change the RedirectUris configured in the IDP
                   //o.CallbackPath = "";

                   //Request these from the userinfo endpoint
                   //o.Scope.Add("openid");
                   //o.Scope.Add("profile");
                   o.Scope.Add("address");
                   o.Scope.Add("userrole");
                 
                   //Confusion on why remove is used
                   // this is to remove the nbf from the identity token.
                   //TO make the token smaller remove claims that are not needed.
                   //o.ClaimActions.Remove("nbf");
                   o.ClaimActions.Remove("address");
                   o.ClaimActions.Remove("role");
                   //o.ClaimActions.Remove("amr");

                   //remove from Claims pricipal
                   o.ClaimActions.DeleteClaim("amr");

                   //To map the newly added claim to claims principal
                   o.ClaimActions.MapUniqueJsonKey("role", "role");

                   // call userinfoendpoint to get extra claims 
                   // this is done to make the IDTcoken smaller.
                   o.GetClaimsFromUserInfoEndpoint = true;

                   o.SaveTokens = true;
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
                // The default HSTS value is 30 days. You may want to change this for
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Gallery}/{action=Index}/{id?}");
            });
        }
    }
}
