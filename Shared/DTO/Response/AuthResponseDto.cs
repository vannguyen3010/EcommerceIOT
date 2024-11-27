namespace Shared.DTO.Response
{
    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? RefreshTokens { get; set; }
        public string? Token { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
