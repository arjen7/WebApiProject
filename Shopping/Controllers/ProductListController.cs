using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopping.Models;
using Shopping.Models.entity;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.Controllers
{
    [Route("api/user/productlist")]
    [ApiController]
    public class ProductListController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var userid = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
            using var context = new ShoppingDbContext();
            var user=await context.Users.FirstOrDefaultAsync(p=>p.Id==userid);
            if (user != null)
            {
                var productlists=await context.ProductLists.Where(p=>p.UserId==user.Id).Include(c=>c.UserProducts).ThenInclude(p=>p.Product).AsNoTrackingWithIdentityResolution().ToListAsync();
                if (!productlists.IsNullOrEmpty())
                    return Ok(productlists);
                return NotFound(new {Message="User dont have any product list"});
            }
            return NotFound(new { Message = "User not found" });
            
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            
            using var context = new ShoppingDbContext();
            var userid = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
            var user = await context.Users.FirstOrDefaultAsync(p => p.Id == userid);
            if (user != null)
            {
                var productlist = await context.ProductLists.Where(p=>p.UserId==userid).Include(c => c.UserProducts).ThenInclude(p=>p.Product).AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(p => p.Id == id);
                if (productlist != null)
                    return Ok(productlist);
                return NotFound(new { Message = "Product list not found" });
            }
            return NotFound(new { Message = "User not found" });
        }
        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post( [FromBody] string name)
        {
            using var context = new ShoppingDbContext();
            var userid = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
            var user = await context.Users.Include(p => p.ProductLists).FirstOrDefaultAsync(p => p.Id==userid);
            if (user != null)
            {
                var productlist = user.ProductLists.FirstOrDefault(p => p.Name == name);
                if (productlist == null)
                {
                    productlist = new ProductList() { Name = name, User = user };
                    user.ProductLists.Add(productlist);
                    context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
                    await context.SaveChangesAsync();
                    return Ok(new { Message = "Product list added successful" });
                }
                return Conflict(new { Message = "Product list already have" });
            }
            return NotFound(new { Message = "User not found" });
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> Post(int id,[FromBody] UserProductResponse response)
        {
            
            using var context = new ShoppingDbContext();
            var userid = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
            var user = await context.Users.FirstOrDefaultAsync(p => p.Id == userid);
            if (user != null)
            {
                var productlist = await context.ProductLists.Include(c => c.UserProducts).FirstOrDefaultAsync(p => p.Id == id);
                if (productlist != null && user.Id == productlist.UserId) 
                {
                    if (!productlist.IsCompleted)
                    {
                        if (productlist.UserProducts.Any(p => p.ProductId == response.Productid))
                            return Conflict(new { Message = "User Product already have" });
                        var product = await context.Products.Include(p=>p.Category).FirstOrDefaultAsync(p => p.Id == response.Productid);
                        if (product != null)
                        {
                            UserProduct usrproduct = new()
                            {
                                Comment=response.Comment,
                                Product = product,
                                ProductList = productlist,
                            };
                            productlist.UserProducts.Add(usrproduct);
                            context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
                            await context.SaveChangesAsync();
                            return Ok(new { Message = "Product added successfuly" });
                        }
                        return NotFound(new { Message = "Product not found" });
                    }
                    return BadRequest(new {Message="Product list is completed"});
                }
                return NotFound(new { Message = "Product list not found" });
            }
            return NotFound(new { Message = "User not found" });
        }
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> Patch(int id,[FromBody] bool iscompeleted)
        {
            
            using var context = new ShoppingDbContext();
            var userid = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
            var user = await context.Users.Include(p=>p.ProductLists).FirstOrDefaultAsync(p => p.Id ==userid);
            if (user != null)
            {
                var productlist = await context.ProductLists.FirstOrDefaultAsync(p => p.Id == id);
                if (productlist != null && user.Id == productlist.UserId)
                {
                    productlist.IsCompleted = iscompeleted;
                    await context.SaveChangesAsync();
                    context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
                    return Ok(new { Message = "Product list complete status changed" });
                }
                return NotFound(new { Message = "Product list not found" });
            }
            return NotFound(new { Message = "User not found" });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            
            using var context = new ShoppingDbContext();
            var userid = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
            var user = await context.Users.FirstOrDefaultAsync(p => p.Id==userid);
            if (user != null)
            {
                var productlist = await context.ProductLists.Include(p=>p.UserProducts).FirstOrDefaultAsync(p => p.Id == id);
                if (productlist != null && user.Id == productlist.UserId)
                {
                    user.ProductLists.Remove(productlist);
                    context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
                    await context.SaveChangesAsync();
                    return Ok(new { Message = "Product list removed successfuly" });
                }
                return NotFound(new { Message = "Product list not found" });
            }
            return NotFound(new { Message = "User not found" });
    
        }
        [HttpDelete("{id}/product/{userproductid}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, int userproductid)
        {
            
            using var context = new ShoppingDbContext();
            var userid = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
            var user = await context.Users.FirstOrDefaultAsync(p => p.Id==userid);
            if (user != null)
            {
                var productlist = await context.ProductLists.Where(p=>p.UserId==userid).Include(c => c.UserProducts).FirstOrDefaultAsync(p => p.Id == id);
                if (productlist != null)
                {
                    
                    var usrproduct = await context.UserProducts.FirstOrDefaultAsync(p => p.Id == userproductid);
                    if (usrproduct != null)
                    {
                        productlist.UserProducts.Remove(usrproduct);
                        context.UserId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);
                        await context.SaveChangesAsync();
                        return Ok(new { Message = "Product removed successfuly" });
                    }
                    
                    return NotFound(new { Message = "Product not found" });
                }
                return NotFound(new { Message = "Product list not found" });
            }
            return NotFound(new { Message = "User not found" });
        }
    }
}
