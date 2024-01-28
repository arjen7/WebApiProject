using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopping.Models.entity;
using Shopping.Models;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] int categoryid, [FromQuery] string keyword = "")
        {

            using var context = new ShoppingDbContext();
            var filteredbycategory = categoryid < 1 ? await context.Products.ToListAsync() : await context.Products.Where(p => p.CategoryId == categoryid).AsNoTrackingWithIdentityResolution().ToListAsync();
            var filteredbykeyword = string.IsNullOrEmpty(keyword) ? filteredbycategory : filteredbycategory.Where(p => p.Name.StartsWith(keyword));
            if (!filteredbykeyword.IsNullOrEmpty())
            {
                return Ok(filteredbykeyword);
            }
            return NotFound(new { Message = "Products not found" });

        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {

            using var context = new ShoppingDbContext();
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound(new { Message = "Product not found" });

        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromForm] ProductViewModel _product)
        {
            if(_product != null && !_product.Name.IsNullOrEmpty() && _product.CategoryId != null)
            {
                using var context = new ShoppingDbContext();
                var product = await context.Products.FirstOrDefaultAsync(p => p.Name == _product.Name);
                if (product == null)
                {
                    var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == _product.CategoryId);
                    if (category != null)
                    {
                        if (_product.Image == null || _product.Image.Length == 0)
                        {
                            return BadRequest("Image is required.");
                        }
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Product_Images");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + _product.Image.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await _product.Image.CopyToAsync(fileStream);
                        }
                        var imageUrl = "/Upload/Product_Images/" + uniqueFileName;
                        product = new Product { Name = _product.Name, Category = category, ImageUrl = imageUrl };
                        context.Products.Add(product);
                        context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value);
                        await context.SaveChangesAsync();
                        context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value);
                        return Ok("Product added successfuly");
                    }
                    return NotFound(new { Message = "Category not found" });
                }
                return Conflict(new { Message = "Product already have" });
            }
            return BadRequest(new {Message="Name, Category id, İmage required"});

        }
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Patch(int id,[FromForm] ProductViewModel _product)
        {

            using var context = new ShoppingDbContext();
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                string? imageUrl = string.Empty;
                if (_product.Image != null && _product.Image.Length != 0)
                {
                    string imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Product_Images");

                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        var imagePath = Path.Combine(imagesFolderPath, Path.GetFileName(product.ImageUrl));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Product_Images");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + _product.Image.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await _product.Image.CopyToAsync(fileStream);
                        }
                        imageUrl = "/Upload/Product_Images/" + uniqueFileName;
                    }
                }
                if (!_product.Name.IsNullOrEmpty()) { 
                    if(!await context.Products.AnyAsync(p => p.Name == _product.Name))
                        product.Name = _product.Name;
                }
                var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == _product.CategoryId);
                if (category != null)
                {
                    product.Category = category;
                }
                if (!imageUrl.IsNullOrEmpty()) 
                { 
                    product.ImageUrl = imageUrl; 
                }
                context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value);
                await context.SaveChangesAsync();
                return Ok("Product update successfuly");
            }
            return NotFound(new { Message = "Product not found" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            using var context = new ShoppingDbContext();
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                string imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Product_Images");

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var imagePath = Path.Combine(imagesFolderPath, Path.GetFileName(product.ImageUrl));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                context.Products.Remove(product);
                context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value);
                await context.SaveChangesAsync();
                return Ok(new { Message = "Product is removed successful" });
            }
            return NotFound(new { Message = "Product not found" });
        }
    }
}
