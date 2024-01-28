using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopping.Models.entity;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            using var context = new ShoppingDbContext();
            var categories= await context.Categories.AsNoTrackingWithIdentityResolution().ToListAsync();
            if(!categories.IsNullOrEmpty())
                return Ok(categories);
            return NotFound(new { Message="Categories not have any elements" });
    }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            using var context = new ShoppingDbContext();
            var category = await context.Categories.FirstOrDefaultAsync(p=>p.Id==id);
            if(category!=null)
                return Ok(category);
            return NotFound(new {Message="Category not found"});
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Patch(int id, [FromBody] string name)
        {
            
            using var context = new ShoppingDbContext();
            var categories = await context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            if (categories != null)
            {
                if (!await context.Categories.AnyAsync(p => p.Name == name))
                {
                    categories.Name = name;
                    context.Categories.Update(categories);
                    context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value);
                    await context.SaveChangesAsync();
                    return Ok(new { Message = "Category name changed successfuly" });
                }
                return Conflict(new {Message="Category name is already have"});
            }
            return NotFound(new { Message = "Category not found" });
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post( [FromBody] string name)
        {
            using var context = new ShoppingDbContext();
            var categories = await context.Categories.FirstOrDefaultAsync(p => p.Name ==name);
            if (categories == null)
            {
                categories = new Category() { Name = name };
                await context.Categories.AddAsync(categories);
                context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value);
                await context.SaveChangesAsync();
                return Ok(new { Message = "Category Added Successfuly" });
            }
            return Conflict(new { Message = "Category already have" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            using var context = new ShoppingDbContext();
            var category = await context.Categories.Include(p=>p.Products).ThenInclude(p=>p.UserProducts).FirstOrDefaultAsync(p=>p.Id==id);
            if (category != null) 
            {
                context.Categories.Remove(category);
                context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value);
                await context.SaveChangesAsync();
                return Ok(new { Message = "Category removed Successfuly" });
            }
            return NotFound(new {Message="Category not found"});
        }
    }
}
