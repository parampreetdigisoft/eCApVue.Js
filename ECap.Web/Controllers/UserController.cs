using AutoMapper;
using ECap.Core.Domain.Interfaces;
using ECap.DTO;
using ECap.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ECap.Web.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly IMapper _mapper;
        private IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper=mapper;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ManageUser()
        {

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllManageUser(string filterBy, string searchBy)
        {
            try
            {
                var userList = await _userRepository.GetAllManageUserAsync();
                var userViewModel = _mapper.Map<List<ManageUserViewModel>>(userList);
                if (!string.IsNullOrEmpty(searchBy))
                {
                    if (filterBy == ManageUserFilter.Name.ToString())
                    {
                        userViewModel = userViewModel != null ? userViewModel.Where(x => x.User_FirstName.Contains(searchBy) || x.User_LastName.Contains(searchBy)).ToList() :
                          null;
                    }
                    else if (filterBy == ManageUserFilter.UserID.ToString())
                    {
                        userViewModel = userViewModel != null ? userViewModel.Where(x => x.User_Login.Contains(searchBy)
                        ).ToList() : null;
                    }
                    else if (filterBy == ManageUserFilter.Creation_Date.ToString())
                    {
                        userViewModel = userViewModel != null ? userViewModel.Where(x => x.LastTestCreationDate == searchBy).ToList() : null;
                    }
                    else if (filterBy == ManageUserFilter.Client_Group.ToString())
                    {
                        userViewModel = userViewModel != null ? userViewModel.Where(x => x.C_Name.Contains(searchBy)).ToList() : null;
                    }
                    else if (filterBy == ManageUserFilter.Group_Admin.ToString())
                    {
                        userViewModel = userViewModel != null ? userViewModel.Where(x => x.User_AdminRole == "CA").ToList() : null;
                    }
                    else if (filterBy == ManageUserFilter.User_No.ToString())
                    {
                        int number;
                        bool success = int.TryParse(searchBy, out number);
                        if (success)
                            userViewModel = userViewModel != null ? userViewModel.Where(x => x.UserId == number).ToList() : null;
                        else
                            userViewModel = null;
                    }
                }
                return Ok(userViewModel == null ? new List<ManageUserViewModel>(): userViewModel);
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        [HttpPost]
        public IActionResult SaveEmails(string[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach(var i in ids)
                {
                    var childArray = i;
                }
                
            }
            return View();
        }
        [HttpPost]
        public IActionResult DeleteUser(string[] ids)
        {
            bool status = false;
            if (ids != null && ids.Length > 0)
            {
                foreach (var userId in ids)
                {
                    _userRepository.DeleteUserAsync(Convert.ToInt32(userId));
                }
                status = true;
            }
            return Ok(new JsonResponse { Status = status, Message = MessageConstants.DELETED_SUCCESSFULLY });
        }
        [HttpGet]
        public async Task<IActionResult> GetManageUserDetails(string userId)
        {
            UserInfoViewModel userInfoViewModel = new UserInfoViewModel();
            
                if (!string.IsNullOrEmpty(userId))
                {
                    var userInfo = await _userRepository.GetManageUserDetailAsync(Convert.ToInt32(userId));
                    userInfoViewModel = _mapper.Map<UserInfoViewModel>(userInfo);
                }

            return Ok(userInfoViewModel);
        }
            
        [HttpGet]
        public IActionResult Registration()
        {

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetUserList()
        {
            var userList = await _userRepository.GetAllManageUserAsync();
            var userViewModel = _mapper.Map<List<ManageUserViewModel>>(userList);
            return Ok(userViewModel);
        }
        [HttpGet]
        public IActionResult MemberHome(string language)
        {
            return View();
        }
        public async Task<IActionResult> MarkTestComplete(int purchaseId, int UserId)
        {
            bool userList =  await _userRepository.MarkTestComplete(purchaseId,UserId);
            return Ok(userList);
        }
        
    }
}
