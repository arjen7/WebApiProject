
using Azure;
using Business.Core;
using Business.Interface;
using Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Shopping.Business.Core;
using Shopping.Business.Interfaces;
using Shopping.Business.Services;
using Shopping.Data.Interface;

namespace Shopping.Business.Service
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IPasswordService passwordService, IAuthService authService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _authService = authService;
        }
        public async Task<BaseDto> ChangePasswordAsync(Guid Id, UserChangePasswordResponse ChangePassword)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            var respone = new BaseDto();
            if (user == null)
            {
                respone.DeveloperMessage = "User not found";
                respone.Status = 404;
                respone.Message = "Not Found";
                return respone;
            }
            if (_passwordService.VerifyPassword(user, user.Password, ChangePassword.Password))
            {
                var control = Verification.IsPasswordValid(ChangePassword.NewPassword);
                if (!control)
                {
                    respone.DeveloperMessage = "new Password not valid";
                    respone.Status = 400;
                    respone.Message = "Bad Request";
                    return respone;
                }
                user.Password = _passwordService.HashPassword(user, ChangePassword.NewPassword);
                await _userRepository.UpdateAsync(user.Id, user);
                respone.DeveloperMessage = "Password changed is successful";
                respone.Status = 200;
                respone.Message = "Success";
                return respone;
                
            }
            respone.DeveloperMessage = "Updated not approved";
            respone.Status = 400;
            respone.Message = "Bad Request";
            return respone;
        }

        public async Task<BaseDto> DeleteAsync(Guid Id, string password)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            var response = new BaseDto();
            if (user == null)
            {
                response.DeveloperMessage = "User not found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            if (!_passwordService.VerifyPassword(user, user.Password, password))
            {
                response.DeveloperMessage= "Password  is wrong";
                response.Status = 400;
                response.Message = "Bad Request";
                return response;
            }
            
            await _userRepository.DeleteAsync(user.Id);
            response.DeveloperMessage = "User removed successful";
            response.Status = 200;
            response.Message = "Succes";
            return response;

        }

        public async Task<UserListDto> GetAllAsync(int page)
        {
            var users=new UserListDto();
            users.Message = "Succes";
            users.Status=200;
            users.Users=await _userRepository.GetAllAsync(page);
            return users;
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            var response=new UserDto();
            if(user == null)
            {
                response.DeveloperMessage = "User Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            response.Message = "Succes";
            response.Status = 200;
            response.User = user;
            return response;
        }

        public async Task<UserDto> GetByIdAsync(Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            var response = new UserDto();
            if (user == null)
            {
                response.DeveloperMessage = "User Not Found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            response.Message = "Succes";
            response.Status = 200;
            response.User = user;
            return response;
        }

        public async Task<TokenDto> LoginAsync(UserLoginResponse loginDto)
        {
            var response = new TokenDto();
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                response.DeveloperMessage = "User not found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            if (!_passwordService.VerifyPassword(user, user.Password, loginDto.Password))
            {
                response.DeveloperMessage="Password  is wrong";
                response.Status = 400;
                response.Message = "Bad Request";
                return response;
            }

            response = await _authService.LoginUserAsync(user,response);
            response.DeveloperMessage = "Login succesfull";
            response.Message = "Succes";
            response.Status=200;
            return response ;
        }
        
        public async Task<BaseDto> SignupUserAsync(UserSignupResponse request)
        {
            var userEmail = await _userRepository.GetByEmailAsync(request.Email);
            var newguid = Guid.NewGuid();

            var response = new BaseDto();

            if (!Verification.IsValidEmail(request.Email))
            {
                response.DeveloperMessage ="Email is not valid";
                response.Message = "Bad Request";
                response.Status = 400;
                return response;
            }

            if (!Verification.IsPasswordValid(request.Password))
            {
                response.DeveloperMessage = "Password is not valid";
                response.Message = "Bad Request";
                response.Status = 400;
                return response;
            }

            if (userEmail != null)
            {
                response.DeveloperMessage ="Email is exist";
                response.Message = "Conflict";
                response.Status = 300;
                return response;
            }
            while (await _userRepository.GetByIdAsync(newguid) != null)
            {
                newguid = Guid.NewGuid();
            }
            
            var newuser = new User
            {
                Id = newguid,
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                Password = _passwordService.HashPassword(null, request.Password),
                Role="User"
            };
            await _userRepository.AddAsync(newuser);
            response.Message = "Succes";
            response.Status = 201;
            response.DeveloperMessage = "Signin succesfull";
            return response;
        }

        public async Task<BaseDto> UpdateAsync(Guid Id, UserUpdateResponse updateDto)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            var response = new BaseDto();
            bool control = false;
            if (user == null)
            {
                response.DeveloperMessage = "User not found";
                response.Message = "Not Found";
                response.Status = 404;
                return response;
            }
            if (!updateDto.Email.IsNullOrEmpty() && Verification.IsValidEmail(updateDto.Email))
            {
                if (_userRepository.GetByEmailAsync(updateDto.Email).Result != null)
                {
                    response.DeveloperMessage = "Email is exist";
                    response.Message = "Conflict";
                    response.Status = 300;
                    return response;
                }
                user.Email = updateDto.Email;
            }    
            if (!updateDto.Name.IsNullOrEmpty())  { user.Name = updateDto.Name; control = true; }
            if (!updateDto.LastName.IsNullOrEmpty()) { user.LastName = updateDto.LastName; control = true; }

            if (control)
            {
                await _userRepository.UpdateAsync(Id, user);
                    response.DeveloperMessage = "Updated successful";
                    response.Message = "Succes";
                    response.Status = 200;
                    return response;
            }
            response.DeveloperMessage="Update not Approved";
            response.Message = "Bad Request";
            response.Status = 400;
            return response;

        }
        
    }
}
