using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace lyroge.framework.Http
{
    public static class MyWebClient
    {
        /// <summary>
        /// 请求指定的URL，返回Response对象 注意：在Asp.Net页面Post的时候要加入__VIEWSTATE、__EVENTVALIDATION数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">post提交时的数据内容(a=1&b=2) 注意：post内容的值要url encode编码</param>
        /// <param name="SetHeaderAction">
        /// 设置请求头的信息
        /// 如：ContentType 、ContentLength 、Method 等 
        /// 默认设置的头：KeepAlive=true; AllowAutoRedirect=false; request.ContentType = "application/x-www-form-urlencoded";
        /// </param>        
        /// <returns>返回带Cookie对象的Response对象</returns>
        public static HttpWebResponse GetResponse(string url, string postData = null, Action<HttpWebRequest> SetHeaderAction = null)
        {
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            
            #region 默认的request头信息
            CookieContainer cc = new CookieContainer();        
            request.CookieContainer = cc;
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.ContentType = "application/x-www-form-urlencoded";
            #endregion

            #region 自定义头部信息
            if (SetHeaderAction != null)
                SetHeaderAction(request);
            #endregion

            #region 如果是post提交的话，写入提交的数据到流中
            if (request.Method == "POST")
            {
                postData = postData ?? string.Empty;
                var bytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = bytes.Length;

                var stream = request.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();
            }
            #endregion

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            response.Cookies = cc.GetCookies(new Uri(url));
            return response;
        }

        /// <summary>
        /// 获取URL地址页面返回的内容
        /// </summary>
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

        /// <summary>
        /// multipart/form-data类型post数据
        /// </summary>
        public static WebResponse UploadFile(string url, UploadedFile uploadedFile, Dictionary<string, string> dicPostData = null, Action<HttpWebRequest> SetHeaderAction = null)
        {     
            HttpWebRequest wr = WebRequest.Create(url) as HttpWebRequest;

            #region 请求头信息基本设置
            //分界符字符串 拼在请求头中 multipart/form-data; boundary=boundary_string
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.AllowAutoRedirect = false;            
            #endregion

            #region 外部设置请求头
            if (SetHeaderAction != null)
                SetHeaderAction(wr);
            #endregion

            Stream rs = wr.GetRequestStream();

            //首先输入分界符字节串
            //提交的数据中分界符要加两个 '-' 
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary);
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            #region 输入上传文件的内容
            if (uploadedFile != null && uploadedFile.data.Length > 0)
            {
                var ContentDisposition = 
                Encoding.UTF8.GetBytes(string.Format("\r\nContent-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", uploadedFile.Name, uploadedFile.FileName, uploadedFile.ContentType));

                rs.Write(ContentDisposition, 0, ContentDisposition.Length);
                rs.Write(uploadedFile.data, 0, uploadedFile.data.Length);
            }
            #endregion          

            #region 写入表单普通数据内容
            if (dicPostData != null && dicPostData.Count > 0)
            {
                foreach (var kv in dicPostData)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    var ContentDisposition = System.Text.Encoding.UTF8.GetBytes(string.Format("\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n", kv.Key));
                    rs.Write(ContentDisposition, 0, ContentDisposition.Length);

                    var Content = System.Text.Encoding.UTF8.GetBytes(kv.Value);
                    rs.Write(Content, 0, Content.Length);
                }
            }
            #endregion            
            
            #region 输入内容尾，并关闭流
            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();
            #endregion

            var webresponse = wr.GetResponse();
            return webresponse;
        }
    }

    /// <summary>
    /// 上传的文件内容结构
    /// </summary>
    public class UploadedFile
    {
        public UploadedFile()
        {
            data = new byte[0];
        }

        /// <summary>
        /// 上传文件控件html元素的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>        
        public string FileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        public byte[] data { get; set; }
    }
}
