using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UNOSChat.AuthenticationAPI.Dtos;
using UNOSChat.AuthenticationAPI.Services;

namespace UNOSChat.AuthenticationAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtHandler _jwtHandler;
    private readonly IAccountService _accountService;

    public AccountController(UserManager<IdentityUser> userManager, JwtHandler jwtHandler, IAccountService accountService)
    {
        _userManager = userManager;
        _jwtHandler = jwtHandler;
        _accountService = accountService;
    }

    [HttpPost("Registration")]
    [SwaggerResponse(200, "User Created", typeof(RegistrationResponseDto))]
    public async Task<IActionResult> RegisterUser([FromBody] RegistrationDto registration)
    {
        if (registration == null || !ModelState.IsValid)
            return BadRequest();

        var response = await _accountService.Register(registration);
        //RegistrationResponseDto response = new() { email= "termo2@con.com", HttpCode =201, IsSuccess=true };

        IActionResult actionResult = response.HttpCode switch
        {
            400 => BadRequest(response),
            _ => StatusCode(response.HttpCode, response)
        };
        
        return actionResult;
    }

    [HttpPost("Login")]
    [SwaggerResponse(200, "Succesfully response", typeof(AuthResponseDto))]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] UserAuthenticationDto userAuthentication)
    {
        var response = await _accountService.Login(userAuthentication);

        ActionResult actionResult = response.HttpCode switch
        {
            401 => Unauthorized(response),
            _ => StatusCode(response.HttpCode, response)
        };

        return actionResult;
    }
}
