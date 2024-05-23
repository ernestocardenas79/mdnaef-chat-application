using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using UNOSChat.ConversationAPI.Models;

namespace UNOSChat.ConversationAPI.Data;

public class UNOSChatDbContext : DbContext
{
    public DbSet<User> Users { get; init; }
    public DbSet<Conversations> Conversations { get; init; }
    public DbSet<ConversationsPH> ConversationsPH { get; init; }
    
    internal static UNOSChatDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<UNOSChatDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);
    public UNOSChatDbContext(DbContextOptions options)
        : base(options)
    {
        //BsonSerializer.RegisterSerializer(typeof(IList<string>), new ListStringSerializer());
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>();
        modelBuilder.Entity<Conversations>();
        modelBuilder.Entity<ConversationsPH>();
        
    }
}
