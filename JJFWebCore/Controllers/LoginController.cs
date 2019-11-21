using System;
using System.Collections.Generic;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JJFWebCore.Models;
using Microsoft.AspNetCore.Mvc;
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
            return new AjaxResult { state = ResultType.success.ToString(), message = "登录成功。" };
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
    }
}