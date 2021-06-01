using System;
using System.ComponentModel.DataAnnotations;

namespace src.Subsystems.User.Data
{
    public class User
    {
        #nullable enable
        [Key] 
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
        
        public string? ActivationCode { get; set; }
        
        public string? PasswordResetCode { get; set; }
        
    }
}