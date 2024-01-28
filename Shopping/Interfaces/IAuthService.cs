using Shopping.Models;

namespace Shopping.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenResponse> LoginUserAsync(UserLoginResponse request);
    }
}
