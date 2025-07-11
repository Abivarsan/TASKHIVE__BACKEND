using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers.ClientSideControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadReceiptController : ControllerBase
    {
        public readonly DataContext _context;

        public UploadReceiptController(DataContext _context)
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

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\react\\Hemal\\UploadedReceipt");

                Console.WriteLine("filepath: " + filePath);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\react\\Hemal\\UploadedReceipt", fileName);

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
        [Route("ClientReceipt")]

        public async Task<IActionResult> UploadReceipt(IFormFile file, int ProID)
        {

            var result = WriteFile(file);


            var newfile = new FileResource
            {
                FileName = file.FileName,
                FileType = "receipt",
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