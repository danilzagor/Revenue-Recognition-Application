using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Contexts;
using RevenueRecognition.Models;

namespace RevenueRecognition.Repositories;

public interface ISoftwareRepository : IGenericRepository<Software>
{
    public new Task<Software?> GetByIdAsync(int id);

    public IEnumerable<Software> GetAll();
}

public class SoftwareRepository(DatabaseContext context) : GenericRepository<Software>(context), ISoftwareRepository
{
    public new async Task<Software?> GetByIdAsync(int id)
    {
        return await context.Software
            .Include(software => software.SoftwareVersions)
            .Include(software => software.SoftwareCategories)
            .ThenInclude(sc => sc.Category)
            .Include(software => software.SoftwareSales)
            .ThenInclude(ss => ss.Sale)
            .FirstOrDefaultAsync(software => software.Id == id);
    }

    public IEnumerable<Software> GetAll()
    {
        return context.Software
            .Include(software => software.SoftwareVersions)
            .Include(software => software.SoftwareCategories)
            .ThenInclude(sc => sc.Category)
            .Include(software => software.SoftwareSales)
            .ThenInclude(ss => ss.Sale);
    }
    
    
}