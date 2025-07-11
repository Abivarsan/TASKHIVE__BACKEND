using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminGetProjectDetailsController : ControllerBase
    {
        public readonly DataContext _context;
        public AdminGetProjectDetailsController(DataContext _context)
        {
            this._context = _context;
        }

       
    }
}
