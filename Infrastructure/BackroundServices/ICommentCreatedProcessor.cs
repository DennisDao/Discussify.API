using Infrastructure.Entities;

namespace Infrastructure.BackroundServices
{
    public interface ICommentCreatedProcessor
    {
        void Process(OutboxMessage message);
    }
}
