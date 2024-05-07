using Business.Core;
using Entity.Entities;
using Shopping.Business.Core;
using Shopping.Business.Interfaces;
using Shopping.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IUserService
    {
        public Task<BaseDto> ChangePasswordAsync(Guid Id, UserChangePasswordResponse ChangePassword);
        public Task<BaseDto> DeleteAsync(Guid Id, string password);
        public Task<UserListDto> GetAllAsync(int page);
        public Task<UserDto> GetByEmailAsync(string email);
        public  Task<UserDto> GetByIdAsync(Guid Id);
        public  Task<TokenDto> LoginAsync(UserLoginResponse loginDto);
        public  Task<BaseDto> SignupUserAsync(UserSignupResponse request);
        public Task<BaseDto> UpdateAsync(Guid Id, UserUpdateResponse updateDto);
        
    }
}
