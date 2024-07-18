using Domain.AggegratesModel.UserAggegrate;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace Discussify.API.Service
{
    public static class DataSeeder
    {
        public static WebApplication Seed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var user = ApplicationUser.Create("Dennis", "Dao", "test@email.com");

                var result = userManager.CreateAsync(user, "Password123$").Result;
            }

            return app;
        }
    }
}
