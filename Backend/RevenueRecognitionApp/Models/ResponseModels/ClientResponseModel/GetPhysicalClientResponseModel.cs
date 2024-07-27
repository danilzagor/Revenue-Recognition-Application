using RevenueRecognition.ResponseModels.ContractResponseModels;

namespace RevenueRecognition.Models.ResponseModels.ClientResponseModel;

public class GetPhysicalClientResponseModel
{
    public string Address { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string PESEL { get; set; }
    
    public List<GetContractResponseModel> Contracts { get; set; }
}