using Microsoft.AspNetCore.Mvc;

namespace UNOSChat.UsersManagmentAPI.Controllers;
public class UserController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
