using System;
using System.Diagnostics;
using System.Threading;

public partial class kf : System.Web.UI.Page
{
    #region const 常量
    //过期时间
    const int EXPIRED_SECONDS = 10;

    #region 几种状态
    //没有消息
    const string NO_MESSAGE = "";

    //没有用户连接
    const string NO_USER = "nouser";

    //有用户连接
    const string HAVE_USER = "haveuser";

    //过期
    const string EXPIRED = "expired";

    #endregion
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        string kfname = Request["id"];
        KFWindow kfWindow = this.Cache[kfname] as KFWindow;

        #region 没有创建好窗口时继续检测
        if (kfWindow == null)
        {
            Response.Write("");
            Response.End();
            return;
        }
        #endregion

        Stopwatch watch = new Stopwatch();
        watch.Start();

        while (true)
        {
            #region 过期则返回
            if (watch.ElapsedMilliseconds / 1000 > EXPIRED_SECONDS)
            {
                watch.Stop();
                Response.Write(EXPIRED);
                break;
            }
            #endregion

            //有用户连接时
            if (string.IsNullOrEmpty(kfWindow.CurrentUserID) == false)
            {
                //发出有用户来了的消息
                if (kfWindow.HaveUser == false)
                {
                    kfWindow.HaveUser = true;
                    Response.Write(HAVE_USER);
                    break;
                }
                string msg = kfWindow.GetUserMessage();
                //没有消息
                if (string.IsNullOrEmpty(msg))
                {
                    Thread.Sleep(50);    
                    continue;
                }
                else
                {
                    //有消息产生
                    Response.Write(msg);
                    break;
                }
            }
            //没有用户在线
            else
            {
                //发出用户下线的消息
                if (kfWindow.HaveUser == true)
                {
                    kfWindow.HaveUser = false;
                    Response.Write(NO_USER);
                    break;
                }
                else
                    continue;
            }
        }
        Response.End();
    }
}