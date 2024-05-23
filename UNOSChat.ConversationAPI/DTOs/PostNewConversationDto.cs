namespace UNOSChat.ConversationAPI.DTOs;

public class PostNewConversationDto {
    public UserDto User { get; set; }
    public UserDto HostUser { get; set; }
    public string Message { get; set; }
}
