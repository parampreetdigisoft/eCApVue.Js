using ECap.Core.Domain.Interfaces;
using ECap.Core.Entities;
using ECap.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECap.Web.Helper
{
    public class LoginHelper
    {
        private readonly IUserRepository _userRepository;

        private readonly string errIdDisabled = "User ID you enter is already disabled. To take the E-CAP, you'll need to apply again.";
        private readonly string errAuthFail = "User ID or password is incorrect. Please copy this information exactly from your confirmation email.";
        private readonly string errAuthFailjp = "ユーザーIDまたはパスワードが間違っています。 お送りしたメールを確認して再度入力してください。";
        private readonly string errAuthFailkr = "사용자 ID나 비밀번호가 일치하지 않습니다. 확인메일로 수신하신 로그인 정보로 정확히 입력하시기 바랍니다.";

        public LoginHelper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

       
        /// <summary>
        /// Validate Login 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="model"></param>
        /// <param name="message"></param>
        /// <param name="redirectURL"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<LoginResponse> ValidateLoginAsync(HttpContext httpContext, LoginViewModel model)
        {
            httpContext.RemoveCookies("Admin");
            httpContext.RemoveCookies("UserStatus");

            LoginResponse loginResponse = new LoginResponse { Message = MessageConstants.ERROR_MESSAGE };

            //English default language
            loginResponse.Languge = LanguageConstants.ENGLISH;

            // TODO: login logic. . .
            if (httpContext.Request.Query.Count > 0 && !string.IsNullOrEmpty(httpContext.Request.Query["lang"]))
                loginResponse.Languge = httpContext.Request.Query["lang"];

            // db calls. 
            var result = await _userRepository.ValidateLoginAsync(new User { Password = model.Password, UserName = model.UserName });

            if (result != null)
            {
                //Check for result.
                var validStatusCode = result.Item1;

                if (validStatusCode == (int)LoginStatusCode.notvalid || validStatusCode == (int)LoginStatusCode.notfound)
                {
                    loginResponse.Status = false;
                    if (loginResponse.Languge.ToLower() == LanguageConstants.JAPAN)
                    {
                        loginResponse.Message = errAuthFailjp;
                    }
                    else if (loginResponse.Languge.ToLower() == LanguageConstants.KOREAN)
                    {
                        loginResponse.Message = errAuthFailkr;
                    }
                    else
                    {
                        loginResponse.Message = errAuthFail;
                    }
                }
                else
                {
                    if (validStatusCode == (int)LoginStatusCode.valid)
                    {
                        loginResponse.Status = true;
                        //Get user entity 
                        var userEntity = (User)result.Item2;
                        if (userEntity?.User_Login.ToLower() == UserRole.admin.ToString())
                        {
                            httpContext.SetCookies(CookieConstants.ADMIN, CookieConstants.ADMIN, null);
                            httpContext.SetCookies(CookieConstants.USER_STATUS, "True", null);
                            loginResponse.CallBackUrl = await RedirectAdminAsync(httpContext, userEntity, model.ReturnUrl);
                        }
                        else if (userEntity?.User_AdminRole.Trim().ToLower() == "ca")
                        {
                            httpContext.SetCookies(CookieConstants.GROUP_ADMIN, CookieConstants.GROUP_ADMIN, null);
                            httpContext.SetCookies(CookieConstants.USER_STATUS, "True", null);
                            loginResponse.CallBackUrl = await RedirectAdminAsync(httpContext, userEntity, model.ReturnUrl);
                        }
                        else
                        {
                            httpContext.SetCookies(CookieConstants.USER, CookieConstants.USER, null);
                            httpContext.SetCookies(CookieConstants.USER_STATUS, userEntity != null ? userEntity.UserLogin_Flag.ToString() : "False", null);
                            loginResponse.CallBackUrl = await RedirectUserAsync(httpContext, userEntity, result.Item3, result.Item4, model.ReturnUrl);
                        }
                    }
                }

                // not handled by sp currently.
                /*if (validStatusCode == (int)LoginStatusCode.restrictedUser)
                {
                }*/
            }
            return loginResponse;
        }

        /// <summary>
        /// Get User Redirection. . .
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="data"></param>
        /// <param name="IsExpire"></param>
        /// <param name="IsExpireScore"></param>
        /// <returns></returns>
        private async Task<string> RedirectUserAsync(HttpContext httpContext, User data, string? IsExpire, string? IsExpireScore, string? returnUrl)
        {
            try
            {
                string lang = LanguageConstants.ENGLISH;
                if (!string.IsNullOrEmpty(data.UserName))
                    // HttpContext.Session.SetString("member", data.UserName);
                    httpContext.Session.SetString(SessionConstants.MEMBER, data.UserName);

                else if (data.UserId != 0)
                {
                    httpContext.Session.SetInt32(SessionConstants.USER_ID, data.UserId);
                    httpContext.Session.SetInt32(SessionConstants.GROUP_USER_ID, data.UserId);
                }
                int UID = Convert.ToInt32(data.UserId);

                var userLanguage = await _userRepository.GetUserLangAsync(Convert.ToInt32(data.UserId));
                if (userLanguage != null)
                {
                    lang = userLanguage.ToString();
                }

                if (lang == "J")
                    lang = LanguageConstants.JAPAN;
                if (lang == "E")
                    lang = LanguageConstants.ENGLISH;

                httpContext.SetCookies(CookieConstants.SESSION_COOKIE, data.UserId.ToString(), null);

                if (!string.IsNullOrEmpty(IsExpire))
                {
                    httpContext.SetCookies(CookieConstants.IS_EXPIRE, IsExpire, null);
                }
                else
                {
                    httpContext.SetCookies(CookieConstants.IS_EXPIRE, "", null);
                }

                if (!string.IsNullOrEmpty(IsExpireScore))
                {
                    httpContext.SetCookies(CookieConstants.IS_EXPIEE_SCORE, IsExpireScore, null);
                }
                else
                {
                    httpContext.SetCookies(CookieConstants.IS_EXPIEE_SCORE, "", null);
                }

                httpContext.SetCookies(CookieConstants.CULTURE, lang, null);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return returnUrl;
                }
                else
                {
                    if (httpContext.Request.Query["action"] == "viewer")
                    {
                        if (!string.IsNullOrEmpty(httpContext.Request.Query["doc"]))
                        {
                            //Response.Redirect(string.Format("DocumentViewer.aspx?doc={0}", HttpContext.Request.Query["doc"]));
                            returnUrl = string.Format("/profile/AdminOnly?doc={0}", httpContext.Request.Query["doc"]);
                        }
                        else
                        {
                            //Response.Redirect("DocumentViewer.aspx");
                            returnUrl = "/profile/AdminOnly";
                        }
                    }
                    else if (!string.IsNullOrEmpty(httpContext.Request.Query["lang"]))
                    {
                        //Response.Redirect(string.Format("MemberHome.aspx?lang={0}", HttpContext.Request.Query["lang"]));
                        returnUrl = string.Format("/profile/AdminOnly?lang={0}", httpContext.Request.Query["lang"]);
                    }
                    else
                    {
                        //Response.Redirect("MemberHome.aspx?lang=" + lang);
                        returnUrl = string.Format("/profile/AdminOnly?lang={0}", lang);
                    }
                }
                return returnUrl;
            }

            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Admin Redirection . . .
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<string> RedirectAdminAsync(HttpContext httpContext, User user, string? returnUrl)
        {
            if (!string.IsNullOrEmpty(user.UserName))
            {
                httpContext.Session.SetString("member", user.UserName);
            }
            else if (user.UserId != 0)
            {
                httpContext.Session.SetInt32(SessionConstants.USER_ID, user.UserId);
                httpContext.Session.SetInt32(SessionConstants.GROUP_USER_ID, user.UserId);
            }

            // set cookies with value N . . .
            httpContext.SetCookies(CookieConstants.IS_EXPIRE, "N", null);
            httpContext.SetCookies(CookieConstants.IS_EXPIEE_SCORE, "N", null);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return returnUrl;
            }
            else
            {
                string action = Convert.ToString(user.User_AdminRole);
                //check if we are getting action QS. .
                if (!string.IsNullOrEmpty(httpContext.Request.Query["action"]))
                {
                    action = httpContext.Request.Query["action"];
                }
                switch (action)
                {
                    case "upload":
                        //Response.Redirect("Admin/UploadDocument.aspx");
                        returnUrl = "Admin/Index";
                        break;
                    case "CA":
                        var i = await _userRepository.CheckAdminAsync(Convert.ToInt32(user.UserId));
                        if (i > 0)
                        {
                            // "CA/Index";
                            returnUrl = "/profile/AdminOnly";
                        }
                        else
                        {
                            // Response.Redirect("UserCheck.aspx");
                            returnUrl = "/profile/AdminOnly";
                        }
                        break;
                    default:
                        //Response.Redirect("Admin/AdminHome.aspx");
                        returnUrl = "/Admin/Index";
                        break;
                }
            }
        
            return returnUrl;
        }
    }
}
