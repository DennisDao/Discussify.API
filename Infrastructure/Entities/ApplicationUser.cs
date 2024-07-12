using Domain.AggegratesModel.UserAggegrate;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
