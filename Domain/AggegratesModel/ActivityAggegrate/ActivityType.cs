using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggegratesModel.ActivityAggegrate
{
    public enum ActivityType
    {
        [Description("Post Created")]
        PostCreated = 1,

        [Description("Comment Created")]
        CommentCreated = 2,

        [Description("User Profile Updated")]
        UserProfileUpdated = 3,

        [Description("Avatar Updated")]
        AvatarUpdated = 4,
    }
}
