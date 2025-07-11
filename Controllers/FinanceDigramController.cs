
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceDigramController : ControllerBase
    {
        private readonly DataContext _digramdatacontext;
        private readonly IMapper _mapper;

        public FinanceDigramController(DataContext dataContext,IMapper mapper)
        {
            _digramdatacontext = dataContext;
            _mapper = mapper;
        }

        [HttpGet ("Projects/{projectId}/register")]
     //   [Authorize(Roles = "1,2")]
        public async Task<ActionResult<List<FinanceDigramDto>>> GetDigramValues(int projectId)
        {
            try
            {
                var projectid = await _digramdatacontext.Projects.FindAsync(projectId);
                if (projectid == null) {
                    return BadRequest();
                }
                var budgetlist = await _digramdatacontext.Budgets.FirstOrDefaultAsync(p => p.ProjectId == projectId);
                if (budgetlist == null)
                {
                    return NotFound($"Budget for project ID {projectId} not found.");
                }
                var Income = await _digramdatacontext.Transactions   
                .Where(i => i.ProjectId == projectId && i.Type == "Income")
                .SumAsync(i => i.Value);
                if (Income == 0)
                {
                    Income = 0;
                }
                var Expence = await _digramdatacontext.Transactions
                .Where(i => i.ProjectId == projectId && i.Type == "Expence")
                .SumAsync(i => i.Value);
                if (Expence == 0)
                {
                    Expence = 0;
                }
                var digramreqdata = new FinanceDigramDto
                {
                    Remaining = budgetlist.TotalCost-Expence,
                    Used= Expence,
                    Income= Income
                };
                return Ok(digramreqdata);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
