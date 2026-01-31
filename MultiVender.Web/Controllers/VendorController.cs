using Microsoft.AspNetCore.Mvc;
using MultiVender.Application.Interfaces;
using MultiVender.Domain.Entities;

namespace MultiVender.Web.Controllers
{
    public class VendorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VendorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: Vendor/Apply
        public IActionResult ApplyVendor()
        {
            return View();
        }

        // POST: Vendor/Apply
        [HttpPost]
        public async Task<IActionResult> Apply(int userId)
        {
            // check if vendor already exists for user
            var existingVendors = await _unitOfWork.Vendors.GetAllAsync();
            if (existingVendors.Any(v => v.UserId == userId))
            {
                return BadRequest("Vendor already exists for this user.");
            }

            var vendor = new Vendor
            {
                UserId = userId,
                Status = VendorStatus.Pending
            };

            await _unitOfWork.Vendors.AddAsync(vendor);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Status), new { userId });
        }

        // GET: Vendor/Status
        public async Task<IActionResult> Status(int userId)
        {
            var vendors = await _unitOfWork.Vendors.GetAllAsync();
            var vendor = vendors.FirstOrDefault(v => v.UserId == userId);

            if (vendor == null)
                return NotFound();

            return View(vendor);
        }

    }
}
