using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult >Index()
        {
            return View(await _userService.GetUser());
        }

    }
}
