using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Domain.Entities
{
    public class Vendor : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public Shop Shop { get; set; }
    }
}
