using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers.FileUploadControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectFileDownloadController : ControllerBase
    {

        public readonly DataContext _context;

        public ProjectFileDownloadController(DataContext _context)
        {
            this._context = _context;
            
        }

        [HttpGet]
        [Route("DownloadProjectFile")]
        public async Task<IActionResult> DownloadFile(string FilePath, string FileName)
        {
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\Users\\Suraj Madhushan\\Desktop\\sw_projectNew\\UploadedData", FilePath);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-strease";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, contentType, FileName);
        }
    }
}
