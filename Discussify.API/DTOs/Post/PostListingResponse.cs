﻿namespace Api.DTOs.Post
{
    public class PostListingResponse
    {
        public int PostId { get; set; }

        public string ImageUrl { get; set; }

        public string AuthorImageUrl { get; set; }

        public int AuthorId { get; set; }

        public List<string> Tags { get; set; }

        public string Title { get; set; }

        public string AuthorName { get; set; }

        public string Description { get; set; }

        public string AuthorLastName { get; set; }

        public int TotalViews { get; set; }

        public int TotalComments { get; set; }

        public int TotalLikes { get; set; }

        public string WhenCreated { get; set; }
    }
}
