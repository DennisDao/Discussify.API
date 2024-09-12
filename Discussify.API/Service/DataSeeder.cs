using Domain.AggegratesModel.FollowerAggegrate;
using Domain.AggegratesModel.NotificationAggegrate;
using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

namespace Discussify.API.Service
{
    public static class DataSeeder
    {
        private static readonly string[] Subjects =
        {
            "How to", "Top 10", "Why", "The Ultimate Guide to", "The Secret of",
            "Discover", "The Importance of", "Understanding", "A Beginner's Guide to",
            "Exploring", "Mastering", "Common Mistakes in", "The Future of"
        };

        private static readonly string[] Topics =
        {
            "Web Development", "Design Patterns", "Cloud Computing", "Data Science",
            "AI and Machine Learning", "Blockchain", "Productivity", "React", "C# Programming",
            "Startup Growth", "Cybersecurity", "Remote Work", "SEO", "Mobile Development"
        };

        private static readonly string[] Endings =
        {
            "in 2024", "for Beginners", "You Should Know", "Everyone Misses", "Step by Step",
            "Like a Pro", "That Will Boost Your Skills", "for Developers", "Made Easy",
            "in Just 10 Minutes", "for Advanced Users", "in a Nutshell", "You Can Master"
        };

        private static readonly string[] AllTags =
        {
            "Technology", "Programming", "AI", "Data Science", "Web Development",
            "Cloud Computing", "Machine Learning", "C#", "JavaScript", "React",
            "Cybersecurity", "Blockchain", "Mobile Development", "Software Engineering",
            "UI/UX", "DevOps", "Productivity", "SEO", "Digital Marketing", "Startups"
        };

        private static readonly HttpClient httpClient = new HttpClient();
        public static async Task<WebApplication> Seed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string currentDirectory = Path.GetDirectoryName(assemblyLocation);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var test = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                var users = await SeedUsers(userManager, test.WebRootPath);
                var categories = SeedCategory(context);

                Random random = new Random();

                for (int i = 0; i < 40; i++)
                {
                    var post = Post.Create(users[random.Next(users.Count)].Id, GenerateRandomPostTitle(), GenerateRandomPostTitle());
                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                    await DownloadImageAsync("https://picsum.photos/600/600", test.WebRootPath, "Post", fileName);
                    post.ChangeImage(fileName);
                    post.SetCategory(categories[random.Next(categories.Count)]);

                    var tag1 = AllTags[random.Next(AllTags.Count())];
                    var tag2 = AllTags[random.Next(AllTags.Count())];

                    post.AddTags(Tag.Create(tag1));
                    post.AddTags(Tag.Create(tag2));

                    context.Add(post);
                    context.SaveChanges();
                }


                //var post = Post.Create(user.Id, "AI Age", "Will AI replace software engineer");
                //post.ChangeImage("Screenshot 2024-04-26 170004.png");
                //post.SetCategory(category1);
                //context.Add(post);
                //context.SaveChanges();

                //post.AddTags(tag1);
                //post.AddTags(tag2);

                //context.Update(post);
                //context.SaveChanges();

                //string jsonString = "[{\"type\":\"paragraph\",\"children\":[{\"text\":\"\"}]}]";

                //var comment = Comment.Create(user.Id, post.Id, jsonString);

                //post.AddComment(comment);
                //context.Update(post);
                //context.SaveChanges();

                //try
                //{
                //    var follower = Follower.Follow(user.Id, user2.Id);
                //    context.Followers.Add(follower);
                //    context.SaveChanges();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
            }

            return app;
        }

        private static void SeedTags(ApplicationDbContext context)
        {
            var tag1 = Tag.Create("finace");
            var tag2 = Tag.Create("bitcoin");

            context.Add(tag1);
            context.Add(tag2);
            context.SaveChanges();
        }

        public static string GenerateRandomPostTitle()
        {
            Random random = new Random();
            string subject = Subjects[random.Next(Subjects.Length)];
            string topic = Topics[random.Next(Topics.Length)];
            string ending = Endings[random.Next(Endings.Length)];

            return $"{subject} {topic} {ending}";
        }

        private static List<Category> SeedCategory(ApplicationDbContext context)
        {
            var categories = new List<Category>();

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

            categories.Add(category1);
            categories.Add(category2);
            categories.Add(category3);
            categories.Add(category4);
            categories.Add(category5);

            return categories;
        }

        private static async Task<List<ApplicationUser>> SeedUsers(UserManager<ApplicationUser> userManager, string folderPath)
        {
            List<ApplicationUser> users = new();

            var user1 = ApplicationUser.Create("Dennis", "Dao", "test@outlook.com");
            user1.ChangeAvatar("dennis.png");

            var user2 = ApplicationUser.Create("Jenny", "Nguyen", "brian@outlook.com");
            user2.ChangeAvatar("Jenny.jpg");

            await userManager.CreateAsync(user1, "Password123$");
            await userManager.CreateAsync(user2, "Password123$");

            users.Add(user1);
            users.Add(user2);

            var fakeUsers = await GetUserDataAsync(@"https://randomuser.me/api/?results=50&inc=name,gender,email,nat,picture&noinfo");

            foreach (var user in fakeUsers)
            {
                var fakeUser = ApplicationUser.Create(user.Name.First, user.Name.Last, user.Email);
                var avatarFileName = Guid.NewGuid().ToString() + ".jpg";
                await DownloadImageAsync(user.Picture.Large, folderPath, "Avatars", avatarFileName);

                fakeUser.ChangeAvatar(avatarFileName);
                await userManager.CreateAsync(fakeUser, "Password123$");
                users.Add(fakeUser);
            }

            return users;
        }

        public static async Task<List<UserProfile>> GetUserDataAsync(string apiUrl)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var resultData = JsonConvert.DeserializeObject<Dictionary<string, List<UserProfile>>>(jsonResponse);
                    return resultData["results"];
                }
                else
                {
                    Console.WriteLine("Error fetching data from API: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public static async Task DownloadImageAsync(string imageUrl, string folder, string subfolder, string fileName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Download the image as a byte array
                    var imageBytes = await client.GetByteArrayAsync(imageUrl);

                    // Create the directory path if it doesn't exist
                    string directoryPath = Path.Combine(folder, subfolder);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Define the file path in the wwwroot folder
                    string filePath = Path.Combine(directoryPath, fileName);

                    // Save the image to the directory
                    await File.WriteAllBytesAsync(filePath, imageBytes);

                    Console.WriteLine("Image downloaded and saved successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    public class Name
    {
        public string Title { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }

    public class Picture
    {
        public string Large { get; set; }
        public string Medium { get; set; }
        public string Thumbnail { get; set; }
    }

    public class UserProfile
    {
        public string Gender { get; set; }
        public Name Name { get; set; }
        public string Email { get; set; }
        public Picture Picture { get; set; }
        public string Nat { get; set; }
    }

    public class FakeUsersApiResponse
    {
        public List<UserProfile> UserProfiles { get; set; }
    }
}
