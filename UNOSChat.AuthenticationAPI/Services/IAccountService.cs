using UNOSChat.AuthenticationAPI.Dtos;

namespace UNOSChat.AuthenticationAPI.Services;
public interface IAccountService
{
    Task<AuthResponseDto> Login(UserAuthenticationDto userAuthentication);
    Task<RegistrationResponseDto> Register(RegistrationDto registration);
}

public class AccountServiceOptions
{
    public string ChatUrl { get; set; }
}