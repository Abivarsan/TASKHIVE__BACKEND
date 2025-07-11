using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs.FileUploadDTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers.FileUploadControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectFileViewController : ControllerBase
    {
        public readonly DataContext _context;
        public ProjectFileViewController(DataContext _context) 
        {
            this._context = _context;
        }

        [HttpGet]
        [Route("Basic")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetFiles(int id)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId ==  id && e.FileType == "basic")
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
        [Route("Timeline")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetTimelineFiles(int id)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == id && e.FileType == "timeline")
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
        [Route("BudgetInfo")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetBudgetFiles(int id)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == id && e.FileType == "budgetinfo")
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
        [Route("ClientDoc")]
        public async Task<ActionResult<IEnumerable<FileResource>>> GetClientFiles(int id)
        {
            var files = await _context.FileResources
                .Where(e => e.ProjectId == id && e.FileType == "clientdoc")
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
