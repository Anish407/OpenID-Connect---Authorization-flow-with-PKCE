using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentitySample.IDP
{
    public static class IDPData
    {

        public static IEnumerable<ApiResource> GetApiResources =>
            new List<ApiResource> { new ApiResource("ApiOne") };

        public static IEnumerable<Client> GetClients =>
                new List<Client> {
                    new Client
                    {
                         ClientId="Client_id",
                         ClientSecrets= { new Secret("CLientSecret".ToSha256()) },
                         AllowedGrantTypes=  GrantTypes.ClientCredentials,
                         AllowedScopes={ "ApiOne"}
                    }
            };
    }
}
