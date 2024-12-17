using Application.Dtos.cpanel;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class UploadFile
    {
        public async Task<ResultUploadDto> Upload(IBrowserFile file,string path, List<string> validTypes,int size=3000)
        {
            if (file == null || file.Size == 0)
            {
                               return new ResultUploadDto { IsSuccess=false,Message= "فایلی انتخاب نمایید" };
            }
            var ext = Path.GetExtension(file.Name).ToLower();
            
            if (validTypes.Where(a => a == ext).Any() == false)
            {
                
                return new ResultUploadDto { IsSuccess = false, Message = "نوع فایل انتخابی صحیح نمی باشد" };
            }
            if (file.Size / 1024 > size)
            {
                
                return new ResultUploadDto { IsSuccess = false, Message = "اندازه فایل بزرگ است" };

            }
            var filename = Path.GetTempFileName() + $"{new Random().Next(1000)}{ext}";
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Cpanel", "assets", "images", "users");

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            using (var stream = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                await file.OpenReadStream(maxAllowedSize: 10_000_000).CopyToAsync(stream);
            }
            return new ResultUploadDto { IsSuccess = true, Filename = filename};

        }
    }
}
