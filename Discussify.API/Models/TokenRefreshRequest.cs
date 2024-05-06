using System.ComponentModel.DataAnnotations;

namespace Discussify.API.Models
{
    public class TokenRefreshRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
