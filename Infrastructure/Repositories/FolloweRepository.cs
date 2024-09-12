using Application.Followers;
using Domain.AggegratesModel.FollowerAggegrate;
using Domain.AggegratesModel.UserAggegrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public sealed class FolloweRepository : IFollowerRepository
    {
        private readonly ApplicationDbContext _context;
        public FolloweRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public Follower GetFollowerByid(int followId)
        {
            return _context.Followers.FirstOrDefault(x => x.Id == followId);
        }

        public IEnumerable<Follower> GetFollowers(int userId)
        {
            return _context.Followers.Where(x => x.FollowingUserId == userId).ToList();
        }

        public IEnumerable<Follower> GetFollowing(int userId)
        {
            return _context.Followers.Where(x => x.FollowerUserId == userId).ToList();
        }

        public void Delete(Follower follower)
        {
            _context.Followers.Remove(follower);
            _context.SaveChanges();
        }

        public Follower AddFollower(int userId, int targetUserId)
        {
            var follower = Follower.Follow(userId, targetUserId);
            _context.Followers.Add(follower);
            _context.SaveChanges();

            return follower;
        }
    }
}
