using EnvironmentApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
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

            if (!_authService.IsRegisted(request))
                return BadRequest("用户已存在，请直接登录");
            return Ok("注册成功");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RequestToken(LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Request");

            if (!_authService.IsAuthenticated(request, out var token))
                return BadRequest("用户不存在或密码错误");
            return Ok(token);
        }
    }
}