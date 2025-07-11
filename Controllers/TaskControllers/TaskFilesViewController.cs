using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs.FileUploadDTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers.TaskControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskFilesViewController : ControllerBase
    {
        public readonly DataContext _context;
        public TaskFilesViewController(DataContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        [Route("TaskInfo")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetFiles(int PId, int TId)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == PId && e.FileType == "basicInfo" && e.TaskId == TId)
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
        [Route("Images")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetImages(int PId, int TId)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == PId && e.FileType == "image" && e.TaskId == TId)
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
        [Route("audio")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetAudioFiles(int PId, int TId)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == PId && e.FileType == "audio" && e.TaskId == TId)
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
        [Route("video")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetVideoFiles(int PId, int TId)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == PId && e.FileType == "video" && e.TaskId == TId)
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
        [Route("zip")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetZipFiles(int PId, int TId)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == PId && e.FileType == "zip" && e.TaskId == TId)
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
