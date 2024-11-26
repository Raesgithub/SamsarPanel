using Domain.Resourses;
using Microsoft.AspNetCore.Identity;

namespace Domain.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? Firstname { get; set; } = null;
        public string? Lastname { get; set; } = null;
        public string? Avater { get; set; } = null;
        public string? LastDateLogin { get; set; } = null;
        public bool IsSuspend { get; set; } = false;
        public int LoginCount { get; set; } = 0;

        public string UserImage()
        {
            if (string.IsNullOrEmpty(Avater))
            {
                return ConstantCpanel.image_user_no_avatar;
            }
            return $"{ConstantCpanel.image_user_Path}/{Avater}";
        }
    }

}
