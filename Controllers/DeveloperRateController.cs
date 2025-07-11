using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperRateController : ControllerBase
    {
        private readonly DataContext _datacontext;
        private readonly IMapper _imapper;

        public DeveloperRateController(DataContext dataContext, IMapper mapper)
        {
            _datacontext = dataContext;
            _imapper = mapper;
        }
        public static List<DeveloperRate> developerrate = new List<DeveloperRate>()
        {
            new DeveloperRate()
        };
        [HttpPost("register")]
     //   [Authorize(Roles = "1")]
        public async Task<ActionResult<DeveloperRate>> AddRate(DeveloperRate updatedRate)
        {
            var newRate = _datacontext.DeveloperRates.Add(updatedRate);

            // Save changes asynchronously
            await _datacontext.SaveChangesAsync();

            // Return a 200 OK response with the updated rate
            return Ok(newRate.Entity);
        }
        /* [HttpPut("developerrate")]
         public async Task<ActionResult<List<DeveloperRate>>> UpdateRate(double developerrate)
         {
             var exsistingrate= _datacontext.DeveloperRates.FirstOrDefaultAsync();
             if (exsistingrate == null)
             {
                 var newRate = new DeveloperRate()
                 {
                     CurrentRate = developerrate
                 };
             }
             var setNewRate = developerrate;
             _datacontext.SaveChanges();
             return Ok(setNewRate);
         }*/

        [HttpGet("register")]
      //  [Authorize(Roles = "1,2,3")]
        public async Task<ActionResult<GetRateDto>> GetRate()
        {
            var currnetRate = await _datacontext.DeveloperRates.OrderByDescending(x => x.Rateid).FirstOrDefaultAsync();
            var current_devep__rate = _imapper.Map<GetRateDto>(currnetRate);

            if (currnetRate == null)
            {
                throw new Exception("Not found");
            }
            return Ok(current_devep__rate);
        }
    }
}
