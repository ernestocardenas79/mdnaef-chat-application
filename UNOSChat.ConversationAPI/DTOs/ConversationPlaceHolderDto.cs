using UNOSChat.ConversationAPI.Models;

namespace UNOSChat.ConversationAPI.DTOs;

public class ConversationPlaceHolderDto
{
    public ConversationPlaceHolderDto()
    {
        members = new List<Member>();
    }

    public string id { get; set; }

    public IEnumerable<Member> members { get; set; }
}
