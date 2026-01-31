using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Domain.Entities
{
    public enum VendorStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public class Vendor : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public VendorStatus Status { get; set; } = VendorStatus.Pending;
        public Shop Shop { get; set; }
    }
}
