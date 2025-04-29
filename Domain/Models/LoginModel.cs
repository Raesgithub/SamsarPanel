using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class LoginModel
    {
        [Required]
        public string un { get; set; }
        [Required]
        public string pa { get; set; }
    }
}
