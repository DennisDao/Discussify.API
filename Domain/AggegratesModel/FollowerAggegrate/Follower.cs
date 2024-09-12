using Domain.SeedWork;

namespace Domain.AggegratesModel.FollowerAggegrate
{
    public sealed class Follower : Entity
    {
        public int FollowerUserId { get; private set; }
        public int FollowingUserId { get; private set; }
        public DateTime WhenCreated { get; private set; }

        private Follower()
        {

        }

        public static Follower Follow(int userId, int followingUserId)
        {
            return new Follower()
            {
                FollowerUserId = userId,
                FollowingUserId = followingUserId,
                WhenCreated = DateTime.UtcNow
            };
        }
    }
}
