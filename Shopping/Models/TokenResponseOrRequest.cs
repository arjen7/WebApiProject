namespace Shopping.Models
{
    public class GenerateTokenRequest
    {
        public int UserId { get; set; }
        public string Role { get; set; } = "";

    }
    public class GenerateTokenResponse
    {
        public string Token { get; set; } = "";
        public DateTime TokenExpireDate { get; set; }
    }
}
