using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UNOSChat.ConversationAPI.DTOs;
using UNOSChat.ConversationAPI.Services.Interfaces;

namespace UNOSChat.ConversationAPI.Controllers;
[Route("api/user")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDto userDto)
    {
        await userService.CreateUserAsync(userDto);
        return Ok();
    }

    [HttpPost("contacts")]
    public async Task<IActionResult> Contacts(CreateContactsDto contactsDto)
    {
        await userService.AddContacts(contactsDto);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put(UserDto user)
    {
        await userService.UpdateUserAsync(user);
        return Ok();
    }

    [HttpGet("{email}")]
    public async Task<IActionResult> Get([FromRoute] string email)
    {
        UserDto user = new() { user = email };
        
        var response = await userService.FindUserAsync(user);
        
        if(response == null)
        {
            return NotFound();
        }

        user.avatar = response.avatar;
        user.name = response.name;
        user.id = response._id.ToString();
        user.user = response.user;

        return Ok(user);
    }

    [HttpGet("all/{filter}")]
    public async Task<IActionResult> GetUsers([FromRoute] string filter)
    {

        var response = await userService.FindUsersByFilterAsync(filter);

        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
    }
}
