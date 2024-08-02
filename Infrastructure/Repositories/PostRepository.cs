using Application.Posts;
using Domain.AggegratesModel.PostAggegrate;

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

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories;
        }

        public IEnumerable<Post> GetLatestPost()
        {
            var post = _context.Posts
                //.Include(x => x.Topics)
                //.ThenInclude(x => x.TopicType)
                .Take(TAKE_LIMIT);

            return post;
        }

        public Post GetPostById(int postId)
        {
            return _context.Posts.FirstOrDefault(x => x.Id == postId);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
