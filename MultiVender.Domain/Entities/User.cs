using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Domain.Entities
{
    public class User 
    {
        public int Id { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool IsVendor { get; set; } = false;
        public int RoleId { get; set; }     // Foreign key
        public Role? Role { get; set; }     // Navigation property

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
