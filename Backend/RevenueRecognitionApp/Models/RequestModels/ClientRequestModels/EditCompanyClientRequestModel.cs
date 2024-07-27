using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.RequestModels;

public class EditCompanyClientRequestModel
{
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(100)]
    public string? Address { get; set; }
    [MaxLength(254)]
    public string? Email { get; set; }
    
    [MaxLength(15)]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number can only contain numbers.")]
    public string? PhoneNumber { get; set; }
    
}