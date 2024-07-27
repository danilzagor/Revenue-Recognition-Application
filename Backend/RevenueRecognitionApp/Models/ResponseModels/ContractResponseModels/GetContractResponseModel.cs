using RevenueRecognition.Models;

namespace RevenueRecognition.ResponseModels.ContractResponseModels;

public class GetContractResponseModel
{
    public ContractDetailsResponseModel Contract { get; set; }
    public SoftwareResponseModel Software { get; set; }
    public string SoftwareVersion { get; set; }
    public ClientDetailsResponseModel Client { get; set; }

    public decimal PaidAmount { get; set; }
}

public class SoftwareResponseModel
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }
}

public class ContractDetailsResponseModel
{
    public int Id { get; set; }

    public DateOnly BeginningDate { get; set; }
    
    public DateOnly EndingDate { get; set; }
    
    public string ContractStatus { get; set; }
    
    public decimal Price { get; set; }
    
    public int ActualisationPeriod { get; set; }
}

public class ClientDetailsResponseModel
{
    public int Id { get; set; }
    
    public string Address { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
}