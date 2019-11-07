using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JJFWebCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace JJFWebCore.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

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
    }
}