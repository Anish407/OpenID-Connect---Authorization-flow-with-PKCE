using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ImageGallery.Client.Handlers
{
    /// <summary>
    /// this will automatically add the bearer token to the httpClient object for the APICLient
    /// We will associate this handler to that HttpClient
    /// </summary>
    public class HttpRequestHandler : DelegatingHandler
    {
        public HttpRequestHandler(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        /// <summary>
        /// Get the token and add it to the HttpRequest
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await HttpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken)
                 ?? throw new Exception("Access token not present in the context");

            request.SetBearerToken(token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
