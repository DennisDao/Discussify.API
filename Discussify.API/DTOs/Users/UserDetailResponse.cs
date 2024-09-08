using Discussify.API.Extensions;
using Domain.AggegratesModel.UserAggegrate;
using Microsoft.AspNetCore.Hosting.Server;

namespace Api.DTOs.Users
{
    public static class UserDetailResponse
    {
        public static object ToUserDetailResponse(this IUser user, IServer server)
        {
            return new
            {
                Avatar = $"{server.GetHostUrl()}/avatars/{user.Avatar}",
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Bio = user.Bio,
                WhenCrearted = user.WhenCreated
            };
        }
    }
}
