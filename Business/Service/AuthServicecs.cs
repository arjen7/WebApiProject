using Business.Core;
using Entity.Entities;
using Shopping.Business.Core;
using Shopping.Business.Interfaces;

namespace Shopping.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService tokenService;

        public AuthService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<TokenDto> LoginUserAsync(User user,TokenDto response)
        {
            var generatedTokenInformation = await tokenService.GenerateToken(new GenerateTokenRequest { UserId=user.Id,Role=user.Role });
            response.AuthToken = generatedTokenInformation.Token;
            response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;
            return response;
        }
    }
}
