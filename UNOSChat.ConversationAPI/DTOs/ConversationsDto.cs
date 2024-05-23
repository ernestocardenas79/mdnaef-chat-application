namespace UNOSChat.ConversationAPI.DTOs;

public class ConversationsDto
{
    public string id { get; set; }

    public string conversationId { get; set; }

    public IEnumerable<MessageDto> messages { get; set; }
}