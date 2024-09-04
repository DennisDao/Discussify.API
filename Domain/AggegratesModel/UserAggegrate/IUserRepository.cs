using Domain.AggegratesModel.PostAggegrate;

namespace Domain.AggegratesModel.UserAggegrate
{
    public interface IUserRepository
    {
        /// <summary> Returns a list of users</summary>
        IEnumerable<IUser> GetUsers(int pageSize, int pageNumber);

        /// <summary> Returns a list of users that batch the search query</summary>
        IEnumerable<IUser> FindUser(string query);

        Task<IUser> GetUserByIdAsync(int id);

        Task<IUser> GetUserByEmailAsync(string email);
    }
}
