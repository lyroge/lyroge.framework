using System;
using System.Threading;
using System.Diagnostics;

public partial class ct : System.Web.UI.Page
{
    //过期时间
    const int EXPIRED_SECONDS = 10;
    const string EXPIRED = "expired";
    const string KF_OFF_LINE = "kfoffline";

    protected void Page_Load(object sender, EventArgs e)
    {
        string kfname = Request["id"];
        KFWindow kfWindow = this.Cache[kfname] as KFWindow;

        //客服已经不在线
        if (kfWindow == null)
        {
            Response.Write(KF_OFF_LINE);
            Response.End();
            return;
        }

        Stopwatch watch = new Stopwatch();
        watch.Start();

        while (true)
        {
            //过期则返回
            if (watch.ElapsedMilliseconds / 1000 > EXPIRED_SECONDS)
            {
                //过期将客服窗口销毁,返回过期提示        
                Response.Write(EXPIRED);
                watch.Stop();
                break;
            }

            string msg = kfWindow.GetKfMessage();
            //未获取到消息
            if (string.IsNullOrEmpty(msg))
            {
                Thread.Sleep(50);
                continue;
            }
            //获取到消息则输出
            else
            {
                Response.Write(msg);
                break;
            }
        }
        Response.End();
    }
}