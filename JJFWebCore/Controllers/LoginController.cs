using System;
using System.Collections.Generic;
using System.DrawingCore.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JJFWebCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebTools;

namespace JJFWebCore.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public AjaxResult CheckLogin(string username, string password, string code)
        {
            if (!username.Equals("kbdadmin"))
            {
                if (1==2)
                {
                    throw new Exception("验证码错误，请重新输入");
                }
            }
            SessionUser sessionUser = new SessionUser()
            {
                Id = 1,
                Name = "jiangjf",
                Role = "admin"
            };
            string jwtToken = GetAccessToken(sessionUser);
            return new AjaxResult { state = ResultType.success.ToString(), message = jwtToken };
        }
        #region 数字验证码
        [HttpGet]
        public IActionResult NumberVerifyCode()
        {
            string code = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.NumberVerifyCode);
            var bitmap = VerifyCodeHelper.GetSingleObj().CreateBitmapByImgVerifyCode(code, 100, 40);
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            return File(stream.ToArray(), "image/png");
        }
        #endregion
        private string GetAccessToken(SessionUser user)
        {
            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Sub, "admin")
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TESTTESTTESTTESTTESTTEST"));
            var token = new JwtSecurityToken(
                   issuer: "jjf",
                   audience: "jiangjf",
                   expires: DateTime.Now.AddHours(2),
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                   );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public class SessionUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }
    }
}