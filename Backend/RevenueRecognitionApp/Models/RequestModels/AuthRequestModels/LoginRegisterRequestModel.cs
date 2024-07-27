using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.RequestModels.AuthRequestModels;

public class LoginRegisterRequestModel
{
    [Required] public string Login { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}