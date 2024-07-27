using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognition.Exceptions;
using RevenueRecognition.Services;

namespace RevenueRecognition.Controllers;
[ApiController]
[Route("/api/revenue/")]
public class RevenueController(IRevenueService service) : ControllerBase
{
    [HttpGet("actual")] 
    [Authorize]
    public async Task<IActionResult> GetActualRevenue(int? productId = null, string? currency = null)
    {
        try
        {
            var result = await service.GetActualRevenueAsync(productId, currency);
            return Ok($"{decimal.Round(result,3)} {(currency ?? "PLN").ToUpperInvariant()}");
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (HttpRequestException e)
        {
            return BadRequest($"Currency code:{currency} does not exist.");
        }
        
    }
    
    [HttpGet("expected")] 
    [Authorize]
    public async Task<IActionResult> GetExpectedRevenue(int? productId = null, string? currency = null)
    {
        try
        {
            var result = await service.GetExpectedRevenueAsync(productId, currency);
            return Ok($"{decimal.Round(result,3)} {(currency ?? "PLN").ToUpperInvariant()}");
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (HttpRequestException e)
        {
            return BadRequest($"Currency code:{currency} does not exist.");
        }
        
    }
}