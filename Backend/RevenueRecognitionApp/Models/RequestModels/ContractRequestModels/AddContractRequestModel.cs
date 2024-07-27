using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.RequestModels.ContractRequestModels;

public class AddContractRequestModel
{
    public DateOnly BeginningDate { get; set; }
    public DateOnly EndingDate { get; set; }
    
    [RegularExpression(@"^[1-4]*$", ErrorMessage = "Actualisation period can only be 1, 2, 3 or 4 years.")]
    public int ActualisationPeriod { get; set; }

    public int SoftwareId { get; set; }
    public int SoftwareVersionId { get; set; }
}