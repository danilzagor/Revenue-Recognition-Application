using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognition.Exceptions;
using RevenueRecognition.Services;

namespace RevenueRecognition.Controllers;

[ApiController]
[Route("/api/software")]
public class SoftwareController(ISoftwareService service) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult GetAllSoftware()
    {
        var result = service.GetAllSoftware();
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("{softwareId:int}")]
    public async Task<IActionResult> GetSoftwareById(int softwareId)
    {
        try
        {
            var result = await service.GetSoftwareById(softwareId);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        
    }
}