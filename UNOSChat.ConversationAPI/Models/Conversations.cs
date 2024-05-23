using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace UNOSChat.ConversationAPI.Models;

[Collection("conversations")]

public class Conversations
{
    public Conversations()
    {
        messages = new List<Message>();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId _id { get; set; }

    public ObjectId conversationId { get; set; }

    public IList<Message> messages { get; set; }
}
