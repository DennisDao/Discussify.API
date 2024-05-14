using Domain.AggegratesModel.PostAggegrate;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IEnumerable<Post> GetLatestPost()
        {
            var post = _context.Posts
                .Include(x => x.Topics)
                .ThenInclude(x => x.TopicType)
                .Take(TAKE_LIMIT);

            return post;
        }
    }
}
