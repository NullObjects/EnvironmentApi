using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EnvironmentApi.Models
{
    /// <summary>
    /// 认证接口
    /// </summary>
    public interface IAuthenticateService
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool IsRegisted(RegistRequestDto request);

        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="request">认证请求</param>
        /// <param name="token">生成token</param>
        /// <returns>认证结果</returns>
        bool IsAuthenticated(LoginRequestDto request, out string token);
    }

    /// <summary>
    /// 认证方法
    /// </summary>
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUser _user;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(IUser user, IOptions<TokenManagement> tokenManagement)
        {
            _user = user;
            _tokenManagement = tokenManagement.Value;
        }

        public bool IsRegisted(RegistRequestDto request)
        {
            //用户已存在
            if (_user.Select(request.UserName) != null)
                return false;
            //用户不存在
            _user.Add(new UserModel
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role
            });
            return true;
        }

        public bool IsAuthenticated(LoginRequestDto request, out string token)
        {
            token = string.Empty;
            //获取用户
            var user = _user.Select(request.Username);
            if (user is null || user.Password != request.Password)
                return false;
            //获取用户角色
            var roles = user.Role.Split("::", StringSplitOptions.RemoveEmptyEntries).ToList();

            //创建claim
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            //生成token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer, _tokenManagement.Audience, claims,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration), signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}