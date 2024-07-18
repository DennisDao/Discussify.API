namespace Domain.AggegratesModel.UserAggegrate
{
    public interface IUserRepository
    {
        Task<IUser> GetUserByIdAsync(int id);

        Task<IUser> GetUserByEmailAsync(string email);
    }
}
