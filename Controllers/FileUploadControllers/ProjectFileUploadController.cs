using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers.FileUploadControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectFileUploadController : ControllerBase
    {
        public readonly DataContext _context;

        public ProjectFileUploadController(DataContext _context)
        {
            this._context = _context;
        }





        private async Task<string> WriteFile(IFormFile file)
        {
            string fileName = "";

            try
            {

                
                    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    fileName = DateTime.Now.Ticks.ToString() + extension;
                    Console.WriteLine("filename: " + fileName);

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\Users\\Suraj Madhushan\\Desktop\\sw_projectNew\\UploadedData");

                    Console.WriteLine("filepath: " + filePath);

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\Users\\Suraj Madhushan\\Desktop\\sw_projectNew\\UploadedData", fileName);

                    Console.WriteLine("exactpath: " + exactPath);

                    using (var stream = new FileStream(exactPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Not uploaded");
                
            }
            return (fileName);
        }

        [HttpPost]
        [Route("BasicInfo")]

        public async Task<IActionResult> UploadBasicInfo(IFormFile file, int ProID)
        {
            
            var result = WriteFile(file);

           
                var newfile = new FileResource
                {
                    FileName = file.FileName,
                    FileType = "basic",
                    LocalStoragePath = await result,
                    UpdatedDate = DateTime.Now,
                    ProjectId = ProID,
                    TaskId = null
                };

                _context.Add(newfile);
                await _context.SaveChangesAsync();

                return Ok();
            
        }

        [HttpPost]
        [Route("TimeLine")]

        public async Task<IActionResult> UploadTimeLine(IFormFile file, int ProID)
        {
            var result = WriteFile(file);

            var newfile = new FileResource
            {
                FileName = file.FileName,
                FileType = "timeline",
                LocalStoragePath = await result,
                UpdatedDate = DateTime.Now,
                ProjectId = ProID,
                TaskId = null
            };

            _context.Add(newfile);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        [Route("BudgetInfo")]

        public async Task<IActionResult> UploadBudgetInfo(IFormFile file, int ProID)
        {
            var result = WriteFile(file);

            var newfile = new FileResource
            {
                FileName = file.FileName,
                FileType = "budgetinfo",
                LocalStoragePath = await result,
                UpdatedDate = DateTime.Now,
                ProjectId = ProID,
                TaskId = null
            };

            _context.Add(newfile);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        [Route("ClientDoc")]

        public async Task<IActionResult> UploadClientDoc(IFormFile file, int ProID)
        {
            var result = WriteFile(file);

            var newfile = new FileResource
            {
                FileName = file.FileName,
                FileType = "clientdoc",
                LocalStoragePath = await result,
                UpdatedDate = DateTime.Now,
                ProjectId = ProID,
                TaskId = null
            };

            _context.Add(newfile);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
