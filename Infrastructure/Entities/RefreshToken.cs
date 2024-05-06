using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public  class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime WhenCreated { get; set; }

        public DateTime WhenExpire { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
    }
}
