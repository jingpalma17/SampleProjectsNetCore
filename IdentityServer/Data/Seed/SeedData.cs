// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace IdentityServer.Data.Seed
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var userRole = roleMgr.FindByNameAsync("user").Result;
                    if (userRole == null)
                    {
                        userRole = new IdentityRole
                        {
                            Name = "user"
                        };
                        _ = roleMgr.CreateAsync(userRole).Result;
                    }

                    var adminRole = roleMgr.FindByNameAsync("admin").Result;
                    if (adminRole == null)
                    {
                        adminRole = new IdentityRole
                        {
                            Name = "admin"
                        };
                        _ = roleMgr.CreateAsync(adminRole).Result;
                    }

                    var employeeRole = roleMgr.FindByNameAsync("employee").Result;
                    if (employeeRole == null)
                    {
                        employeeRole = new IdentityRole
                        {
                            Name = "employee"
                        };
                        _ = roleMgr.CreateAsync(employeeRole).Result;
                    }

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var user1 = userMgr.FindByNameAsync("user1").Result;
                    if (user1 == null)
                    {
                        user1 = new ApplicationUser
                        {
                            UserName = "user1",
                            Email = "user1@email.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user1, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user1, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "User1 User"),
                            new Claim(JwtClaimTypes.GivenName, "User1"),
                            new Claim(JwtClaimTypes.FamilyName, "User"),
                            new Claim(JwtClaimTypes.WebSite, "http://user1.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        if (!userMgr.IsInRoleAsync(user1, userRole.Name).Result)
                        {
                            _ = userMgr.AddToRoleAsync(user1, userRole.Name).Result;
                        }

                        Log.Debug("user1 created");
                    }
                    else
                    {
                        Log.Debug("user1 already exists");
                    }

                    var admin1 = userMgr.FindByNameAsync("admin1").Result;
                    if (admin1 == null)
                    {
                        admin1 = new ApplicationUser
                        {
                            UserName = "admin1",
                            Email = "admin1@email.com",
                            EmailConfirmed = true
                        };
                        var result = userMgr.CreateAsync(admin1, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(admin1, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Admin1 Admin"),
                            new Claim(JwtClaimTypes.GivenName, "Admin1"),
                            new Claim(JwtClaimTypes.FamilyName, "Admin"),
                            new Claim(JwtClaimTypes.WebSite, "http://admin1.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        if (!userMgr.IsInRoleAsync(admin1, adminRole.Name).Result)
                        {
                            _ = userMgr.AddToRoleAsync(admin1, adminRole.Name).Result;
                        }
                        Log.Debug("Admin1 created");
                    }
                    else
                    {
                        Log.Debug("Admin1 already exists");
                    }

                    var employee1 = userMgr.FindByNameAsync("employee1").Result;
                    if (employee1 == null)
                    {
                        employee1 = new ApplicationUser
                        {
                            UserName = "employee1",
                            Email = "employee1@email.com",
                            EmailConfirmed = true
                        };
                        var result = userMgr.CreateAsync(employee1, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(employee1, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Employee1 Employee"),
                            new Claim(JwtClaimTypes.GivenName, "Employee1"),
                            new Claim(JwtClaimTypes.FamilyName, "Employee"),
                            new Claim(JwtClaimTypes.WebSite, "http://employee1.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        if (!userMgr.IsInRoleAsync(employee1, employeeRole.Name).Result)
                        {
                            _ = userMgr.AddToRoleAsync(employee1, employeeRole.Name).Result;
                        }
                        Log.Debug("Employee1 created");
                    }
                    else
                    {
                        Log.Debug("Employee1 already exists");
                    }
                }
            }
        }
    }
}
