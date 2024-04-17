using Microsoft.AspNetCore.Mvc;

namespace UNOSChat.AuthenticationAPI.Controllers;
public class AuthenticationController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
