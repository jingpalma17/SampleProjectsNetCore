// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Models;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.Data.Seed
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1"){
                    Scopes =
                    {
                        Scopes.Article.Read,
                        Scopes.Article.Write,
                        Scopes.Identity.Read
                    }
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new ApiScope[]
            {
                new ApiScope(name: Scopes.Article.Read, displayName: "read to article api."),
                new ApiScope(name: Scopes.Article.Write, displayName: "write to article api."),
                new ApiScope(name: Scopes.Identity.Read, displayName: "read to identity api.")
            };
        }

        public static IEnumerable<Client> GetClients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = {"https://localhost:52402/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"https://localhost:52402"},
                    PostLogoutRedirectUris = {"https://localhost:52402/swagger/" },
                    AllowedScopes =
                    {
                        Scopes.Article.Read,
                        Scopes.Article.Write,
                        Scopes.Identity.Read
                    }
                },
                new Client
                {
                    ClientId = "client1",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        Scopes.Article.Read,
                        Scopes.Article.Write,
                        Scopes.Identity.Read
                    }
                },
                new Client
                {
                    ClientId = "client2",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        Scopes.Article.Read,
                        Scopes.Article.Write,
                        Scopes.Identity.Read
                    }
                },
            };
    }
}