using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognition.Exceptions;
using RevenueRecognition.RequestModels.ContractRequestModels;
using RevenueRecognition.Services;

namespace RevenueRecognition.Controllers;

[ApiController]
[Route("/api/contract/")]
public class ContractController(IContractService contractService) : ControllerBase
{
    [HttpPost("{clientId:int}")]
    [Authorize]
    public async Task<IActionResult> AddContract(AddContractRequestModel requestModel, int clientId)
    {
        try
        {
            var result = await contractService.AddContractAsync(requestModel, clientId);
            return Created("/api/contract/",result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ActiveContractException e)
        {
            return BadRequest(e.Message);
        }
        catch (DatePeriodIsIncorrectException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("client/{clientId:int}")]
    [Authorize]
    public async Task<IActionResult> GetContractsByClientId(int clientId)
    {
        try
        {
            var result = await contractService.GetContractByClientIdAsync(clientId);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet("{contractId:int}")]
    [Authorize]
    public async Task<IActionResult> GetContract(int contractId)
    {
        try
        {
            var result = await contractService.GetContractAsync(contractId);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
}