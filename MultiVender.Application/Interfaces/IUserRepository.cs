using MultiVender.Domain.Entities;

namespace MultiVender.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        // Add user-specific methods here if needed
    }
}
