using Domain.AggegratesModel.FollowerAggegrate;
using Domain.AggegratesModel.NotificationAggegrate;

namespace Application.Followers
{
    public interface IFollowerRepository
    {
        /// <summary> Get all followers for a user</summary>
        IEnumerable<Follower> GetFollowers(int userId);

        /// <summary> Get all following for a user</summary>
        IEnumerable<Follower> GetFollowing(int userId);

        /// <summary> Find a follow record by Id</summary>
        Follower GetFollowerByid(int followId);

        /// <summary>Add a following for the user</summary>
        Follower AddFollower(int userId, int targetUserId);

        /// <summary> Delete a follower record</summary>
        void Delete(Follower follower);
    }
}
