using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.UserAggegrate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void CreatePost(int userId, string title, string description, int categoryId, string[] tags)
        {
            var category = _postRepository
                .GetAllCategories()
                .FirstOrDefault(x => x.Id == categoryId);

            var post = Post.Create(userId, title, description);
            post.ChangeImage("test");
            post.SetCategory(category);

            _postRepository.AddPost(post);
            _postRepository.SaveChanges();

            foreach (var tag in tags)
            {
                var newTag = Tag.Create(tag);
                _postRepository.AddTag(post, newTag);
            }

           
        }

        public Post GetPost(int postId)
        {
            return _postRepository.GetPostById(postId);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _postRepository.GetAllCategories().ToList();
        }

        public void Save()
        {
            _postRepository.SaveChanges();
        }
    }
}
