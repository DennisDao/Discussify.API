namespace Api.DTOs.Users
{
    public class UpdateUserDetailRequest
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
    }
}
