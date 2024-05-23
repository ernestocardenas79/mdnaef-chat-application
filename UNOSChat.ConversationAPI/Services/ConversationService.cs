using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Security.Claims;
using UNOSChat.ConversationAPI.Data;
using UNOSChat.ConversationAPI.DTOs;
using UNOSChat.ConversationAPI.Models;
using UNOSChat.ConversationAPI.Services.Interfaces;

namespace UNOSChat.ConversationAPI.Services;

public class ConversationService : IConversationService
{
    private readonly UNOSChatDbContext _context;
    private readonly IHttpContextAccessor _httpContext;

    private string userEmail{ get=> _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);}

    public ConversationService(UNOSChatDbContext context, IHttpContextAccessor httpContext)
    {
        _context = context;
        _httpContext = httpContext;
    }

    public async Task<IEnumerable<ConversationPlaceHolderDto>> GetConversationPH()
    {
        var conversation = new List<ConversationPlaceHolderDto>();

        var conversationsCollection = await _context.ConversationsPH.AsNoTracking()
                                .Where(c => c.emailsMembers.Contains(userEmail))
                                .ToListAsync();

        if (conversationsCollection != null)
        {
            foreach (var conversationPh in conversationsCollection)
            {
                conversation.Add(new() { id = conversationPh._id.ToString(), 
                                         members = conversationPh.members 
                });
            }
        }

        return conversation;
    }

    public async Task<ConversationsDto> GetAllMessages(string id)
    {
        ConversationsDto conversations = new();

        var conversationsCollection = await _context.Conversations.AsNoTracking()
                                .Where(c=> c.conversationId.ToString() == id)
                                .FirstOrDefaultAsync();

        if (conversationsCollection != null)
        {
            conversations.id = conversationsCollection._id.ToString();
            conversations.conversationId = conversationsCollection.conversationId.ToString();
            conversations.messages = MessageDto.ConvertFromMessage( conversationsCollection.messages);
        }

        return conversations;
    }

    public async Task AddMessageToConversation(AddMessageDto message)
    {
        ConversationsDto conversations = new();

        var conversation = await _context.Conversations
                                .Where(c => c.conversationId.ToString() == message.conversationId)
                                .FirstOrDefaultAsync();

        if(conversation != null)
        {
            conversation.messages.Add(new() { member = new ObjectId(message.member), message = message.message });
            await _context.SaveChangesAsync();
        }
    }

    public async Task<NewConversationResponseDto> CreateConversation(PostNewConversationDto newConversation)
    {
        var members = new List<Member>() { new Member() { id= newConversation.User.id, user = newConversation.User.user, name = newConversation.User.name, avatar = newConversation.User.avatar ?? "" },
            new Member() { id= newConversation.HostUser.id,user = newConversation.HostUser.user, name = newConversation.HostUser.name, avatar = newConversation.HostUser.avatar ?? "" },
        };

        ConversationsPH conversationPh = new() { members= members , emailsMembers= new string[] { newConversation.User.user , newConversation.HostUser.user } };

        conversationPh._id = ObjectId.GenerateNewId();
        await _context.ConversationsPH.AddAsync(conversationPh);

        Conversations conversation= new() { conversationId = conversationPh._id, messages = new List<Message> { new() { member = new ObjectId(newConversation.HostUser.id), message = newConversation.Message } } };
        await _context.Conversations.AddAsync(conversation);

        await _context.SaveChangesAsync();

        return new() { ConversationId = conversationPh._id.ToString(), HostUser = newConversation.HostUser, User = newConversation.User };
    }
}
