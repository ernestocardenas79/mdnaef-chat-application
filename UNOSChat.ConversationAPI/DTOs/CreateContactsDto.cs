using UNOSChat.ConversationAPI.Models;

namespace UNOSChat.ConversationAPI.DTOs;

public class CreateContactsDto : UserDto { 
    public IEnumerable<Member> contacts {  get; set; }
}
