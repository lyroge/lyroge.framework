using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

/// <summary>
/// ChatClientBuilder 的摘要说明
/// </summary>
public class ChatClientBuilder
{
    #region 构造函数
    private ChatClientBuilder()
    {
    }
    #endregion

    const int MAX_KF_NUMBER = 10;

    public static ChatClientBuilder Current = new ChatClientBuilder();

    /// <summary>
    /// 获取可用客服窗口，返回客服窗口ID
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public string Build(System.Web.UI.Page page)
    {
        //只能有一个线程获取窗口操作
        lock (this)
        {
            KFWindow window = GetKFClient(page);
            if (window != null)
            {
                window.CurrentUserID = "chatting";
                return window.Name;
            }
        }
        return "";
    }

    /// <summary>
    /// 获取客服窗口名称
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    private KFWindow GetKFClient(System.Web.UI.Page page)
    {
        int onLineNum = 0;
        int idleNum = 0;
        for (int i = 0; i < MAX_KF_NUMBER; i++)
        {
            string key = (i + 1).ToString().PadLeft(3, '0');
            if (page.Cache[key] != null)
            {
                onLineNum++;
                if ((page.Cache[key] as KFWindow).CurrentUserID == null)
                {
                    idleNum++;
                    return page.Cache[key] as KFWindow;
                }
            }
        }

        string message = string.Empty;
        if (idleNum == 0)
            message = "十分抱歉，现在客服人员都在繁忙工作中，请稍后再试";
        if (onLineNum == 0)
        {
            //message = "十分抱歉，现在没有客服人员在线，请留言";
            KFWindow window = new KFWindow(page.Session.SessionID);
            page.Cache.Insert(window.Name, window, null, Cache.NoAbsoluteExpiration, new TimeSpan(12, 0, 0), CacheItemPriority.Normal, null);
            return window;
        }

        page.Response.Write(message);
        page.Response.End();
        return null;
    }
}