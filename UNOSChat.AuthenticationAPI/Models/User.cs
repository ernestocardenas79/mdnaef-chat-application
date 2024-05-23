using Microsoft.AspNetCore.Identity;

namespace UNOSChat.AuthenticationAPI.Models;

public class User: IdentityUser
{
    public string Name { get; set; }
}
