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
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsSuspend { get; set; } = false;
        public string LastDateLogin { get; set; }
        public string? avatar { get; set; }
        public int LoginCount { get; set; } = 0;
    }
}
