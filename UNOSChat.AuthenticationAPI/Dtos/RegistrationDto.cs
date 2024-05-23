using System.ComponentModel.DataAnnotations;

namespace UNOSChat.AuthenticationAPI.Dtos;

public class RegistrationDto
{
    public string? Name { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }
}

public class CreateChatUserDto
{
    public string Name { get; set; }

    public string User { get; set; }

}