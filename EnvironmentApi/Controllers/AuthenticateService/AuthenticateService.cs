using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using EnvironmentApi.Models;
using EnvironmentApi.Models.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EnvironmentApi.Controllers.AuthenticateService
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
        UserModel AddUserData(RegistRequestDto request);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        UserModel GetUserData(string username);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserModel> GetUserData();

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        UserModel ModifyUserData(ModifyRequestDto requestDto, bool isAdmin);

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UserModel DeleteUserData(string username);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request">认证请求</param>
        /// <returns>认证结果</returns>
        string Authenticated(LoginRequestDto request);

        /// <summary>
        /// 解析claims获取用户及角色
        /// </summary>
        /// <param name="request"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        string ParsingClaims(IEnumerable<Claim> request, out List<string> roles);
    }

    /// <summary>
    /// 认证方法
    /// </summary>
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly EnvironmentContext _context;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(EnvironmentContext context, IOptions<TokenManagement> tokenManagement)
        {
            _context = context;
            _tokenManagement = tokenManagement.Value;
        }

        public UserModel AddUserData(RegistRequestDto request)
        {
            //rsa解密
            var code = SecurityRsa.Decrypt(request.Password);
            if (code is null) return null;
            //查找用户
            if (_context.User.Find(request.UserName) != null) return null;
            var newUser = new UserModel
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = SecurityAes.Encrypt(code),
                Role = request.Role.ToLower()
            };
            _context.User.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public UserModel GetUserData(string username)
        {
            return _context.User.Find(username);
        }

        public IEnumerable<UserModel> GetUserData()
        {
            return _context.User;
        }

        public UserModel ModifyUserData(ModifyRequestDto requestDto, bool isAdmin)
        {
            //rsa解密
            var oldCode = SecurityRsa.Decrypt(requestDto.OldPassword);
            var code = SecurityRsa.Decrypt(requestDto.Password);
            if (oldCode is null || code is null) return null;
            //查找用户
            var user = _context.User.Find(requestDto.UserName);
            if (user is null || (user.Password != SecurityAes.Encrypt(oldCode) && !isAdmin))
                return null;
            user.Email = requestDto.Email;
            user.Password = SecurityAes.Encrypt(code);
            user.Role = requestDto.Role.ToLower();
            _context.User.Update(user);
            _context.SaveChanges();
            return user;
        }

        public UserModel DeleteUserData(string username)
        {
            var user = _context.User.Find(username);
            if (user is null)
                return null;
            _context.User.Remove(user);
            _context.SaveChanges();
            return user;
        }

        public string Authenticated(LoginRequestDto request)
        {
            //rsa解密
            var code = SecurityRsa.Decrypt(request.Password);
            if (code is null) return null;
            //获取用户
            var user = _context.User.Find(request.UserName);
            if (user is null || user.Password != SecurityAes.Encrypt(code))
                return null;
            //获取用户角色
            var roles = user.Role.Split("::", StringSplitOptions.RemoveEmptyEntries).ToList();

            //创建claim
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            //生成token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer, _tokenManagement.Audience, claims,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration), signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }

        public string ParsingClaims(IEnumerable<Claim> request, out List<string> roles)
        {
            var user = string.Empty;
            roles = new List<string>();
            foreach (var claim in request)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.Name:
                        user = claim.Value;
                        break;
                    case ClaimTypes.Role:
                        roles.Add(claim.Value);
                        break;
                }
            }

            return user;
        }
    }
}