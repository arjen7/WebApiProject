namespace Shopping.Models
{
    public class TokenResponse
    {
        public bool AuthenticateResult { get; set; }
        public string? AuthToken { get; set; }
        public DateTime AccessTokenExpireDate { get; set; }
    }
    public class UserSignupResponse
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PasswordAgain { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
    }
    public class UserLoginResponse
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class UserProductResponse
    {
        public int Productid { get; set; }
        public string Comment { get; set; } = "";
    }
}
