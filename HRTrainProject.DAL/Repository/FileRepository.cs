using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRTrainProject.DAL.Repository
{
    public class FileRepository
    {
        public async Task UploadPhoto(List<IFormFile> files, string directoryPath, string prefixName = "")
        {
            var size = files.Sum(f => f.Length);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var path = $@"{directoryPath}\{prefixName}{file.FileName}";

                    if (!System.IO.File.Exists(path))
                    {
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    
                }
            }
        }
    }
}
