using RevenueRecognition.ResponseModels.ContractResponseModels;

namespace RevenueRecognition.Models.ResponseModels.ClientResponseModel;

public class GetCompanyClientResponseModel
{
    public string Address { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Name { get; set; }
    
    public string KRS { get; set; }

    public List<GetContractResponseModel> Contracts { get; set; }
}

