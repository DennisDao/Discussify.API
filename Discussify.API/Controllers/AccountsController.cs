﻿using Api.DTOs.Users;
using Discussify.API.Extensions;
using Discussify.API.Models;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Discussify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IServer _server;

        public AccountsController(UserManager<ApplicationUser> userManager,
            IServer server, 
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _server = server;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = ApplicationUser.Create(model.FirstName, model.LastName,model.Email);

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new { UserId = user.Id, Message = "Registration successful" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpGet("UserDetails")]
        public async Task<IActionResult> UserDetails(int userId)
        {

            var user = await _userRepository.GetUserByIdAsync(userId);

            if(user == null)
            {
                return NotFound("User not found");
            }

            var dto = user.ToUserDetailResponse(_server);

            return Ok(dto);
        }

        [HttpPost("UpdateUserDetail")]
        public async Task<IActionResult> UserDetails([FromBody] UpdateUserDetailRequest request)
        {

            var user = await _userRepository.GetUserByIdAsync(request.UserId) as ApplicationUser;
            user.ChangeFirstName(request.FirstName);
            user.ChangeLastName(request.LastName);
            user.ChangeBio(request.Bio);
            user.ChangeEmail(request.Email);
            user.UserName = request.Email;

            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile image, string userID)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No file uploaded.");

            var user = await _userManager.FindByIdAsync(userID);

            var uploadsFolder = Path.Combine("wwwroot", "Avatars");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var avatar = Guid.NewGuid().ToString() + "_" + image.FileName.Replace(" ", "");
            var filePath = Path.Combine(uploadsFolder, avatar);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            
            user.ChangeAvatar(avatar);
            await _userManager.UpdateAsync(user);

            var avatarUrl = $"{_server.GetHostUrl()}/Avatars/{avatar}";

            return Ok(new { avatarUrl });
        }
    }
}
