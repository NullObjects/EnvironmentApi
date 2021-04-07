using EnvironmentApi.Controllers.AuthenticateService;
using EnvironmentApi.Models;
using EnvironmentApi.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authService;

        public AuthenticationController(IAuthenticateService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegistRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Request");

            //检查token权限
            var isAdmin = false;
            var user = _authService.ParsingClaims(HttpContext.User.Claims, out var roles);
            if (roles.Contains("admin"))
                isAdmin = true;
            //注册管理员
            if (request.Role.ToLower().Contains("admin") && !isAdmin)
                return BadRequest("非授权用户，无法注册");

            //用户注册
            if (_authService.AddUserData(request) is null)
                return BadRequest("注册失败，检查是否已注册或其他注册信息");
            return Ok("注册成功");
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "public")]
        public ActionResult Information()
        {
            var user = _authService.ParsingClaims(HttpContext.User.Claims, out var roles);
            //管理员组查看所有用户
            return roles.Contains("admin")
                ? new ObjectResult(_authService.GetUserData())
                : new ObjectResult(_authService.GetUserData(user));
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "public")]
        public ActionResult Modify(ModifyRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Request");

            //检查token权限
            var isAdmin = false;
            var user = _authService.ParsingClaims(HttpContext.User.Claims, out var roles);
            if (roles.Contains("admin"))
                isAdmin = true;
            if ((request.UserName != user || request.Role.ToLower().Contains("admin")) && !isAdmin)
                return BadRequest("非授权用户，无法修改");

            if (_authService.ModifyUserData(request, isAdmin) is null)
                return BadRequest("修改失败，检查修改信息");
            return Ok("修改成功");
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        [Authorize(Roles = "public")]
        public ActionResult Delete(string username)
        {
            var user = _authService.ParsingClaims(HttpContext.User.Claims, out var roles);
            if (username != user)
            {
                if (!roles.Contains("admin"))
                    return BadRequest("非授权用户，无法删除");
            }

            if (_authService.DeleteUserData(username) is null)
                return BadRequest("删除失败，检查待删除用户名");
            return Ok("删除成功");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Request");

            var token = _authService.Authenticated(request);
            if (token is null)
                return BadRequest("用户不存在或密码错误");
            return Ok(token);
        }

        /// <summary>
        /// 获取RSA公钥
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetKey()
        {
            return Ok(SecurityRsa.PublicKeyString);
        }
    }
}