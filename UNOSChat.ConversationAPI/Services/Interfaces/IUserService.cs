using UNOSChat.ConversationAPI.DTOs;
using UNOSChat.ConversationAPI.Models;

namespace UNOSChat.ConversationAPI.Services.Interfaces;
public interface IUserService
{
    Task CreateUserAsync(CreateUserDto userDto);
    Task<User?> FindUserAsync(UserDto user);
    Task AddContacts(CreateContactsDto contacts);
    Task UpdateUserAsync(UserDto userDto);
    Task<IEnumerable<UserDto?>> FindUsersByFilterAsync(string filter);
}