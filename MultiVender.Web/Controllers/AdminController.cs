using Microsoft.AspNetCore.Mvc;
using MultiVender.Application.Interfaces;
using MultiVender.Application.Services;
using MultiVender.Domain.Entities;

namespace MultiVender.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVendorService _vendorService;
        public AdminController(IUnitOfWork unitOfWork , IVendorService vendorService)
        {
            _unitOfWork = unitOfWork;
            _vendorService = vendorService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/PendingVendors
        public async Task<IActionResult> PendingVendors()
        {
            var vendors = await _unitOfWork.Vendors.GetAllAsync();
            var pendingVendors = vendors
                .Where(v => v.Status == VendorStatus.Pending)
                .ToList();

            return View(pendingVendors);
        }

        // POST: Admin/ApproveVendor/5
        [HttpPost]
        public async Task<IActionResult> ApproveVendor(int id)
        {
            await _vendorService.ApproveVendorAsync(id);
            return RedirectToAction(nameof(PendingVendors));
        }

        // GET: Admin/ApprovedVendors
        public async Task<IActionResult> ApprovedVendors()
        {
            var vendors = await _unitOfWork.Vendors.GetAllAsync();
            var approvedVendors = vendors
                .Where(v => v.Status == VendorStatus.Approved)
                .ToList();

            return View(approvedVendors);
        }
    }
}
