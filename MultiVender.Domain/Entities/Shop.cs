using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Domain.Entities
{
    public class Shop : BaseEntity
    {
        public string ShopName { get; set; }
        public string Description { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
