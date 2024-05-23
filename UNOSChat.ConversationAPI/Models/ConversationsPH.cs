using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.EntityFrameworkCore;
using UNOSChat.ConversationAPI.Data;

namespace UNOSChat.ConversationAPI.Models;

[Collection("conversationsPH")]

public class ConversationsPH
{
    public ConversationsPH()
    {
        members = new List<Member>();
        //emailsMembers= new List<string>();
    }

    [BsonId]
    public ObjectId _id { get; set; }
    
    public IEnumerable<Member> members { get; set; }

    public string[]? emailsMembers { get; set; }
}

