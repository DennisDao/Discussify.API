using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggegratesModel.PostAggegrate
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetLatestPost();

        void AddPost(Post post);
    }
}
