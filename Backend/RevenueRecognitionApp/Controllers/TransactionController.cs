using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognition.Exceptions;
using RevenueRecognition.RequestModels.TransactionRequestModels;
using RevenueRecognition.Services;

namespace RevenueRecognition.Controllers;

[ApiController]
[Route("/api/transaction/")]
public class TransactionController(ITransactionService service) : ControllerBase
{
    [HttpPost]
    [Route("{contractId:int}")]
    [Authorize]
    public async Task<IActionResult> MakeTransaction(int contractId, MakeTransactionRequestModel requestModel)
    {
        try
        {
            await service.MakeTransactionAsync(contractId, requestModel);
            return Created();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e) when (e is ContractIsOutdatedException or ContractIsSignedException)
        {
            return BadRequest(e.Message);
        }
    }
}