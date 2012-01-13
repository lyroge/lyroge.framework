using System;
using System.Text;
using System.Net;
using System.IO;

namespace lyroge.framework.HttpWeb
{
    public static class RequestHelper
    {
        /// <summary>
        /// 请求指定的url， 返回Response对象 注意：在asp.net页面post的时候要加入__VIEWSTATE、__EVENTVALIDATION数据
        /// </summary>
        /// <param name="url">待请求的地址</param>
        /// <param name="SetHeaderAction">设置请求头的信息，如：ContentType 、ContentLength 、Method 等 默认设置的头：KeepAlive=true; AllowAutoRedirect=false; request.ContentType = "application/x-www-form-urlencoded";</param>
        /// <param name="postdata">post时提交的数据 注意：post内容的值要url encode编码</param>
        /// <returns>返回带Cookie对象的Response对象</returns>
        public static HttpWebResponse GetResponse(string url, string postdata = null, Action<HttpWebRequest> SetHeaderAction = null)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            CookieContainer cc = new CookieContainer();

            request = HttpWebRequest.Create(url) as HttpWebRequest;

            //默认的request头信息
            request.CookieContainer = cc;
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = "application/x-www-form-urlencoded";

            //外部设置请求头信息
            if (SetHeaderAction != null)
                SetHeaderAction(request);

            //如果是post提交的话，写入提交的数据到流中
            if (request.Method == "POST")
            {
                postdata = postdata ?? string.Empty;
                var bytes = Encoding.UTF8.GetBytes(postdata);
                request.ContentLength = bytes.Length;
                var stream = request.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();
            }

            response = request.GetResponse() as HttpWebResponse;
            response.Cookies = cc.GetCookies(new Uri(url));
            return response;
        }

        /// <summary>
        /// 请求指定的url， 返回请求后页面的内容 注意：在asp.net页面post的时候要加入__VIEWSTATE、__EVENTVALIDATION数据
        /// </summary>
        /// <param name="url">待请求的地址</param>
        /// <param name="SetHeaderAction">设置请求头的信息，如：ContentType 、ContentLength 、Method 等 默认设置的头：KeepAlive=true; AllowAutoRedirect=false; </param>
        /// <param name="postdata">post时提交的数据 注意：post内容的值要url encode编码</param>
        /// <returns>返回请求后页面的内容</returns>
        public static string GetResponseContent(string url, string postdata = null, Action<HttpWebRequest> SetHeaderAction = null)
        {
            var result = string.Empty;
            var response = GetResponse(url, postdata, SetHeaderAction);
            using (var sr = new StreamReader(response.GetResponseStream(), true))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }
    }
}
