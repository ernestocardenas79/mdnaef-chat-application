using UNOSChat.ConversationAPI.Models;

namespace UNOSChat.ConversationAPI.DTOs;

public class MessageDto
{
    public string message { get; set; }
    public string member { get; set; }

    // Operador de conversión personalizado para convertir una colección de Message a una colección de MessageDto
    public static IEnumerable<MessageDto> ConvertFromMessage(IEnumerable<Message> messages)
    {
        return messages.Select(message =>
            new MessageDto
            {
                message = message.message,
                member = message.member.ToString(),
            });
    }
}
