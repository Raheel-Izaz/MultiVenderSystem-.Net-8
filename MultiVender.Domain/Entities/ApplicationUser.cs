using Microsoft.AspNetCore.Identity;

namespace MultiVender.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
       public bool IsVendor { get; set; }
    }
}
