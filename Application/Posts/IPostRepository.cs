﻿using Domain.AggegratesModel.PostAggegrate;

namespace Application.Posts
{
    public interface IPostRepository
    {
        /// <summary> Returns the latest posts </summary>
        IEnumerable<Post> GetPost(int pageSize, int pageNumber);

        /// <summary> Returns all post that match the search query </summary>
        IEnumerable<Post> GetPostByQuery(string query);

        /// <summary> Returns all post for a user </summary>
        IEnumerable<Post> GetPostByUserId(int userId);

        /// <summary> Returns the total of post</summary>
        int GetTotalPost();

        /// <summary> Returns all avaliable category </summary>
        IEnumerable<Category> GetAllCategories();

        Post GetPostById (int postId);

        /// <summary>Add a tag to a post  </summary>
        void AddTag(Post post, Tag tag);

        /// <summary> Add a new post</summary>
        void AddPost(Post post);

        void SaveChanges();
    }
}
