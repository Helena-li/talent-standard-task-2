using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Common.Contracts
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file, FileType type);
        Task<bool> DeleteFile(string id, FileType type);
        Task<string> GetFileURL(string id, FileType type);
    }

    public enum FileType
    {
        ProfilePhoto = 1,
        UserVideo = 2,
        UserCV = 3
    }
}
