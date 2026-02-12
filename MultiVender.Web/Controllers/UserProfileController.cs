using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiVender.Application.Interfaces;
using MultiVender.Domain.Entities;
using System.Security.Claims;

namespace MultiVender.Web.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public IActionResult Profile()
        {
            return View();
        }


        // GET: Home/ApplyVendor
        public IActionResult ApplyVendor()
        {
            return View();
        }

        // POST: Home/ApplyVendor
        [HttpPost]
        public async Task<IActionResult> ApplyVendorPost()
        {
            int userId = GetUserId();

            var existingVendors = await _unitOfWork.Vendors
                .GetAllAsync(v => v.UserId == userId);

            if (existingVendors.Any())
            {
                return BadRequest("You already applied.");
            }

            var vendor = new Vendor
            {
                UserId = userId,
                Status = Domain.Entities.VendorStatus.Pending
            };

            await _unitOfWork.Vendors.AddAsync(vendor);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("VendorStatus");
        }

        public async Task<IActionResult> VendorStatus()
        {
            int userId = GetUserId();

            var vendor = (await _unitOfWork.Vendors
                .GetAllAsync(v => v.UserId == userId))
                .FirstOrDefault();

            if (vendor == null)
                return NotFound();

            return View(vendor);
        }
    }
}
