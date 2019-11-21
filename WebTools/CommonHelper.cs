using System;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;

namespace WebTools
{
    public class CommonHelper
    {
        #region 验证码
        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }
        #endregion
        public static string CodeJson(string code, string msg)
        {
            return "{\"code\":\"" + code + "\",\"msg\":\"" + msg + "\"}";
        }
    }
}
