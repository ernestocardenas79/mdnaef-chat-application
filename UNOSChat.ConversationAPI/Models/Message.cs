using MongoDB.Bson;

namespace UNOSChat.ConversationAPI.Models;

public class Message
{
    public string message { get; set; }
    public ObjectId member { get; set; }
}