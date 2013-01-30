using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

/// <summary>
/// KFClientBuilder 的摘要说明
/// </summary>
public class KFClientBuilder
{
    #region 构造函数
    private KFClientBuilder()
    {
    }
    #endregion

    const int MAX_KF_NUMBER = 10;

    public static KFClientBuilder Current = new KFClientBuilder();

    /// <summary>
    /// 创建一个客服窗口，返回窗口名称ID
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public string Build(System.Web.UI.Page page)
    {
        string kfName = GetClientName(page);
        if (string.IsNullOrEmpty(kfName) == false)
        {
            KFWindow kfWindow = new KFWindow(kfName);
            page.Cache.Insert(kfName, kfWindow, null, Cache.NoAbsoluteExpiration, new TimeSpan(12, 0, 0), CacheItemPriority.Normal, null);
            return kfName;
        }
        return "";
    }

    /// <summary>
    /// 获取将要创建窗口的名称ID
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    private string GetClientName(System.Web.UI.Page page)
    {
        for (int i = 0; i < MAX_KF_NUMBER; i++)
        {
            string key = (i + 1).ToString().PadLeft(3, '0');
            if (page.Cache[key] == null)
                return key;
            continue;
        }
        page.Response.Write("<script>alert('已经开启了10个客服，达到极限');window.close();</script>");
        return "";
    }
}