using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using static UNOSChat.ConversationAPI.Controllers.ConversationController;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using UNOSChat.ConversationAPI.Data;
using UNOSChat.ConversationAPI.Models;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Host = UNOSChat.ConversationAPI.Models.ConversationHost;
using Microsoft.EntityFrameworkCore;
using UNOSChat.ConversationAPI.Services.Interfaces;
using UNOSChat.ConversationAPI.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace UNOSChat.ConversationAPI.Controllers;

[ApiController]
[Route("api/conversation")]
[Authorize]
public class ConversationController : ControllerBase
{
    private readonly UNOSChatDbContext _context;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IConversationService _conversationService;


    public ConversationController(UNOSChatDbContext context
        , IHttpContextAccessor httpContext,IConversationService conversationService)
    {
        _context = context;
        this._httpContext = httpContext;
        _conversationService = conversationService;
    }

    [HttpGet("{id}")]
    [SwaggerResponse(200, "Succesfully response", typeof(ConversationsDto))]
    public async Task<IActionResult> GetAllMessages([FromRoute] string id)
    {
        var messages = await _conversationService.GetAllMessages(id);
        return Ok(messages);
    }

    [HttpGet]
    [Route("ph")]
    [SwaggerResponse(200, "Succesfully response", typeof(ConversationPlaceHolderDto))]
    public async Task<IActionResult> GetConversationPH()
    {
        var messages = await _conversationService.GetConversationPH();

        return Ok(messages);
    }

    [HttpPost]
    [Route("message")]
    public async Task<IActionResult> AddMessageToConversarion(AddMessageDto message)
    {
        await _conversationService.AddMessageToConversation(message);
        return NoContent();
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateConversarion([FromBody]PostNewConversationDto newConversation)
    {
        var conversation = await _conversationService.CreateConversation(newConversation);
        return CreatedAtAction(nameof(GetAllMessages), new { id= conversation.ConversationId }, conversation);
    }
    

    //[HttpGet]
    //public async Task<IActionResult> Get()
    //{
    //    //var client = new MongoClient(@"mongodb://mongoadmin:LikeAnd@chatdb:27017");
    //    //var db = UNOSChatDbContext.Create(client.GetDatabase("UNOSChat"));
    //    //var users = await db.Users.FirstOrDefaultAsync();
    //    //Console.WriteLine(movie);

    //    //IMongoCollection<User> _usersCollection;
    //    //IMongoCollection<ConversationDocument> _conversationDocument;

    //    //MongoClient client = new MongoClient(@"mongodb://mongoadmin:LikeAnd@chatdb:27017");
    //    //IMongoDatabase database = client.GetDatabase("UNOSChat");

    //    //_usersCollection = database.GetCollection<User>("users");
    //    //var users = await _usersCollection.Find(new BsonDocument()).ToListAsync();

    //    //_conversationDocument = database.GetCollection<ConversationDocument>("Conversations");
    //    //var users = await _conversationDocument.Find(new BsonDocument()).ToListAsync();

    //    //await _context.Users.AddAsync(new Models.User()
    //    //{
    //    //    user = 2,
    //    //    name = "Mario",
    //    //    avatar = @"https://i.pravatar.cc/150?u=2Mario",
    //    //    contacts = new List<Models.Contact>()
    //    //});

    //    //await _context.SaveChangesAsync();

    //    //var user = await _context.Users.FirstOrDefaultAsync(u => u.user == 1);
    //    //user.contacts = new List<Contact>() { 
    //    //    { new Contact 
    //    //        { 
    //    //            avatar = @"https://i.pravatar.cc/40?u=2Mario",
    //    //            user =2,
    //    //            name="Mario"
    //    //        } 
    //    //    }
    //    //};
    //    //var user2 = await _context.Users.FirstOrDefaultAsync(u => u.user == 2);
    //    //user2.contacts = new List<Contact>() {
    //    //    { new Contact
    //    //        {
    //    //            avatar = @"https://i.pravatar.cc/40?u=1Ernesto",
    //    //            user =1,
    //    //            name="Ernesto"
    //    //        } 
    //    //    }
    //    //};

    //    //await _context.SaveChangesAsync();

    //    //var x = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

    //    var users = await _context.Conversations.AsNoTracking()
    //    //.Select(s => new ConversationDocument() { _id = s._id, host = new Host(s.host.user, s.host.name, s.host.avatar) })
    //    //.Select(s => new Conversations() { _id = s._id, host = s.host })
    //    .FirstOrDefaultAsync();

    //    //var users = await _context.ConversationsPH.AsNoTracking()
    //    ////.Select(s => new ConversationDocument() { _id = s._id, host = new Host(s.host.user, s.host.name, s.host.avatar) })
    //    ////.Select(s => new Conversations() { _id = s._id, host = s.host })
    //    //.FirstOrDefaultAsync();

    //    //var users = (from c in _context.Conversations.AsNoTracking()
    //    //             select new ConversationDocument() { _id = c._id, host = new ConversationHost(c.host.user, c.host.name, c.host.avatar) }
    //    //             );

    //    //var users = await _context.Users.ToListA sync();
    //    //var users = await _context.Users.FirstOrDefaultAsync();
    //    //var users = await _context.Users.FirstOrDefaultAsync(u => u.user == 2);
    //    return Ok(users);
    //}
}
