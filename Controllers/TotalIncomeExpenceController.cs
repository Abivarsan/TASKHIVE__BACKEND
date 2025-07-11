using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;
using TASKHIVE.DTOs;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TotalIncomeExpenceController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TotalIncomeExpenceController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<List<MonthlyFinanceDto>>> TotalYearIncomeExpence(int year)
        {
            double[] totalmonthincome = new double[12];
            double[] totalmonthlyexpence = new double[12];

            for (int i = 1; i <= 12; i++)
            {
                totalmonthincome[i - 1] = await _context.Transactions
                    .Where(t => t.Date.Year == year && t.Date.Month == i && t.Type == "Income")
                    .SumAsync(t => t.Value);

                totalmonthlyexpence[i - 1] = await _context.Transactions
                    .Where(t => t.Date.Year == year && t.Date.Month == i && t.Type == "Expence")
                    .SumAsync(t => t.Value);
            }
            var result = new List<MonthlyFinanceDto>();
            for (int i = 0; i < 12; i++)
            {
                result.Add(new MonthlyFinanceDto
                {
                    Month = i + 1, 
                    Income = totalmonthincome[i],
                    Expence = totalmonthlyexpence[i]
                });
            }

            return Ok(result);


        }


    }
}
