using MultiVender.Application.Interfaces;
using MultiVender.Application.Services;
using MultiVender.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Infrastructure.Repository.Services
{
    public class VendorService : IVendorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VendorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ApproveVendorAsync(int vendorId)
        {
            // 1. Get vendor by Id
            var vendor = await _unitOfWork.Vendors.GetAsync(vendorId);

            if (vendor == null)
                throw new Exception("Vendor not found");

            // 2. Prevent double approval
            if (vendor.Status == VendorStatus.Approved)
                throw new Exception("Vendor already approved");

            // 3. Approve vendor
            vendor.Status = VendorStatus.Approved;

            // 4. Create shop
            var shop = new Shop
            {
                VendorId = vendor.Id,
                ShopName = "Vendor Shop",
                Description = "New Vendor Shop",
                isActive = true
            };

            await _unitOfWork.Shops.AddAsync(shop);

            // 5. Update user vendor flag
            var user = await _unitOfWork.Users.GetAsync(vendor.UserId);
            user.IsVendor = true;

            // 6. Save changes
            await _unitOfWork.SaveAsync();
        }
    }
}
