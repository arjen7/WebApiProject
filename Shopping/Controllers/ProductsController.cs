using Business.Interface;
using Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopping.Business.Core;
using System.Security.Claims;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> Get([FromQuery] int page, [FromQuery] Guid? categoryid = null, [FromQuery] string keyword = "")
        {

            var result = await _productService.GetAll(page, categoryid, keyword);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else if (result.Status == 404)
            {
                return NotFound(result);
            }
            else
                return BadRequest(result);
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _productService.Get(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else if (result.Status == 404)
            {
                return NotFound(result);
            }
            else
                return BadRequest(result);

        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromForm] ProductResponse res)
        {
            var result = await _productService.Create(res);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else if (result.Status == 404)
            {
                return NotFound(result);
            }
            else
                return BadRequest(result);

        }
        [HttpPatch("update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Patch(Guid id, [FromForm] ProductUpdateResponse res)
        {
            var result = await _productService.Update(id,res);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else if (result.Status == 404)
            {
                return NotFound(result);
            }
            else
                return BadRequest(result);

        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productService.Delete(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else if (result.Status == 404)
            {
                return NotFound(result);
            }
            else
                return BadRequest(result);
        }
    }
}
