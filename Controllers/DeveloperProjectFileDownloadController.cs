using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using TASKHIVE.Data;
using TASKHIVE.Models;


namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    public class DeveloperProjectFileDownloadController : ControllerBase
    {


        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeveloperProjectFileDownloadController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        [HttpGet]
        [Route("DeveloperProjectFileDownload/{filepath}/{filename}")]
        public async Task<IActionResult> DownloadDeveloperFile(string filepath, string filename)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "D:\\Fileresources\\ProjectFiles", filepath);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-strease";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, contentType, filename);
        }



    }
}

