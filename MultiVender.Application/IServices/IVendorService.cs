using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Application.Services
{
    public interface IVendorService
    {
        Task ApproveVendorAsync(int vendorId);
    }
}
