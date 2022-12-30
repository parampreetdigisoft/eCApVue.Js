using ECap.Core.Domain.Interfaces;
using ECap.Core.Entities;
using ECap.Web.Helper;
using ECap.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace ECap.Web.Controllers
{
    public class ProfileController : BaseController
    {
        private IUserRepository _userRepository;
        private readonly ILogger<ProfileController> _logger;
        public ProfileController(IUserRepository userRepository, ILogger<ProfileController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            LoginResponse loginResponse = null;
            if (!ModelState.IsValid)
            {
                loginResponse = new LoginResponse { Status = false, ModalStateError = ModelState.ErrorsToJsonResult() };
                return Ok(loginResponse);
            }

            //login helper to get redirection. . .
            LoginHelper loginHelper = new LoginHelper(_userRepository);

            //validate  & process login request. 
            loginResponse = await loginHelper.ValidateLoginAsync(HttpContext, model);
            return Ok(loginResponse);
        }

        [HttpGet]
        public IActionResult AdminOnly()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture)
        {
            //English default language 
            string language = string.IsNullOrEmpty(culture)? language = "en": culture; 
            HttpContext.SetCookies(CookieConstants.CULTURE, language, null);
            return Ok(culture); 
        }

       

    }
}
