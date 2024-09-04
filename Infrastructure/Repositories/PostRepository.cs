using Application.Posts;
using Domain.AggegratesModel.PostAggegrate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddPost(Post post)
        {
           _context.Posts.Add(post);
        }

        public void AddTag(Post post, Tag tag)
        {
            post.AddTags(tag);
            SaveChanges();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories;
        }

        public IEnumerable<Post> GetPost(int pageSize, int pageNumber)
        {
            var posts = _context.Posts
                  .Include(x => x.Tags)
                  .Include(x => x.Comments)
                  .OrderByDescending(x => x.WhenCreated) 
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .ToList();

            return posts;
        }

        public Post GetPostById(int postId)
        {
            return _context.Posts
                    .Include(x => x.Comments)
                    .FirstOrDefault(x => x.Id == postId);
        }

        public IEnumerable<Post> GetPostByQuery(string query)
        {
            return _context.Posts
                .Include(x => x.Tags)
                .Where(x => x.Title.StartsWith(query) || x.Title.Contains(query) || x.Tags.Any(x => x.Name == query));
        }

        public IEnumerable<Post> GetPostByUserId(int userId)
        {
            return _context.Posts.Where(x => x.UserId == userId);
        }

        public int GetTotalPost()
        {
            return _context.Posts.Count();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
