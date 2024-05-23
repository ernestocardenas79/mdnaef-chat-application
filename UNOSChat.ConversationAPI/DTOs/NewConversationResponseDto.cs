namespace UNOSChat.ConversationAPI.DTOs;

public class NewConversationResponseDto
{
    public UserDto User { get; set; }
    public UserDto HostUser { get; set; }
    public string ConversationId { get; set; }

}
