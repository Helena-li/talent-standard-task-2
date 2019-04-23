using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Talent.Common.Aws;
using Talent.Common.Contracts;

namespace Talent.Common.Services
{
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment _environment;
        private readonly string _tempFolder;
        private IAwsService _awsService;

        public FileService(IHostingEnvironment environment, 
            IAwsService awsService)
        {
            _environment = environment;
            _tempFolder = "images\\";
            _awsService = awsService;
        }

        public async Task<string> GetFileURL(string id, FileType type)
        {
            var url = "http://localhost:60290/images/" + id;
            return url;
        }

        public async Task<string> SaveFile(IFormFile file, FileType type)
        {
            if (file==null||file.Length==0)
            {
                return null;
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), _tempFolder, file.FileName);
            using (var stream= new FileStream(path,FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return file.FileName;
        }

        public async Task<bool> DeleteFile(string id, FileType type)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), _tempFolder, id);
            if (File.Exists(path))
            {
                await Task.Run(() => {
                    File.Delete(path);
                    return true;
                });
            }
            return false;
        }


        #region Document Save Methods

        private async Task<string> SaveFileGeneral(IFormFile file, string bucket, string folder, bool isPublic)
        {
            //Your code here;
            throw new NotImplementedException();
        }
        
        private async Task<bool> DeleteFileGeneral(string id, string bucket)
        {
            //Your code here;
            throw new NotImplementedException();
        }
        #endregion
    }
}
