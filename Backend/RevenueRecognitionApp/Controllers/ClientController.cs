using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognition.Exceptions;
using RevenueRecognition.RequestModels;
using RevenueRecognition.Services;

namespace RevenueRecognition.Controllers;

[ApiController]
[Route("/api/clients")]
public class ClientController(IClientService clientService) : ControllerBase
{
    [HttpGet("physical")]
    [Authorize]
    public async Task<IActionResult> GetAllPhysicalClients()
    {
        var clients = await clientService.GetAllPhysicalClients();
        return Ok(clients);
    }

    [HttpGet("company")]
    [Authorize]
    public async Task<IActionResult> GetAllCompanyClients()
    {
        var clients = await clientService.GetAllCompanyClients();
        return Ok(clients);
    }

    [HttpGet("physical/{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetPhysicalClientById(int id)
    {
        try
        {
            var client = await clientService.GetPhysicalClientById(id);
            return Ok(client);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("company/{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetCompanyClientById(int id)
    {
        try
        {
            var client = await clientService.GetCompanyClientById(id);
            return Ok(client);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("physical")]
    [Authorize]
    public async Task<IActionResult> AddPhysicalClient(AddPhysicalClientRequestModel requestModel)
    {
        try
        {
            await clientService.AddPhysicalClientAsync(requestModel);
            return Created();
        }
        catch (ClientAlreadyExistsException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("company")]
    [Authorize]
    public async Task<IActionResult> AddCompanyClient(AddCompanyClientRequestModel requestModel)
    {
        try
        {
            await clientService.AddCompanyClientAsync(requestModel);
            return Created();
        }
        catch (ClientAlreadyExistsException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("physical/{clientId:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeletePhysicalClient(int clientId)
    {
        try
        {
            await clientService.DeletePhysicalClientAsync(clientId);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("physical/{clientId:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> EditPhysicalClient(int clientId, EditPhysicalClientRequestModel requestModel)
    {
        try
        {
            await clientService.EditPhysicalClientAsync(clientId, requestModel);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("company/{clientId:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> EditCompanyClient(int clientId, EditCompanyClientRequestModel requestModel)
    {
        try
        {
            await clientService.EditCompanyClientAsync(clientId, requestModel);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}