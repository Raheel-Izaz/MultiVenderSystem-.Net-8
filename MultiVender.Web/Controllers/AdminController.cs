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

        // GET: Admin/Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return View(categories);
        }

        // GET: Admin/CreateCategory
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/CreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        // GET: Admin/EditCategory/5
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/EditCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Update(category);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        // GET: Admin/DeleteCategory/5
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/DeleteCategory/5
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            var category = await _unitOfWork.Categories.GetAsync(id);
            if (category != null)
            {
                _unitOfWork.Categories.Remove(category);
                await _unitOfWork.SaveAsync();
            }
            return RedirectToAction(nameof(Categories));
        }
    }
}
