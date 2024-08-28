using Application.Posts;
using Discussify.API.DTOs.Categories;
using Discussify.API.Extensions;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace Discussify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly IServer _server;
        public CategoryController(PostService postService, IServer server)
        {
            _postService = postService;
            _server = server;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllCategories()
        {
            List<CategoriesResponse> categoryResponse = new List<CategoriesResponse>();
            try
            {
                var categories = _postService.GetAllCategories();

                foreach (var category in categories)
                {
                    categoryResponse.Add(new CategoriesResponse()
                    {
                        CategoryName = category.CategoryName,
                        Image = $"{_server.GetHostUrl()}/Images/Categories/{category.Image}",
                        Description = category.Description,
                        Id = category.Id
                    }); 
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Oops!");
            }

            return Ok(categoryResponse);
        }
    }
}
