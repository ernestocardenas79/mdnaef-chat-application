using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace UNOSChat.ConversationAPI.Models;

[Collection("users")]
public class User
{
    public User()
    {
        contacts = new List<Member>();
    }
    public ObjectId _id { get; set; }
    public string user { get; set; }
    public string name { get; set; }
    public string? avatar { get; set; }

    public IEnumerable<Member>? contacts { get; set; }

}
