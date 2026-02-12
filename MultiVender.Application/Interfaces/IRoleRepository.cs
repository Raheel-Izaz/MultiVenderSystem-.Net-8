using MultiVender.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Application.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        void Update(Role role);
    }
}
