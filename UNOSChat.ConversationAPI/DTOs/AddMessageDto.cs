namespace UNOSChat.ConversationAPI.DTOs;

public class AddMessageDto
{
    public string conversationId { get; set; }
    public string message { get; set; }
    public string member { get; set; }
}
