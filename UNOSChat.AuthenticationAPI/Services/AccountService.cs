using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using UNOSChat.AuthenticationAPI.Dtos;
using UNOSChat.AuthenticationAPI.Models;

namespace UNOSChat.AuthenticationAPI.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtHandler _jwtHandler;
    private readonly AccountServiceOptions _acountServiceOptions;
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountService(UserManager<IdentityUser> userManager, IHttpClientFactory httpClientFactory, JwtHandler jwtHandler, IOptions<AccountServiceOptions> configuredOptions)
    {
        _userManager = userManager;
        _httpClientFactory = httpClientFactory;
        _jwtHandler = jwtHandler;
        _acountServiceOptions = new();
        _acountServiceOptions = configuredOptions.Value;
    }

    public async Task<RegistrationResponseDto> Register(RegistrationDto registration)
    {
        User user = new()
        {
            Email = registration.Email,
            UserName = registration.Name
        };

        var result = await _userManager.CreateAsync(user, registration.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);

            return new RegistrationResponseDto { HttpCode = 400, Errors = errors, IsSuccess = false };
        }

        var token = GenerateToken(user);
        await RegisterUserOnChatApp(user, token);

        return new RegistrationResponseDto() { HttpCode = 201, token= token, email = registration.Email };
    }

    public async Task<AuthResponseDto> Login(UserAuthenticationDto userAuthentication)
    {
        var user = await _userManager.FindByNameAsync(userAuthentication.UserName);

        if (user == null || !await _userManager.CheckPasswordAsync(user, userAuthentication.Password))
            return new AuthResponseDto { HttpCode = 401, Errors = new List<string> { "Invalid Authentication" }, IsSuccess = false };

        var token = GenerateToken(user);

        return new AuthResponseDto { IsSuccess = true, Token = token, email = user.Email };
    }

    private Task RegisterUserOnChatApp(User user, string token)
    {
        CreateChatUserDto chatUser = new() { Name = user.UserName, User= user.Email };

        HttpClient client = _httpClientFactory.CreateClient("UNOSChatApp");

        HttpRequestMessage message = new();
        message.Headers.Add("Accept", "application/json");
        message.Method = HttpMethod.Post;
        message.RequestUri = new Uri(_acountServiceOptions.ChatUrl);

        message.Headers.Add("Authorization", $"Bearer {token}");

        message.Content = new StringContent(JsonConvert.SerializeObject(chatUser), Encoding.UTF8, "application/json");

        return client.SendAsync(message);
     }

    private string GenerateToken(IdentityUser user)
    {
        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var claims = _jwtHandler.GetClaims(user);
        var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
       
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}
