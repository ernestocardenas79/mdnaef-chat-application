namespace UNOSChat.AuthenticationAPI.Dtos;

public class AuthResponseDto:BaseResponse
{
    public string? Token { get; set; }
    public string email { get; set; }
}