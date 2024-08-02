using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace Discussify.API.Service
{
    public static class DataSeeder
    {
        public static async Task<WebApplication> Seed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var user = ApplicationUser.Create("Dennis", "Dao", "test@email.com");

                var result = await userManager.CreateAsync(user, "Password123$");

                var category1 = Category.Create("Javascript");
                var category2 = Category.Create("C#");

                context.Add(category1);
                context.Add(category2);
                context.SaveChanges();

                var tag1 = Tag.Create("finace");
                var tag2 = Tag.Create("bitcoin");

                context.Add(tag1);
                context.Add(tag2);
                context.SaveChanges();


                var post = Post.Create(user.Id, "SQL Injection", "pizza");
                post.ChangeImage("test");
                post.SetCategory(category1);
                context.Add(post);
                context.SaveChanges();

                post.AddTags(tag1);
                post.AddTags(tag2);

               
                context.Update(post);
                context.SaveChanges();
            }

            return app;
        }
    }
}
