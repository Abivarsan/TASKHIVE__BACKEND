using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers.TaskControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAudioUploadController : ControllerBase
    {
        public readonly DataContext _context;

        public TaskAudioUploadController(DataContext _context)
        {
            this._context = _context;
        }


        private async Task<string> WriteAudio(IFormFile file)
        {
            string fileName = "";
            try
            {
                if (!file.ContentType.Contains("audio/"))
                {

                    fileName = "notAudio";
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
        [Route("AudioUpload")]

        public async Task<IActionResult> UploadAudios(IFormFile file, int ProID, int TId)
        {

            var result = WriteAudio(file);

            var name = await result;


            if (name == "notAudio")
            {
                return BadRequest();
            }
            else
            {
                var newfile = new FileResource
                {
                    FileName = file.FileName,
                    FileType = "audio",
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
