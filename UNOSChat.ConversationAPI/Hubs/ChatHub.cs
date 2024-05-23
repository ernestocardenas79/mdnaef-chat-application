using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using UNOSChat.ConversationAPI.DTOs;
using UNOSChat.ConversationAPI.Models;

namespace UNOSChat.ConversationAPI.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task UserConnected(string email)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, email);
    }

    public async Task ConnectConversation(string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
    }

    public async Task ConversationCreated(NewConversationResponseDto newConversation)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, newConversation.ConversationId);

        await Clients.Group(newConversation.User.user).SendAsync("NewConversationPH", newConversation);
    }

    public async Task SendMessage(string room, UserDto user, string message)
    {
        await Clients.Group(room).SendAsync("ReceiveMessage", user, message);
    }

    public override Task OnConnectedAsync()
    {
        base.OnConnectedAsync();

        return  Task.CompletedTask;
    }
}
