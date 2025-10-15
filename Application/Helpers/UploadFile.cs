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
        private async Task<ResultUploadDto> CheckValidFiles(List<IBrowserFile> files, List<string> validTypes, int size = 3000)
        {
            foreach (var file in files)
            {
                if (file == null || file.Size == 0)
                {
                    return new ResultUploadDto { IsSuccess = false, Message = "فایلی انتخاب نمایید" };
                }
                  var ext = Path.GetExtension(file.Name).ToLower();

                if (validTypes.Where(a => a == ext).Any() == false)
                {

                    return new ResultUploadDto { IsSuccess = false, Message = $"نوع فایل  {file.Name} انتخابی صحیح نمی باشد" };
                }
                if (file.Size / 1024 > size)
                {

                    return new ResultUploadDto { IsSuccess = false, Message = $"سایز فایل    {file.Name} انتخابی صحیح نمی باشد" };

                }
            }

            return new ResultUploadDto { IsSuccess = true };
        }
        public async Task<ResultUploadDto> Uploads(List<IBrowserFile> files, string path, List<string> validTypes, int size = 3000)
        {
            var filenames = "";
            var res= await CheckValidFiles(files, validTypes, size);
            if (res.IsSuccess==false)
            {
                return res;
            }
            foreach (var file in files)
            {
                var filename = Path.GetFileName(Path.GetTempFileName() + $"{new Random().Next(1000)}{Path.GetExtension(file.Name).ToLower()}");
                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Cpanel", "assets", "images", "users");

                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                using (var stream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                {
                    await file.OpenReadStream(maxAllowedSize: 10_000_000).CopyToAsync(stream);
                }
                filenames += filenames == "" ? filename : "," + filename;   // 1.jpg,2.png,23.jpg
            }

            return new ResultUploadDto { IsSuccess = true, Filename = filenames };

        }

        public void DeleteFile(string fisicallyPath,string filename)
        {
            File.Delete(fisicallyPath + "/" + filename);
            

        }


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
            var filename =Path.GetFileName( Path.GetTempFileName() + $"{new Random().Next(1000)}{ext}");
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
