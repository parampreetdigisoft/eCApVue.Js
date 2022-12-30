using ECap.Core.Domain.Interfaces;
using ECap.DTO;
using ECap.Web.Helper;
using ECap.Web.Models;

using Microsoft.AspNetCore.Mvc;

using System.Data;

namespace ECap.Web.Controllers
{
    public class AdminController : BaseAdminController
    {
        private IUserRepository _userRepository;
        public AdminController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult>ChangePassword(ChangePasswordViewModel changePasswmiordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new JsonResponse { Status = false, ModalStateError = ModelState.ErrorsToJsonResult() });
            }
            else
            {
                var userId = HttpContext.Session.GetInt32(SessionConstants.USER_ID);
                await _userRepository.ChangePassword(changePasswmiordViewModel.Password, Convert.ToInt32(userId));
                return Ok(new JsonResponse { Status = true, Message = MessageConstants.SUCCESS_MESSAGE });
            }
        }
    }
}
