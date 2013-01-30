using System;
using System.Web.Caching;

public partial class KfClient : System.Web.UI.Page
{
    protected string Name;

    protected void Page_Load(object sender, EventArgs e)
    {
        //销毁命令
        if (Request["destory"] != null)
        {
            KFWindow window = Cache.Remove(Request["id"]) as KFWindow;
            window.DumpData();
            return;
        }

        //创建一个新的窗口
        if (Request["msg"] == null)
        {
            Name = KFClientBuilder.Current.Build(this);
        }
        else
        {   //发送客服消息
            Message message = new Message();

            message.DateTime = DateTime.Now;
            message.Content = Request["msg"];
            message.MessageType = MessageType.KF;
            message.HaveRead = false;

            bool b = (Cache[Request["id"]] as KFWindow).AddMessage(message);
            Response.Write(b);
            Response.End();
        }
    }
}