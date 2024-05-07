namespace Domain.AggegratesModel.UserAggegrate
{
    public interface IUserRepository
    {
        Task<IUser> GetUserByIdAsync(string id);

        Task<IUser> GetUserByEmailAsync(string email);
    }
}
