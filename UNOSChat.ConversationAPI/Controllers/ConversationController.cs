using Microsoft.AspNetCore.Mvc;

namespace UNOSChat.ConversationAPI.Controllers;
public class ConversationController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
