using EnvironmentApi.Models;
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
            this._authService = authService;
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

            //注册管理员
            if (request.Role.ToLower().Contains("admin"))
            {
                _authService.ParsingClaims(HttpContext.User.Claims, out var roles);
                if (!roles.Contains("admin"))
                    return BadRequest("非授权用户，无法注册管理员");
            }

            //用户注册
            if (_authService.AddUserData(request) is null)
                return BadRequest("用户已存在，请直接登录");
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

            var user = _authService.ParsingClaims(HttpContext.User.Claims, out var roles);
            if (request.UserName != user)
            {
                if (!roles.Contains("admin"))
                    return BadRequest("非授权用户，无法修改其他用户信息");
            }

            if (_authService.ModifyUserData(request) is null)
                return BadRequest("修改失败");
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
                    return BadRequest("非授权用户，无法删除其他用户信息");
            }

            if (_authService.DeleteUserData(username) is null)
                return BadRequest("删除失败");
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