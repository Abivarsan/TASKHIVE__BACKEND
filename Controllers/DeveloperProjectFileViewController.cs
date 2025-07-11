using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs.FileUploadDTOs;
using TASKHIVE.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    public class DeveloperProjectFileViewController : ControllerBase
    {


        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeveloperProjectFileViewController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        [HttpGet]
        [Route("TaskInfo/{projectid}")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetFiles(int projectid)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == projectid && e.FileType == "basicInfo")
                .Select(e => new FileNamesViewDTO
                {
                    FileId = e.FileId,
                    FileName = e.FileName,
                    LocalStoragePath = e.LocalStoragePath

                }).ToListAsync();

            if (files == null)
            {
                return NotFound();
            }

            return Ok(files);
        }

        [HttpGet]
        [Route("Images/{projectid}")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetImages(int projectid)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == projectid && e.FileType == "image")
                .Select(e => new FileNamesViewDTO
                {
                    FileId = e.FileId,
                    FileName = e.FileName,
                    LocalStoragePath = e.LocalStoragePath

                }).ToListAsync();

            if (files == null)
            {
                return NotFound();
            }

            return Ok(files);
        }

        [HttpGet]
        [Route("audio/{projectid}")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetAudioFiles(int projectid)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == projectid && e.FileType == "audio")
                .Select(e => new FileNamesViewDTO
                {
                    FileId = e.FileId,
                    FileName = e.FileName,
                    LocalStoragePath = e.LocalStoragePath

                }).ToListAsync();

            if (files == null)
            {
                return NotFound();
            }

            return Ok(files);
        }




        [HttpGet]
        [Route("zip/{projectid}")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetZipFiles(int projectid)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == projectid && e.FileType == "zip")
                .Select(e => new FileNamesViewDTO
                {
                    FileId = e.FileId,
                    FileName = e.FileName,
                    LocalStoragePath = e.LocalStoragePath

                }).ToListAsync();

            if (files == null)
            {
                return NotFound();
            }

            return Ok(files);
        }
    }
}

