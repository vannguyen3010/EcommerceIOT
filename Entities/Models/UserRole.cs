using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class UserRole : IdentityRole
    {
        public DateTime DateCreated { get; set; }
    }
}
