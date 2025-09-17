using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Resourses
{
    public static class ConstantCpanel
    {
        public const string image_user_Path = "Cpanel/assets/images/users";
        public const string image_slider_Path = "Cpanel/assets/images/sliders";
        public const string image_product_Path = "Cpanel/assets/images/products";

        public static List<string> image_Valid_Types = new List<string> { ".jpg", ".png",".webp",".jpeg",".gif" };
        public static string pysicaly_image_user_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image_user_Path);
        public static string pysicaly_image_slider_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image_slider_Path);
        public static string pysicaly_image_product_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image_product_Path);

        public  const string image_user_no_avatar = "/Cpanel/assets/media/image/user.png";
        
        public const string connectionString= "Server=(localdb)\\mssqllocaldb;Database=aspnet-SamsarPanel-3353ccfb-a532-47d2-b5f2-9c5d23693f62;Trusted_Connection=True;MultipleActiveResultSets=true";
    }
}
