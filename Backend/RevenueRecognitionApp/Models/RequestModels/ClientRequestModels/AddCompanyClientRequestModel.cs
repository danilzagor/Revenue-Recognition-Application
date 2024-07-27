using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.RequestModels;

public class AddCompanyClientRequestModel
{
    [MaxLength(50)]
    [Required]
    public string Name { get; set; }
    
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
    
    [MaxLength(14)]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "KRS can only contain numbers.")]
    [Required]
    public string KRS { get; set; }
}