using MultiVender.Application.Interfaces;
using MultiVender.Domain.Entities;
using MultiVender.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Infrastructure.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
