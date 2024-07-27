using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.RequestModels;

public class AddPhysicalClientRequestModel
{
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "First name can only contain letters and spaces.")]
    [Required]
    public string FirstName { get; set; }
    
    [MaxLength(75)]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Last name can only contain letters and spaces.")]
    [Required]
    public string LastName { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Address { get; set; }
    
    [MaxLength(254)]
    [Required]
    public string Email { get; set; }
    
    [MaxLength(15)]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number can only contain numbers.")]
    [Required]
    public string PhoneNumber { get; set; }
    
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "PESEL can only contain numbers.")]
    [MaxLength(11)]
    [Required]
    public string PESEL { get; set; }
}