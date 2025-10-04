using System.ComponentModel.DataAnnotations;

namespace SamsarPanel.Client.ViewModels
{
    public class ContactModel
    {
        [Display(Name = "نام کامل")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "شماره تلفن الزامی است")]
        [RegularExpression(@"^(09(1[0-9]|2[0-9]|3[0-9]|9[0-9]))\d{7}$", ErrorMessage = "شماره معتبر وارد کنید (مثلاً 0912xxxxxxx)")]
        [Display(Name = "شماره تلفن")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "موضوع الزامی است")]
        [StringLength(90, MinimumLength = 5, ErrorMessage = "موضوع باید بین ۳ تا ۱۸ کلمه باشد")]
        [Display(Name = "موضوع")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "متن پیام الزامی است")]
        [StringLength(512, MinimumLength = 32, ErrorMessage = "متن باید بین ۳۲ تا ۵۱۲ کاراکتر باشد")]
        [Display(Name = "متن پیام")]
        public string Message { get; set; }
    }
}
