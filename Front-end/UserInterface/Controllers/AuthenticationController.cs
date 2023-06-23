using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.MicroserviceCommunication.Concrete;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IIdentityServiceComm _identityService;
        

        public AuthenticationController(IIdentityServiceComm identityService)
        {
           _identityService = identityService;
        }

        public IActionResult SigningIn()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult >SigningIn(SignInCredentials signInCredentials)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }
            var response = await _identityService.SignIn(signInCredentials);
            if (!response.IsSuccessful)
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError(String.Empty, x);

                });
                return View();
            }
            return RedirectToAction(nameof(Index), "Home");
            
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _identityService.RevokeRefreshToken();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult >FrontSignUp()
        {
            return View();
        }
        public async Task<IActionResult> FrontSignIn()
        {
            return View();
        }
        public async Task<IActionResult> SignUp(UserSignupModel userSignUpModel)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _identityService.SignUp(userSignUpModel);
            if (response.IsSuccessful)
            {

                return RedirectToAction(nameof(AuthenticationController.SignIn));
            }
            else
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError(String.Empty, x);

                });
                return View();
            }
            

        }
    }
}
