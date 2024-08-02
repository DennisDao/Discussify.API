using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggegratesModel.PostAggegrate
{
    public class Category : Entity
    {
        public string CategoryName { get; set; }

        public DateTime WhenCreated { get; private set; }

        public DateTime WhenUpdated { get; private set; }

        private Category()
        {
                
        }

        public static Category Create(string categoryName)
        {
            return new Category() 
            { 
                CategoryName = categoryName,
                WhenCreated = DateTime.UtcNow,
                WhenUpdated = DateTime.UtcNow,
            };  
        }
    }
}
