using System.Security.Cryptography;
using System.Text;

namespace Shopping.Services
{
    public class PasswordService
    {
        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16]; // 16 bayt rastgele tuz oluşturulacak
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Şifre ve tuz birleştirme
            byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

            // SHA256 kullanarak hash oluşturma
            byte[] hashedPassword = SHA256.HashData(saltedPassword);
            return Convert.ToBase64String(hashedPassword);
        }
    }
}
