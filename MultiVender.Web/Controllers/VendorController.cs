using Microsoft.AspNetCore.Mvc;
using MultiVender.Application.Interfaces;
using MultiVender.Domain.Entities;
using System.Threading.Tasks;

namespace MultiVender.Web.Controllers
{
    public class VendorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VendorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private int GetVendorId()
        {
            return 3;
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

        // Vendor Shop Management

        public async Task<IActionResult> Shop()
        {
            int vendorId = GetVendorId();

            var shop = (await _unitOfWork.Shops
                .GetAllAsync(s => s.VendorId == vendorId))
                .FirstOrDefault();

            return View(shop);
        }

        // Create shop
        public IActionResult CreateShop()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShop(Shop shop)
        {
            if (!ModelState.IsValid)
                return View(shop);

            shop.VendorId = GetVendorId();

            await _unitOfWork.Shops.AddAsync(shop);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // Edit shop
        public async Task<IActionResult> EditShop(int id)
        {
            int vendorId = GetVendorId();
            var shop = await _unitOfWork.Shops.GetAsync(id);

            if (shop == null || shop.VendorId != vendorId)
                return NotFound();

            return View(shop);
        }

        [HttpPost]
        public async Task<IActionResult> EditShop(Shop shop)
        {
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        


        // Product Management

        // List vendor products
        public async Task<IActionResult> Products()
        {
            int vendorId = GetVendorId();

            var shop = (await _unitOfWork.Shops
                .GetAllAsync(s => s.VendorId == vendorId))
                .FirstOrDefault();

            if (shop == null)
                return RedirectToAction("Create", "VendorShop");

            var products = await _unitOfWork.Products
                .GetAllAsync(p => p.ShopId == shop.Id);

            return View(products);
        }

        // Create product
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = await _unitOfWork.Categories.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            int vendorId = GetVendorId();

            var shop = (await _unitOfWork.Shops
                .GetAllAsync(s => s.VendorId == vendorId))
                .FirstOrDefault();

            if (shop == null)
            {
                ModelState.AddModelError("", "Please create your shop first.");
                return View(product);
            }

            product.ShopId = shop.Id;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // Edit product
        public async Task<IActionResult> EditProduct(int id)
        {
            int vendorId = GetVendorId();

            var product = await _unitOfWork.Products.GetAsync(id);
            if (product == null)
                return NotFound();

            var shop = (await _unitOfWork.Shops
                .GetAllAsync(s => s.VendorId == vendorId))
                .FirstOrDefault();

            if (shop == null || product.ShopId != shop.Id)
                return NotFound();

            ViewBag.Categories = await _unitOfWork.Categories.GetAllAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // Delete product
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Products.GetAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _unitOfWork.Products.GetAsync(id);

            if (product == null)
                return NotFound();

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
