
using Business.Core;
using Entity.Entities;
using Shopping.Business.Core;

namespace Shopping.Business.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenDto> LoginUserAsync(User user, TokenDto response);
    }
}
