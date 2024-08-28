using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggegratesModel.PostAggegrate
{
    public class Category : Entity
    {
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime WhenCreated { get; private set; }

        public DateTime WhenUpdated { get; private set; }

        private Category()
        {
                
        }

        public static Category Create(string categoryName, string description, string image)
        {
            return new Category() 
            { 
                CategoryName = categoryName,
                Image = image, 
                Description = description,
                WhenCreated = DateTime.UtcNow,
                WhenUpdated = DateTime.UtcNow,
            };  
        }
    }
}
