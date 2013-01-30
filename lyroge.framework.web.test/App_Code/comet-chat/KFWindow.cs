using System;
using System.Collections.Generic;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using lyroge.framework.DbHelper;

/// <summary>
/// KFWindow 的摘要说明
/// 客服窗口管理类，包括消息数据结构
/// </summary>
public class KFWindow 
{
    /// <summary>
    /// 没有消息记录
    /// </summary>
    const string NO_MESSAGE = "";

    #region 构造函数

    public KFWindow(string name)
    {
        Name = name;
        MessageList = new MessageList();
        WaitingUsers = new Queue<string>();
        HaveUser = false;
    }
    #endregion

    #region 窗口属性
    /// <summary>
    /// 客服名称,窗口名称
    /// </summary>
    public string Name;

    /// <summary>
    /// 正在聊天客户ID
    /// </summary>
    public string CurrentUserID;

    /// <summary>
    /// 当前聊天记录，客户、客服都有
    /// </summary>
    public MessageList MessageList;

    /// <summary>
    /// 等待中的用户列表
    /// </summary>
    public Queue<string> WaitingUsers;

    public bool HaveUser;
    #endregion

    #region 获取消息方法
    /// <summary>
    /// 获取用户发送的消息
    /// </summary>
    /// <returns></returns>
    public string GetUserMessage()
    {
        return GetMessage(MessageType.User);
    }

    /// <summary>
    /// 获取客服发送的消息
    /// </summary>
    /// <returns></returns>
    public string GetKfMessage()
    {
        return GetMessage(MessageType.KF);
    }

    /// <summary>
    /// 获取未读消息记录
    /// </summary>
    /// <param name="messageType"></param>
    /// <returns></returns>
    private string GetMessage(MessageType messageType)
    {
        string html = "<div class='message_record'><div class='message_title'><span class='username'>{0}</span><span class='datetime'>{1}</span></div><div class='message_content'>{2}</div></div>";
        string kfName = messageType == MessageType.User ? "用户" : "客服-" + Name;
        System.Text.StringBuilder sbHtml = new System.Text.StringBuilder();

        MessageList messages = new MessageList();
        foreach (Message msg in MessageList)
        {
            if (msg.MessageType == messageType && msg.HaveRead == false)
            {
                messages.Add(msg);

                //将获取到的消息至为已读
                msg.HaveRead = true;

                sbHtml.AppendFormat(html, kfName, msg.DateTime.ToString("yyyy-M-d H:m"), msg.Content);
            }
        }

        //没有消息则返回空
        if (messages.Count == 0)
            return NO_MESSAGE;


        return sbHtml.ToString();
    }
    #endregion

    /// <summary>
    /// 添加一条消息记录到列表
    /// </summary>
    /// <param name="message"></param>
    public bool AddMessage(Message message)
    {
        if (message.MessageType == MessageType.KF && string.IsNullOrEmpty(CurrentUserID) == true)
            return false;

        Monitor.Enter(MessageList);
        MessageList.Add(message);
        Monitor.Exit(MessageList);
        return true;
    }

    /// <summary>
    /// 保存聊天记录的xml文件格式
    /// </summary>
    const string XML_FORMAT = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Records>{0}</Records>";
    const string XML_RECORD = "<Record><From>{0}</From><To>{1}</To><DateTime>{2}</DateTime><Content>{3}</Content></Record>";
    const string INSERT_CONTENT = "insert into tb_ChatRecord(userid, username, kfname, content) values(@userid, @username, @kfname, @content);";

    public void DumpData()
    {
        if (MessageList.Count == 0)
            return;

        System.Text.StringBuilder sbMessages = new System.Text.StringBuilder();
        foreach (Message msg in MessageList)
        {
            string from = msg.MessageType == MessageType.KF ? "客服" : CurrentUserID;
            string to = msg.MessageType == MessageType.KF ? CurrentUserID : "客服";
            sbMessages.AppendFormat(XML_RECORD, from, to, msg.DateTime.ToString(), msg.Content);
        }
        string content = string.Format(XML_FORMAT, sbMessages.ToString());

        int kfname;
        if (int.TryParse(Name, out kfname) == false)
            Name = "";

        SqlParameter pUserID = new SqlParameter("@userid", 0);
        SqlParameter pUserName = new SqlParameter("@username", CurrentUserID);
        SqlParameter pKfName = new SqlParameter("@kfname", Name);
        SqlParameter pContent = new SqlParameter("@content", content);

        //需要配置连接数据库
        IDbHelper db = new SqlHelper("", true);
        db.ExecuteNonQuery(INSERT_CONTENT, pUserID, pUserName, pKfName, pContent);
    }
}

#region 消息相关类

/// <summary>
/// 消息类别
/// </summary>
public enum MessageType
{
    KF,
    User
}

/// <summary>
/// 消息
/// </summary>
public class Message
{
    public string Content;
    public DateTime DateTime;
    public MessageType MessageType;
    public bool HaveRead;
}

/// <summary>
/// 消息列表
/// </summary>
public class MessageList : List<Message>
{
}

#endregion