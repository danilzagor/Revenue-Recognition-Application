using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.RequestModels;

public class EditPhysicalClientRequestModel
{
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "First name can only contain letters and spaces.")]
    public string? FirstName { get; set; }
    
    [MaxLength(75)]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Last name can only contain letters and spaces.")]
    public string? LastName { get; set; }
    
    [MaxLength(100)]
    public string? Address { get; set; }
    
    [MaxLength(254)]
    public string? Email { get; set; }
    
    [MaxLength(15)]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number can only contain numbers.")]
    public string? PhoneNumber { get; set; }
    

    
}