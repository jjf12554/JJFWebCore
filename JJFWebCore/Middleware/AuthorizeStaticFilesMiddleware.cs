using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace JJFWebCore.Middleware
{
    /// <summary>
    /// 文件管理中间件
    /// </summary>
    public class AuthorizeStaticFilesMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeStaticFilesMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthorizationService authorService)
        {
            var url = context.Request.Path;
            var sid = context.Request.Headers["sid"].ToString();
            if (string.IsNullOrEmpty(sid))
            {
                throw new Exception("resource 403 forbidden sid is empty");
            }

            var result = ValidateResourceAuthor(url,sid);

            if (result == false)
            {
                await context.ForbidAsync();
            }

            await _next(context);
        }
        public bool ValidateResourceAuthor(string url,string sid)
        {
            //var loginUser = UserHelper._GetUser(req.SID);

            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("url is empty");
            }
            //https://localhost:5001/assets/upload/images/20181018/0d9819d2-14d2-47eb-a763-be9d19c69e42.jpg
            url = url.Trim().ToLower();

            if (url.EndsWith(".mp4") || url.EndsWith(".mp3"))
            {
                //...
            }

            return true;
        }

    }
}
