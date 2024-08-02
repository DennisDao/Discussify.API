using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.UserAggegrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts
{
    public sealed class PostService
    {
        private readonly IPostRepository _postRepository;
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public IEnumerable<Post> GetLatestPost()
        {
            return _postRepository.GetLatestPost().ToList();
        }

        public void CreatePost(int userId, string title, string description)
        {
            var category = _postRepository.GetAllCategories().FirstOrDefault();
            var post = Post.Create(userId, title, description);
            post.ChangeImage("test");
            post.SetCategory(category);

            _postRepository.AddPost(post);
            _postRepository.SaveChanges();
        }

        public Post GetPost(int postId)
        {
            return _postRepository.GetPostById(postId);
        }

        public void Save()
        {
            _postRepository.SaveChanges();
        }
    }
}
