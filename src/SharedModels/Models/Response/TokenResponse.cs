namespace SharedModels.Models.Response;

public class TokenResponse
{

    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public DateTime ExpirationDate { get; set; }
    public TokenResponse(string token, string refreshToken, string tokenType, DateTime expirationDate)
    {
        Token = token;
        RefreshToken = refreshToken;
        TokenType = tokenType;
        ExpirationDate = expirationDate;
    }
}
