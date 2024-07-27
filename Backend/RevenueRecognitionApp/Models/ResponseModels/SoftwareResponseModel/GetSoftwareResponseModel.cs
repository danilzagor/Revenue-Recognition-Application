namespace RevenueRecognition.Models.ResponseModels.SoftwareResponseModel;

public class GetSoftwareResponseModel
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }
    
    public List<GetSoftwareVersion> SoftwareVersions { get; set; }

    public List<string> SoftwareCategories { get; set; }

    public List<GetSoftwareSales> SoftwareSales { get; set; }
}

public class GetSoftwareSales
{
    public string Name { get; set; }
    
    public DateOnly StartAt { get; set; }
    
    public DateOnly EndAt { get; set; }
    
    public decimal Value { get; set; }
}

public class GetSoftwareVersion
{
    public int Id { get; set; }
    
    public string Name { get; set; }
}