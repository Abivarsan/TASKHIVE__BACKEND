using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASKHIVE.Data;
using TASKHIVE.DTOs;
using TASKHIVE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TASKHIVE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Developer_FinanceController : ControllerBase
    {
        private readonly DataContext _dfinancialcontext;
        private readonly IMapper _mapper;

        public Developer_FinanceController(DataContext dataContext, IMapper mapper)
        {
            _dfinancialcontext = dataContext;
            _mapper = mapper;
        }

        [HttpPost("Developer/{developerId}/register")]
       // [Authorize(Roles = "3")]
        public async Task<ActionResult<AddPaymentDto>> GetDeveloperPayments(int developerId, int month, int year)
        {
            try
            {
                var developer = await _dfinancialcontext.Developers
                    .Include(d => d.Tasks)
                    .FirstOrDefaultAsync(d => d.DeveloperId == developerId);

                if (developer == null)
                {
                    return NotFound();
                }

                var monthlyTasks = developer.Tasks
                    .Where(t => t.TaskCompleteTime.Year == year && t.TaskCompleteTime.Month == month)
                    .ToList();

                if (!monthlyTasks.Any())
                {
                    return NotFound("No tasks found for the specified month and year");
                }

                var totalHours = monthlyTasks.Sum(t => t.TotalTaskTimeDuration);
                var currentRate = await _dfinancialcontext.DeveloperRates.Where(t=>t.UpdatedDate.Year==year && t.UpdatedDate.Month==month).OrderByDescending(x => x.Rateid).FirstOrDefaultAsync();
                double rate = currentRate.CurrentRate;
                var totalPayment = totalHours * rate;

                var payment = new Payment
                {
                    Year = year,
                    Month = month,
                    TotalMonthPayment = totalPayment,
                    DeveloperId = developerId,
                    MonthlyWorkedHours= totalHours
                };

                _dfinancialcontext.Payments.Add(payment);
                await _dfinancialcontext.SaveChangesAsync();

                var addPaymentDto = _mapper.Map<AddPaymentDto>(payment);
                return Ok(addPaymentDto);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + innerExceptionMessage);
            }
        }

        [HttpGet("Payment/{developerId}/register")]
      //  [Authorize(Roles = "3")]
        public async Task<ActionResult<Payment>> GetPayment(int developerId, int month, int year)
        {
            try
            {
                var monthPayment = await _dfinancialcontext.Payments
                    .Where(p => p.DeveloperId == developerId && p.Year == year && p.Month == month)
                    .OrderByDescending(p => p.PaymentId)
                    .FirstOrDefaultAsync();

                if (monthPayment == null)
                {
                    return NotFound("No payments found for the specified month and year");
                }


                return Ok(monthPayment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("Monthlyrate")]
        //  [Authorize(Roles = "3")]
        public async Task<ActionResult<Payment>> GetMonthlyRate(int month, int year)
        {
            try
            {
                var monthlyrate = await _dfinancialcontext.DeveloperRates.Where(t => t.UpdatedDate.Year == year && t.UpdatedDate.Month == month).
                    OrderByDescending(x => x.Rateid).FirstOrDefaultAsync();


                if (monthlyrate == null)
                {
                    return NotFound("No rate found for the specified month and year");
                }


                return Ok(monthlyrate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
    }
}
