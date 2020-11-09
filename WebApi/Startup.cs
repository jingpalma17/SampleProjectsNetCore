using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using WebApi.Models;
using WebApi.Utils;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebApiAppContext>(options =>
                options.UseSqlServer("Server=127.0.0.1,1433;Initial Catalog=WebApi;User ID=sa;Password=Password123!;MultipleActiveResultSets=true"));

            services.AddAuthentication("Bearer")
               .AddIdentityServerAuthentication(options =>
               {
                   options.ApiName = "api1";
                   options.Authority = "https://localhost:5001";
                   options.RequireHttpsMetadata = false;
               });

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy", corsOptions =>
                {
                    corsOptions.SetIsOriginAllowed(host => true)
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                });
            });

            services.AddControllers()
                    .AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter()));


            services.AddOpenApiDocument(options =>
            {
                options.DocumentName = "v1";
                options.Title = "Protected API";
                options.Version = "v1";

                options.AddSecurity("oauth2", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = "https://localhost:5001/connect/authorize",
                            TokenUrl = "https://localhost:5001/connect/token",
                            Scopes = new Dictionary<string, string>
                            {
                                { Scopes.Article.Read, "Article API - Read access" },
                                { Scopes.Article.Write, "Article API - Write access" },
                                { Scopes.Identity.Read, "Identity API - Read access" },
                                { "profile", "profile" },
                            },
                        },
                        Password = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = "https://localhost:5001/connect/authorize",
                            TokenUrl = "https://localhost:5001/connect/token",
                            Scopes = new Dictionary<string, string>
                            {
                                { Scopes.Article.Read, "Article API - Read access" },
                                { Scopes.Article.Write, "Article API - Write access" },
                                { Scopes.Identity.Read, "Identity API - Read access1" },
                                { "profile", "profile" },
                            },
                        },
                        ClientCredentials = new OpenApiOAuthFlow()
                        {
                            TokenUrl = "https://localhost:5001/connect/token",
                            Scopes = new Dictionary<string, string>
                            {
                                { Scopes.Article.Read, "Article API - Read access" },
                                { Scopes.Article.Write, "Article API - Write access" },
                                { Scopes.Identity.Read, "Identity API - Read access" }
                            },
                        },
                    }
                });

                options.OperationProcessors.Add(new OperationSecurityScopeProcessor("oauth2"));
            });


            services.AddAuthorization(AuthorizationHelper.BuildPolicies);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();

            app.UseSwaggerUi3(options =>
            {
                options.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = "client",
                    ClientSecret = null,
                    AppName = "Demo API - Swagger",
                    UsePkceWithAuthorizationCodeGrant = true
                };
            });


            app.UseCors("DefaultCorsPolicy");

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
