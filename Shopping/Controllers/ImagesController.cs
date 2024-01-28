using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.Controllers
{
    [Route("api/product/image")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpGet("imageurl")]
        [Authorize]
        public async Task<IActionResult> Get(string ImageUrl)
        {
           
            string imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory());
            string filePath = Path.Combine(imagesFolderPath, ImageUrl.TrimStart('/'));
            try { 
            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var contentType = GetContentType(ImageUrl);
                return File(fileBytes,contentType);
            }
            else
            {
                return NotFound(new { Message = "Image not found" });
            }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"An error occurred: {ex.Message}" });
            }
            
        }
        private static string GetContentType(string fileName)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream"; 
            }
            return contentType;
        }
    }
}