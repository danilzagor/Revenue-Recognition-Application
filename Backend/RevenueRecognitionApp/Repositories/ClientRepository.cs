using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Contexts;
using RevenueRecognition.Models;

namespace RevenueRecognition.Repositories;


public interface IClientRepository  : IGenericRepository<Client>
{
    public new Task<Client?> GetByIdAsync(int id);
    Task AddPhysicalClientAsync(PhysicalClient physicalClient);
    Task AddCompanyClientAsync(CompanyClient companyClient);
    Task<PhysicalClient?> GetPhysicalClientByIdAsync(int clientId);
    Task<CompanyClient?> GetCompanyClientByIdAsync(int clientId);
    Task<Client?> GetClientByPhoneOrEmailAsync(Client client);
    Task<List<PhysicalClient>> GetAllPhysicalClients();
    Task<List<CompanyClient>> GetAllCompanyClients();
}

public class ClientRepository(DatabaseContext context) : GenericRepository<Client>(context), IClientRepository
{
    public new Task<Client?> GetByIdAsync(int id)
    {
        return context.Clients
            .Include(client => client.Contracts)
            .FirstOrDefaultAsync(client => client.DeletedAt == null);
    }

    public async Task AddPhysicalClientAsync(PhysicalClient physicalClient)
    {
        await context.PhysicalClients.AddAsync(physicalClient);
    }

    public async Task AddCompanyClientAsync(CompanyClient companyClient)
    {
        await context.CompanyClients.AddAsync(companyClient);
    }

    public async Task<PhysicalClient?> GetPhysicalClientByIdAsync(int clientId)
    {
        return await context.PhysicalClients
            .Where(client => client.Id == clientId)
            .Include(pc => pc.Client)
            .ThenInclude(pc => pc.Contracts)
            .FirstOrDefaultAsync(client => client.Client.DeletedAt == null);
    }

    public async Task<CompanyClient?> GetCompanyClientByIdAsync(int clientId)
    {
        return await context.CompanyClients
            .Where(client => client.Id == clientId)
            .Include(cc => cc.Client)
            .ThenInclude(pc => pc.Contracts)
            .FirstOrDefaultAsync(client => client.Client.DeletedAt == null);
    }

    public async Task<Client?> GetClientByPhoneOrEmailAsync(Client client)
    {
        return await context.Clients
            .Where(clientInDatabase =>
                clientInDatabase.PhoneNumber == client.PhoneNumber || clientInDatabase.Email == client.Email)
            .FirstOrDefaultAsync(clientInDatabase => clientInDatabase.DeletedAt == null);
    }

    public async Task<List<PhysicalClient>> GetAllPhysicalClients()
    {
        return await context.PhysicalClients
            .Include(client => client.Client)
            .Where(clientInDatabase => clientInDatabase.Client.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<List<CompanyClient>> GetAllCompanyClients()
    {
        return await context.CompanyClients
            .Include(client => client.Client)
            .Where(clientInDatabase => clientInDatabase.Client.DeletedAt == null)
            .ToListAsync();
    }
}