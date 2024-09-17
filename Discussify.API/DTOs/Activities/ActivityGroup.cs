using Domain.AggegratesModel.ActivityAggegrate;

namespace Api.DTOs.Activities
{
    public class ActivityGroupResponse
    {
        public string WhenCreated { get; set; }
        public List<ActivityResponse> Activities { get; set; }
    }

    public class ActivityResponse
    {
        public int ActivityId { get; set;}

        public string Image { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
