using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChatClient : System.Web.UI.Page
{
    protected string Name;
    protected string ClientName;
    protected bool CanChat;
    protected string Title;

    protected void Page_Load(object sender, EventArgs e)
    {
        //销毁命令
        if (Request["destory"] != null)
        {
            KFWindow window = (Cache[Request["id"]] as KFWindow);
            window.DumpData();
            window.CurrentUserID = null;
            return;
        }

        //创建一个新的窗口
        if (Request["msg"] == null)
        {
            Name = ChatClientBuilder.Current.Build(this);
            int kfid;
            CanChat = int.TryParse(Name, out kfid);
            if (CanChat == false)
                Title = "暂时没有客服在线，请留言";
            else
                Title = "与客服" + Name + "交谈中";
        }
        else
        {   //发送客服消息
            Message message = new Message();
            message.DateTime = DateTime.Now;
            message.Content = Request["msg"];
            message.MessageType = MessageType.User;
            message.HaveRead = false;
            (Cache[Request["id"]] as KFWindow).AddMessage(message);
        }
    }
}