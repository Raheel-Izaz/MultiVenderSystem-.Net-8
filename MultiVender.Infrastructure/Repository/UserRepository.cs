using MultiVender.Application.Interfaces;
using MultiVender.Domain.Entities;
using MultiVender.Infrastructure.Data;

namespace MultiVender.Infrastructure.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
        // Add user-specific methods here if needed
    }
}
