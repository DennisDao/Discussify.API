using Domain.AggegratesModel.NotificationAggegrate;
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

                var user = ApplicationUser.Create("Dennis", "Dao", "test@outlook.com");
                user.ChangeAvatar("eb1d6ffa-bc53-4562-8980-bbaeb8825dc8_Screenshot2024-04-26111039.png");
                
                var result = await userManager.CreateAsync(user, "Password123$");

                var category1 = Category.Create("Javascript", "Javascript", "javascript.png");
                var category2 = Category.Create("C#", "Csharp", "csharp.png");
                var category3 = Category.Create("CSS", "CSS", "css.png");
                var category4 = Category.Create("React", "React", "react.png");
                var category5 = Category.Create("HTML 5", "HTML 5", "html5.png");

                context.Add(category1);
                context.Add(category2);
                context.Add(category3);
                context.Add(category4);
                context.Add(category5);
                context.SaveChanges();

                var tag1 = Tag.Create("finace");
                var tag2 = Tag.Create("bitcoin");

                context.Add(tag1);
                context.Add(tag2);
                context.SaveChanges();


                var post = Post.Create(user.Id, "AI Age", "Will AI replace software engineer");
                post.ChangeImage("Screenshot 2024-04-26 170004.png");
                post.SetCategory(category1);
                context.Add(post);
                context.SaveChanges();

                post.AddTags(tag1);
                post.AddTags(tag2);

                context.Update(post);
                context.SaveChanges();

                string jsonString = "[{\"type\":\"paragraph\",\"children\":[{\"text\":\"\"}]}]";

                var comment = Comment.Create(user.Id, post.Id, jsonString);

                post.AddComment(comment);
                context.Update(post);
                context.SaveChanges();

                // Adding notification 
                var notification = Notification.Create(user.Id, "Tim commented on your post", "www.google.com");
                context.Add(notification);
                context.SaveChanges();
            }

            return app;
        }
    }
}
