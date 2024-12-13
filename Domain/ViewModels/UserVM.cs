using Domain.Resourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class UserVM
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public string? FullName { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsSuspend { get; set; } = false;
        public string LastDateLogin { get; set; } = String.Empty;
        public string? avatar { get; set; } = null;
        public int LoginCount { get; set; } = 0;

        public string GetAvatar()
        {
            if (string.IsNullOrEmpty(avatar)) 
                return ConstantCpanel.image_user_no_avatar;
            return ConstantCpanel.image_user_Path+"/"+avatar;
        }

    }
}
