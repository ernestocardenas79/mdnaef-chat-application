using UNOSChat.ConversationAPI.DTOs;

namespace UNOSChat.ConversationAPI.Services.Interfaces;
public interface IConversationService
{
    Task<ConversationsDto> GetAllMessages(string id);
    Task<IEnumerable<ConversationPlaceHolderDto>> GetConversationPH();

    Task AddMessageToConversation(AddMessageDto message);
    Task<NewConversationResponseDto> CreateConversation(PostNewConversationDto newConversation);
}