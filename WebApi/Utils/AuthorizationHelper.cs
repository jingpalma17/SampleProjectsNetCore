using System;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models;

namespace WebApi.Utils
{
    public static class AuthorizationHelper
    {
        public static void BuildPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(Policies.Article.ReadOnly, builder =>
            {
                builder.RequireClaim("scope", Scopes.Article.Read);

            });

            options.AddPolicy(Policies.Article.Write, builder =>
            {
                builder.RequireClaim("scope", Scopes.Article.Write);
            });

            options.AddPolicy(Policies.Identity.ReadOnly, builder =>
            {
                builder.RequireClaim("scope", Scopes.Identity.Read);
            });
        }
    }
}
