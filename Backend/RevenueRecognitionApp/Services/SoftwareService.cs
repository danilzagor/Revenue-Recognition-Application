using RevenueRecognition.Exceptions;
using RevenueRecognition.Models.ResponseModels.SoftwareResponseModel;
using RevenueRecognition.Repositories;

namespace RevenueRecognition.Services;

public interface ISoftwareService
{
    List<GetSoftwareResponseModel> GetAllSoftware();
    Task<GetSoftwareResponseModel> GetSoftwareById(int softwareId);
}

public class SoftwareService(ISoftwareRepository softwareRepository) : ISoftwareService
{
    public List<GetSoftwareResponseModel> GetAllSoftware()
    {
        var software = softwareRepository.GetAll();
        var result = software.Select(softwareInRepo => new GetSoftwareResponseModel
        {
            Id = softwareInRepo.Id,
            Description = softwareInRepo.Description,
            Name = softwareInRepo.Name,
            Price = softwareInRepo.Price,
            SoftwareCategories = softwareInRepo.SoftwareCategories
                .Select(categories => categories.Category.Name)
                .ToList(),
            SoftwareSales = softwareInRepo.SoftwareSales
                .Select(sales => new GetSoftwareSales
                {
                    StartAt = sales.Sale.StartAt,
                    EndAt = sales.Sale.EndAt,
                    Name = sales.Sale.Name,
                    Value = sales.Value
                }).ToList(),
            SoftwareVersions = softwareInRepo.SoftwareVersions
                .Select(version => new GetSoftwareVersion
                {
                    Id = version.Id,
                    Name = version.Name
                }).ToList()
        }).ToList();
        return result;
    }

    public async Task<GetSoftwareResponseModel> GetSoftwareById(int softwareId)
    {
        var software = await softwareRepository.GetByIdAsync(softwareId);

        if (software is null)
        {
            throw new NotFoundException($"Software with id:{softwareId} does not exist.");
        }

        var result = new GetSoftwareResponseModel
        {
            Id = software.Id,
            Description = software.Description,
            Name = software.Name,
            Price = software.Price,
            SoftwareCategories = software.SoftwareCategories
                .Select(categories => categories.Category.Name)
                .ToList(),
            SoftwareSales = software.SoftwareSales
                .Select(sales => new GetSoftwareSales
                {
                    StartAt = sales.Sale.StartAt,
                    EndAt = sales.Sale.EndAt,
                    Name = sales.Sale.Name,
                    Value = sales.Value
                }).ToList(),
            SoftwareVersions = software.SoftwareVersions
                .Select(version => new GetSoftwareVersion
                {
                    Id = version.Id,
                    Name = version.Name
                }).ToList()
        };
        return result;
    }
}