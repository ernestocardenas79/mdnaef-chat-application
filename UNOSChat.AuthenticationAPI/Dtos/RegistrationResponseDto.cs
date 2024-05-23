namespace UNOSChat.AuthenticationAPI.Dtos;

public class RegistrationResponseDto: BaseResponse
{
    public string token { get; set; }
    public string email { get; set; }
}
