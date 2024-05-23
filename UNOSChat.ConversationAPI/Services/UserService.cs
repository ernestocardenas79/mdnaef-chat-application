using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using UNOSChat.ConversationAPI.Data;
using UNOSChat.ConversationAPI.DTOs;
using UNOSChat.ConversationAPI.Models;
using UNOSChat.ConversationAPI.Services.Interfaces;

namespace UNOSChat.ConversationAPI.Services;

public class UserService : IUserService
{
    private readonly UNOSChatDbContext _context;

    public UserService(UNOSChatDbContext context)
    {
        _context = context;
    }

    public async Task CreateUserAsync(CreateUserDto userDto)
    {
        var user = new User()
        {
           avatar = @$"https://i.pravatar.cc/40?u={userDto.user}{userDto.name}",
           name = userDto.name,
           user = userDto.user,
           contacts = new List<Member>()
        };
        var existingUser = await FindUserAsync(userDto);

        if (existingUser == null)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return;
        }
    }
    public async Task AddContacts(CreateContactsDto contactsDto)
    {
        var user = await FindUserAsync(contactsDto);
       
        if (user != null)
        {
            List<Member> updatedContacts = new List<Member>();

            if (user.contacts.Any())
            {
                updatedContacts = user.contacts.ToList();
            }

            updatedContacts.AddRange(contactsDto.contacts);

            user.contacts = updatedContacts;

            await _context.SaveChangesAsync();

            return;
        }
    }

    public async Task UpdateUserAsync(UserDto userDto)
    {
        var user = await FindUserAsync(userDto);
        
        if (user != null)
        {
            user.name = userDto.name;
            user.avatar = userDto.avatar;

            await _context.SaveChangesAsync();

            return;
        }
    }

    public Task<User?> FindUserAsync(UserDto user)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.user == user.user);
    }

    public async Task<IEnumerable<UserDto?>> FindUsersByFilterAsync(string filter)
    {
        IEnumerable<UserDto?> result = new List<UserDto?>();
        var users = await _context.Users.Where(u => u.name.ToLower().Contains(filter.ToLower())).ToListAsync();

        result = users.Select(u => new UserDto() { avatar = u.avatar, id = u._id.ToString(), name = u.name, user = u.user });

        return result;
    }
}
