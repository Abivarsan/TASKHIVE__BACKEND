using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetClientNamesController : ControllerBase
    {
        public readonly DataContext _context;

        public GetClientNamesController(DataContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetCNames()
        {
            var clients = await _context.Clients
                .Select(e => new GetClientNamesDTO
                {
                    ClientId = e.ClientId,
                    ClientName = e.ClientName
                }).ToListAsync();

            if(clients == null)
            {
                return NotFound();
            }

            return Ok(clients);
        }
    }
}
