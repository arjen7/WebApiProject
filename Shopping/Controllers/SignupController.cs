using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models.entity;
using Shopping.Services;
using Shopping.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopping.Controllers
{
    [Route("api/signup")]

    public class SignupController : ControllerBase
    {
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] UserSignupResponse user)
        {
            if (Verification.IsValidEmail(user.Email) && Verification.IsStrongPassword(user.Password)&&user.Password==user.PasswordAgain)
            {
                using var context = new ShoppingDbContext();
                var usr= await context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
                if (usr == null)
                {
                    var saltvalue = PasswordService.GenerateSalt();
                    var hashpassword = PasswordService.HashPassword(user.Password, saltvalue);
                    usr = new User()
                    {
                        Email = user.Email,
                        PasswordHash = hashpassword,
                        Name = user.Name,
                        LastName = user.LastName,
                        SaltValue = saltvalue,
                    };
                    context.Users.Add(usr);
                    await context.SaveChangesAsync();

                    return Ok(new {Message="Registration successful"});
                }
                return Conflict(new { Message = "Email already exists" });
            }
            return BadRequest(new {Message="Invalid email or pasword format"});
        }
    }
}
