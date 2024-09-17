using Application.Activites;
using Domain.AggegratesModel.ActivityAggegrate;

namespace Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _context;
        public ActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Activity Add(Activity activity)
        {
            _context.Add(activity);
            _context.SaveChanges();
            return activity;
        }

        public IEnumerable<Activity> GetActivitesByUserId(int userId)
        {
            return _context.Activites.Where(x => x.UserId == userId);
        }
    }
}
