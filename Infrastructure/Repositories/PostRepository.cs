using Application.Posts;
using Domain.AggegratesModel.PostAggegrate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private const int TAKE_LIMIT = 10;
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

        public IEnumerable<Post> GetLatestPost()
        {
            var post = _context.Posts
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .Take(TAKE_LIMIT);

            return post;
        }

        public Post GetPostById(int postId)
        {
            return _context.Posts
                    .Include(x => x.Comments)
                    .FirstOrDefault(x => x.Id == postId);
        }

        public IEnumerable<Post> GetPostByQuery(string query)
        {
            return _context.Posts.Where(x => x.Title.StartsWith(query) || x.Title.Contains(query));
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
