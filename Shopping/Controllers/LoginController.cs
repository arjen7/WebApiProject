using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Interfaces;
using Shopping.Models;
using Shopping.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        readonly IAuthService authService;

        public LoginController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostAsync([FromBody]  UserLoginResponse user)
        {
            var response=await authService.LoginUserAsync(user);
            if(response.AuthenticateResult)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
