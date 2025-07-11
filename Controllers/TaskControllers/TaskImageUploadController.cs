using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers.TaskControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskImageUploadController : ControllerBase
    {
        public readonly DataContext _context;

        public TaskImageUploadController(DataContext _context)
        {
            this._context = _context;
        }


        private async Task<string> WriteImg(IFormFile file)
        {
            string fileName = "";
            try
            {
                if (!file.ContentType.Contains("image/"))
                {

                    fileName = "notImage";
                }
                else
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Not uploaded");

            }
            return (fileName);
        }

        [HttpPost]
        [Route("ImgUpload")]

        public async Task<IActionResult> UploadImages(IFormFile file, int ProID, int TId)
        {

            var result = WriteImg(file);

            var name = await result;


            if (name == "notImage")
            {
                return BadRequest();
            }
            else
            {
                var newfile = new FileResource
                {
                    FileName = file.FileName,
                    FileType = "image",
                    LocalStoragePath = await result,
                    UpdatedDate = DateTime.Now,
                    ProjectId = ProID,
                    TaskId = TId
                };

                _context.Add(newfile);
                await _context.SaveChangesAsync();

                return Ok();
            }
        }
    }
}
