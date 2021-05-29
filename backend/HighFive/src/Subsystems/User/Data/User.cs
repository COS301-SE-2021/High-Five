using System;
using System.ComponentModel.DataAnnotations;

namespace src.Subsystems.User.Data
{
    public class User
    {
        #nullable enable
        [Key] 
        private Guid UserId { get; set; }

        private string FirstName { get; set; }

        private string LastName { get; set; }

        private string Username { get; set; }

        private string Password { get; set; }

        [Required]
        private string Email { get; set; }
        
        private string? ActivationCode { get; set; }
        
        private string? PasswordResetCode { get; set; }
        
    }
}