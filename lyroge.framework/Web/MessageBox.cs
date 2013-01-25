using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace lyroge.framework.Web
{
    public static class MessageBox
    {
        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void Show(this Page page, string msg)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript' defer>alert('" + msg.ToString() + "');</script>");
        }

        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page"></param>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        public static void ShowMessageAndRedirect(this Page page, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.AppendFormat("top.location.href='{0}'", url);
            Builder.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());
        }

        /// <summary>
        /// 输出自定义脚本信息
        /// </summary>
        public static void ResponseScript(this Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javascript'>" + script + "</script>");
        }

        /// <summary>
        /// 为服务器控件添加确认事件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="msg"></param>
        public static void ShowConfirm(this WebControl control, string msg)
        {
            control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }
    }
}

