using Microsoft.AspNetCore.Identity;

namespace Domain.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? Firstname { get; set; } = null;
        public string? Lastname { get; set; } = null;


    }

}
