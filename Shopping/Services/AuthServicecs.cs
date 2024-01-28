using Microsoft.EntityFrameworkCore;
using Shopping.Interfaces;
using Shopping.Models.entity;
using Shopping.Models;

namespace Shopping.Services
{
    public class AuthService : IAuthService
    {
        readonly ITokenService tokenService;

        public AuthService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<TokenResponse> LoginUserAsync(UserLoginResponse request)
        {
            TokenResponse response = new();

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (Verification.IsValidEmail(request.Email) && Verification.IsStrongPassword(request.Password))
            {
                using var context = new ShoppingDbContext();
                var usr = await context.Users.FirstOrDefaultAsync(p => p.Email == request.Email);
                if (usr != null)
                {
                    var saltvalue = usr.SaltValue;
                    var hashpassword = PasswordService.HashPassword(request.Password, saltvalue);
                    if (hashpassword == usr.PasswordHash)
                    {
                        var generatedTokenInformation = await tokenService.GenerateToken(new GenerateTokenRequest { UserId=usr.Id,Role=usr.Role });
                        response.AuthenticateResult = true;
                        response.AuthToken = generatedTokenInformation.Token;
                        response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;
                    }
                }
            }
            else
            {
                response.AuthenticateResult = false;
            }
            return response;
        }
    }
}
