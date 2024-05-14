using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggegratesModel.PostAggegrate
{
    public class Topic : Entity
    {
        public TopicType TopicType { get; private set; }

        public int TopicTypeId { get; private set; }

        public Topic()
        {
             // Entity framework
        }

        public string GetDescription()
        {
            return this.TopicType.Description;
        }
    }
}
