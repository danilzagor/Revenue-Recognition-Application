using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Models;

public class User
{
        [Key]
        public int Id { get; set; }
        
        public string Login { get; set; } = null!;
        
        public string Password { get; set; } = null!;

        public string Salt { get; set; }
        
        public string RefreshToken { get; set; }
        
        public DateTime RefreshTokenExp { get; set; }
        
        public List<string> Roles { get; set; } = null!;
    
}