// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Marvin.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            { 
                //subjectId
                new IdentityResources.OpenId(),
                //Givenname and family name 
                new IdentityResources.Profile(),
                new IdentityResources.Address(),

                new IdentityResource(
                    "userrole", // name 
                    "Role for the user", // name to be displayed 
                    new List<string> { "role"} // type that will be returned
                    )
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
              new ApiResource(
                  "imageGalleryApi",
                  "Image gallery api" , 
                  new List<string>{ "role"} // these roles will need to be requested when requesting this resource
                  )
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
              new Client
              {
                  // this will be displayed on the consent screen
                  ClientName="Image Gallery",
                  ClientId="imagegalleryclient",

                  // type of flow
                  AllowedGrantTypes=GrantTypes.Code,
                  RequirePkce=true,

                  // were to redirect on the client after signin
                  RedirectUris={
                      "https://localhost:44389/signin-oidc"
                  },
                  PostLogoutRedirectUris={ "https://localhost:44389/signout-callback-oidc" },
                  // wat are the scopes this client can request
                  AllowedScopes= {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,

                        // make the client request for access to the users role claim
                        "userrole",
                        "imageGalleryApi"
                  },
                  ClientSecrets= { new Secret("secret".ToSha256()) }
              }

            };

    }
}