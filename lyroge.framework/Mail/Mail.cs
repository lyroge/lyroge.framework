using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace lyroge.framework.Mail
{
    public class Mail
    {
        /// <summary>
        /// 发送邮件到emails邮件， 多个邮件以;隔开
        /// </summary>
        /// <param name="tos"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool SendMail(string from, string fromDisplayName, string tos, string subject, string body)
        {
            MailMessage message = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                Priority = MailPriority.High,
                From = new MailAddress(from, fromDisplayName),
            };

            //添加目的地址
            var emails = tos.Split(';');
            foreach (string m in emails)
            {
                if (!string.IsNullOrEmpty(m))
                    message.To.Add(m);
            }

            //发送邮件服务器
            SmtpClient sc = new SmtpClient();
            sc.Host = "smtp.126.com";
            sc.Port = 25;

            //指定登录服务器的用户名和密码
            sc.Credentials = new NetworkCredential("terrygon@sina.cn", "********"); 
            try
            {
                sc.Send(message);      
            }
            catch
            {
                throw;
            }
            return true;
        }
    }
}
