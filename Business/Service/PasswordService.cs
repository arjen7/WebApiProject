using Business.Interface;
using Entity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using static Shopping.Business.Services.PasswordService;

namespace Shopping.Business.Services
{
    public class PasswordService : IPasswordService
    {
        PasswordHasher<User> hasher = new();
        public string HashPassword(User user, string password)
        {
            return hasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string hashPassword, string providerPassword)
        {
            var result = hasher.VerifyHashedPassword(user, hashPassword, providerPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
