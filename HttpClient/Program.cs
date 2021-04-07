using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using EnvironmentApi.Controllers;
using EnvironmentApi.Controllers.AuthenticateService;
using EnvironmentApi.Models;
using EnvironmentApi.Models.Authentication;

namespace HttpClient
{
    class Program
    {
        private static readonly HttpClientHandler HttpclientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true //httpClient忽略SSL
        };

        private static readonly System.Net.Http.HttpClient Client = new(HttpclientHandler)
        {
            BaseAddress = new Uri("https://127.0.0.1/")
        };

        static void Main(string[] args)
        {
            Console.WriteLine("=== EnvironmentApi Auto Test ===");

            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var device = ClientGet<DeviceStatusModel>("DeviceStatus/Get/latest");
            var devices = ClientGet<IEnumerable<DeviceStatusModel>>("DeviceStatus/Get/hour");
            var environment = ClientGet<EnvironmentModel>("Environment/Get/latest");
            var environments = ClientGet<IEnumerable<EnvironmentModel>>("Environment/Get/hour");

            //获取公钥
            var pubKey = ClientGet<string>("Authentication/GetKey");
            //使用公钥加密密码
            SecurityRsa.InitRsaByKey(pubKey, null); //私钥不使用，随意填充
            const string user = "TESTUSER";
            var pwdCode = SecurityRsa.Encrypt(user);
            //注册
            var register = ClientPost<string, RegistRequestDto>("Authentication/Register",
                new RegistRequestDto
                {
                    UserName = user,
                    Email = $"{user}@TEST",
                    Password = pwdCode,
                    Role = "public"
                });
            //登录
            var jwt = ClientPost<string, LoginRequestDto>("Authentication/Login",
                new LoginRequestDto
                {
                    UserName = user,
                    Password = pwdCode
                });
            //设置jwt
            Client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {jwt}");
            //获取信息
            var info = ClientGet<UserModel>("Authentication/Information");
            //修改信息
            var modify = ClientPost<string, ModifyRequestDto>("Authentication/Modify",
                new ModifyRequestDto
                {
                    UserName = user,
                    Email = $"{user}@TEST.com",
                    OldPassword = pwdCode,
                    Password = pwdCode,
                    Role = "public"
                });
            //删除用户
            var delete = ClientGet<string>($"Authentication/Delete/{user}");
        }

        /// <summary>
        /// httpClient发送Get请求(阻塞)
        /// </summary>
        /// <param name="uri">请求Uri</param>
        /// <typeparam name="T">响应类型</typeparam>
        /// <returns>响应信息</returns>
        private static T ClientGet<T>(string uri)
        {
            try
            {
                Console.WriteLine($"Get ==> {Client.BaseAddress}{uri}");
                var response = Client.GetAsync(uri).Result;
                Console.WriteLine($"Response ==> {(int) response.StatusCode}, {response.ReasonPhrase}");
                return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<T>().Result : default;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed ==> {e.Message}");
                return default;
            }
        }

        /// <summary>
        /// httpClient发送Post请求(阻塞)
        /// </summary>
        /// <param name="uri">请求Uri</param>
        /// <param name="message">请求信息</param>
        /// <typeparam name="T">响应类型</typeparam>
        /// <typeparam name="TU">请求类型</typeparam>
        /// <returns>请求信息</returns>
        private static T ClientPost<T, TU>(string uri, TU message)
        {
            try
            {
                Console.WriteLine($"Post ==> {Client.BaseAddress}{uri}");
                var response = Client.PostAsJsonAsync(uri, message).Result;
                Console.WriteLine($"Response ==> {(int) response.StatusCode}, {response.ReasonPhrase}");
                return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<T>().Result : default;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed ==> {e.Message}");
                return default;
            }
        }
    }
}