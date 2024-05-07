namespace Shopping.Business.Core
{
    public class GenerateTokenRequest
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;

    }
    public class GenerateTokenResponse
    {
        public string Token { get; set; } =string.Empty;
        public DateTime TokenExpireDate { get; set; }
    }
}
