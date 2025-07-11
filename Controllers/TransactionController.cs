using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TASKHIVE.Data;
using TASKHIVE.Models;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Authorization;
using TASKHIVE.DTOs;


[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly DataContext _transacdatacontext;
    private readonly IMapper _mapper;

    public TransactionController(DataContext datacontext, IMapper mapper)
    {
        _transacdatacontext = datacontext;
        _mapper = mapper;
    }

    [HttpGet("register")]
    //   [Authorize(Roles = "1")]
    public async Task<ActionResult<List<GetProjectDto>>> GetProject()
    {
        var projectlist = await _transacdatacontext.Projects.ToListAsync();
        return Ok(projectlist);
    }

    [HttpPost("Project/{projectId}/register")]
    // [Authorize(Roles = "1")]
    public async Task<ActionResult<Transaction>> AddTransaction(AddTransacDto invoice, int projectId)
    {
        try
        {
            var project = await _transacdatacontext.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            var transactionType = invoice.Type;
            Transaction transaction = new Transaction
            {
                Value = invoice.Value,
                Description = invoice.Description,
                Type = invoice.Type,
                Date = invoice.Date,
                ProjectId = projectId
            };

            if (transactionType == "Income")
            {
                transaction.Income = invoice.Value;
                transaction.Expence = 0;
            }
            else if (transactionType == "Expence")
            {
                transaction.Expence = invoice.Value;
                transaction.Income = 0;
            }
            else
            {
                return BadRequest("Invalid transaction type");
            }

            _transacdatacontext.Transactions.Add(transaction);
            await _transacdatacontext.SaveChangesAsync();

            return Ok(transaction);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("Projects/{projectId}/register")]
    //[Authorize(Roles = "1")]
    public async Task<ActionResult<List<GetTransacDto>>> GetTransactions(int projectId)
    {
        try
        {
            var project = await _transacdatacontext.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound("Project does not exist");
            }

            var transactions = await _transacdatacontext.Transactions
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();

            var transactionDtos = _mapper.Map<List<GetTransacDto>>(transactions);
            return Ok(transactionDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("Transaction/{transacId}/{projectId}/register")]
    //  [Authorize(Roles = "1")]
    public async Task<ActionResult> DeleteTransaction(int transacId, int projectId)
    {
        var transaction = await _transacdatacontext.Transactions.FirstOrDefaultAsync(p => p.TransacId == transacId);
        if (transaction == null)
        {
            return NotFound("Transaction not found");
        }

        _transacdatacontext.Transactions.Remove(transaction);
        await _transacdatacontext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("Transaction/{transacId}/register")]
    public async Task<ActionResult<Transaction>> UpdateTransaction(int transacId, [FromBody] AddTransacDto updatedTransac)
    {
        try
        {
            var existingTransaction = await _transacdatacontext.Transactions.FindAsync(transacId);
            if (existingTransaction == null)
            {
                return NotFound("Transaction not found");
            }

            // Update the transaction properties only if they are provided
            if (!string.IsNullOrEmpty(updatedTransac.Description))
            {
                existingTransaction.Description = updatedTransac.Description;
            }

            if (updatedTransac.Date != DateTime.MinValue)
            {
                existingTransaction.Date = updatedTransac.Date;
            }

            if (updatedTransac.Value != 0)
            {
                existingTransaction.Value = updatedTransac.Value;
            }

            if (!string.IsNullOrEmpty(updatedTransac.Type) && existingTransaction.Type != updatedTransac.Type)
            {
                if (updatedTransac.Type == "Income")
                {
                    existingTransaction.Expence = 0;
                    existingTransaction.Income = updatedTransac.Value;
                }
                else if (updatedTransac.Type == "Expence")
                {
                    existingTransaction.Income = 0;
                    existingTransaction.Expence = updatedTransac.Value;
                }
                existingTransaction.Type = updatedTransac.Type;
            }
            else if (existingTransaction.Type == updatedTransac.Type)
            {
                if (existingTransaction.Type == "Income")
                {
                    existingTransaction.Income = updatedTransac.Value;
                }
                else if (existingTransaction.Type == "Expence")
                {
                    existingTransaction.Expence = updatedTransac.Value;
                }
            }

            _transacdatacontext.Entry(existingTransaction).State = EntityState.Modified;
            await _transacdatacontext.SaveChangesAsync();

            return Ok(existingTransaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }
}








/*var excistingtransac = await _transacdatacontext.Transactions.FirstOrDefaultAsync(t => t.TransacId == transacId);
if (excistingtransac == null)
{
    return BadRequest();
}
var currentIncome = await _transacdatacontext.Transactions
.Where(i => i.TransacId == transacId && i.Type == "Income")
.SumAsync(i => i.Value);
var currentExpence = await _transacdatacontext.Transactions
.Where(i => i.TransacId == transacId && i.Type == "Expence")
.SumAsync(i => i.Value);


// excistingtransac.Description = updatetransac.Description != null && updatetransac.Type == null && updatetransac.Value == 0 ? updatetransac.Description : excistingtransac.Description;

if (updatetransac.Description == null && updatetransac.Value != 0 && updatetransac.Type != null) //value and type both are changed ans dis not changed
{

    if (updatetransac.Type == "Income")
    {
        excistingtransac.Expence = currentExpence - excistingtransac.Value;
        excistingtransac.Income = currentIncome + updatetransac.Value + 3000;
    }
    else if (updatetransac.Type == "Expence")
    {
        excistingtransac.Income = currentIncome - excistingtransac.Value;
        excistingtransac.Expence = currentExpence + updatetransac.Value + 3000;
    }
    excistingtransac.Type = updatetransac.Type;
    excistingtransac.Value = updatetransac.Value;
    _transacdatacontext.Transactions.Update(excistingtransac);
    await _transacdatacontext.SaveChangesAsync();


}
return Ok("Updated");
 else if (updatetransac.Description != null && updatetransac.Value != 0 && updatetransac.Type != null) //value and type and dsiscripton are changed
  {
      excistingtransac.Description = updatetransac.Description;
      excistingtransac.Value = updatetransac.Value;
      excistingtransac.Type = updatetransac.Type;


      if (updatetransac.Type == "Income")
      {

          excistingtransac.Income = currentIncome + excistingtransac.Value;
      }
      else if (updatetransac.Type == "Expence")
      {

          excistingtransac.Expence = currentExpence + excistingtransac.Value;
      }
      return Ok(excistingtransac);
  }
else  if (updatetransac.Description == null && updatetransac.Value != 0 && updatetransac.Type == null)  //only value changed not type and discription not chaged
  {
      excistingtransac.Description = excistingtransac.Description;
      excistingtransac.Type = excistingtransac.Type;
      var updatedval = updatetransac.Value - excistingtransac.Value; //updated value calculate
      excistingtransac.Value = updatetransac.Value;
      if (excistingtransac.Type == "Income")
      {

          excistingtransac.Income = currentIncome + updatedval;
      }
      else if (excistingtransac.Type == "Expence")
      {

          excistingtransac.Expence = currentExpence + updatedval;
      }

  }
  else if (updatetransac.Description == null && updatetransac.Value == 0 && updatetransac.Type != null && excistingtransac.Type != updatetransac.Type) //value and dis not changed and only type change
  {
      excistingtransac.Description = excistingtransac.Description;
      excistingtransac.Value = excistingtransac.Value;
      if (excistingtransac.Type == "Income" && updatetransac.Type == "Expence")
      {
          excistingtransac.Income = currentIncome - excistingtransac.Value;
          excistingtransac.Expence = currentExpence + excistingtransac.Value;
      }
      else if (excistingtransac.Type == "Expence" && updatetransac.Type == "Income")
      {
          excistingtransac.Expence = currentExpence - excistingtransac.Value;
          excistingtransac.Income = currentIncome + excistingtransac.Value;
      }
      return Ok(excistingtransac);
  }
  else if (updatetransac.Description != null && updatetransac.Value == 0 && updatetransac.Type != null && excistingtransac.Type != updatetransac.Type) //value not changed and only and discription type change
  {
      excistingtransac.Description = updatetransac.Description;
      excistingtransac.Value = excistingtransac.Value;
      if (excistingtransac.Type == "Income" && updatetransac.Type == "Expence")
      {
          excistingtransac.Income = currentIncome - excistingtransac.Value;
          excistingtransac.Expence = currentExpence + excistingtransac.Value;
      }
      else if (excistingtransac.Type == "Expence" && updatetransac.Type == "Income")
      {
          excistingtransac.Expence = currentExpence - excistingtransac.Value;
          excistingtransac.Income = currentIncome + excistingtransac.Value;
      }
      return Ok(excistingtransac);
  }
  else  //any not selected
  {
      excistingtransac.Value = excistingtransac.Value;
      excistingtransac.Type = excistingtransac.Type;
      excistingtransac.Description = excistingtransac.Description;
      return Ok(excistingtransac);
  }

   _transacdatacontext.Transactions.Update(excistingtransac);
   await _transacdatacontext.SaveChangesAsync();
   return Ok(excistingtransac);*/