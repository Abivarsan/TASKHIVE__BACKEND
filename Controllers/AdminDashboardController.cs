using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASKHIVE.Data;
using TASKHIVE.DTOs;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly DataContext _datacontext;
        private readonly IMapper _mapper;

        public AdminDashboardController(DataContext datacontext, IMapper mapper)
        {
            _datacontext = datacontext;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<List<GetDashboardDto>>> GetDashboard()
        {
            var totaladmins= await _datacontext.Admins.CountAsync();
            var totalmanagers = await _datacontext.ProjectManagers.CountAsync();
            var totaldevelopers= await _datacontext.Developers.CountAsync();
            var totalprojects = await _datacontext.Projects.CountAsync();

            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var totalIncome = await _datacontext.Transactions
                .Where(t => t.Type == "Income" && t.Date >= startOfMonth && t.Date <= endOfMonth)
            .SumAsync(t => t.Value);

            var totalExpense = await _datacontext.Transactions
                .Where(t => t.Type == "Expence" && t.Date >= startOfMonth && t.Date <= endOfMonth)
                .SumAsync(t => t.Value);
            var dashboarddata = new GetDashboardDto
            {
                TotalAdmins=totaladmins,
                TotalManagers= totalmanagers,
                TotalDevelopers = totaldevelopers,
                TotalProjects= totalprojects,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
            };
            return Ok(dashboarddata);
        }

    }
}
