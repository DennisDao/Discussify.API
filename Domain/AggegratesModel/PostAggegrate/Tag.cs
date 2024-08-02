using Domain.SeedWork;
using System.Linq.Expressions;

namespace Domain.AggegratesModel.PostAggegrate
{
    public class Tag : Entity 
    {
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }
        private Tag()
        {
                
        }

        public static Tag Create(string name)
        {
            return new Tag() { Name = name };
        }
    }
}
