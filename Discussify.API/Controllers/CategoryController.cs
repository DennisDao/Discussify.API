using Application.Posts;
using CommonDataContract.Extension;
using CommonDataContract.Post;
using Discussify.API.DTOs.Categories;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discussify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly PostService _postService;
        public CategoryController(PostService postService)
        {
            _postService = postService;     
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
