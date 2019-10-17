using Microsoft.AspNetCore.Identity;

namespace StudentManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
