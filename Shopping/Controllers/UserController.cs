using Business.Interface;
using Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Business.Core;
using System.Security.Claims;

namespace Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserById()
        {
            var IdString = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            

            if (Guid.TryParse(IdString, out Guid Id))
            {
                var result = await _userService.GetByIdAsync(Id);
                if (result.Status==404)
                {
                    return NotFound(result);
                }
                else if (result.Status == 200)
                {
                    result.User = new User
                    {
                        CreatedDate = result.User.CreatedDate,
                        Email = result.User.Email,
                        Id = result.User.Id,
                        LastName = result.User.LastName,
                        ModifiedDate = result.User.ModifiedDate,
                        Name = result.User.Name,
                        Role=result.User.Role,
                    };
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);  
                }
                
            }
            return BadRequest();
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> RegisterUser(UserSignupResponse registerDto)
        {
            var result = await _userService.SignupUserAsync(registerDto);
            if (result.Status == 404)
            {
                return NotFound(result);
            }
            else if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginResponse loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            if (result.Status == 404)
            {
                return NotFound(result);
            }
            else if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPatch("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserUpdateResponse updateDto)
        {
            var IdString = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            
            if (Guid.TryParse(IdString, out Guid Id))
            {
                var result = await _userService.UpdateAsync(Id,updateDto);
                if (result.Status == 404)
                {
                    return NotFound(result);
                }
                else if (result.Status == 200)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            
            return BadRequest();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] UserDeleteResponse res )
        {
            var IdString = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(IdString, out Guid Id))
            {
                var result = await _userService.DeleteAsync(Id,res.password);
                if (result.Status == 404)
                {
                    return NotFound(result);
                }
                else if (result.Status == 200)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }

            return BadRequest();

        }

        [HttpPatch("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChanngePassword([FromBody] UserChangePasswordResponse passwordDto)
        {
            var IdString = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(IdString, out Guid Id))
            {
                var result = await _userService.ChangePasswordAsync(Id, passwordDto);
                if (result.Status == 404)
                {
                    return NotFound(result);
                }
                else if (result.Status == 200)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }

            return BadRequest();
        }
    }
}
