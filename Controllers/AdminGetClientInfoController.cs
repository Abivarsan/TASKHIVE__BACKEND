using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using TASKHIVE.Data;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminGetClientInfoController : ControllerBase
    {
        public readonly DataContext _context;

        public AdminGetClientInfoController(DataContext _context)
        {
            this._context = _context;

        }


        public class AdminGetClientInfoDTO 
        {
            public int ClientId { get; set; }
            public string ClientName { get; set; }
            public string ClientDescription { get; set; }
            public string NIC { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string ContactNumber { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientInfo(int CId)
        {
            try
            {
                var client = await _context.Clients
                    .Where(e => e.ClientId == CId)
                    .Select(e => new AdminGetClientInfoDTO
                    {
                        ClientId = e.ClientId,
                        ClientName = e.ClientName,
                        ClientDescription = e.ClientDescription,
                        NIC = e.NIC,
                        Address = e.Address,
                        Email = e.Email,
                        ContactNumber = e.ContactNumber
                    }
                    ).ToListAsync();

                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
