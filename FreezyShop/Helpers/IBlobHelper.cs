using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FreezyShop.Helpers
{
    public interface IBlobHelper
    {
        Task<Guid> UploadBlobAsync(IFormFile file, string containerName);
        //Task<Guid> UploadBlobAsync2(IFormFile file, string containerName);
    }
}
