using Domain.AggegratesModel.ActivityAggegrate;

namespace Application.Activites
{
    public interface IActivityRepository
    {
        Activity Add(Activity activity);

        IEnumerable<Activity> GetActivitesByUserId(int userId);
    }
}
