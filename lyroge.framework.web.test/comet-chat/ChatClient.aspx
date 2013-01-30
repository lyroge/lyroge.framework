<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChatClient.aspx.cs" Inherits="ChatClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="../js/jquip.min.js"></script>
    <link href="css/css.css" rel="stylesheet" />
    <script language="javascript">
        $(function () {
            function getKfMessage() {
                $("#msginputbox").focus();
                $.get("monitor/ct.aspx?id=<%=Name%>", function (data) {
                    if (data.indexOf("expired") == -1 && data.indexOf("kfoffline") == -1) {
                        $("#chatarea").append(data)[0].scrollTop = 10000;
                    }
                    else if (data.indexOf("kfoffline") != -1) {
                        document.write("很抱歉，客服人员离线，页面即将关闭");
                        return false;
                    }
                    getKfMessage();
                });
            }

            if ("<%=CanChat%>" == "True") {
                getKfMessage();
            }

            $("#btnsend").click(function () {
                if ($.trim($("#msginputbox").val()) == "")
                    return false;
                $.post("chatclient.aspx", { "id": "<%=Name%>", "msg": $("#msginputbox").val() }, function () {
                    var date = new Date().getFullYear() + "-" + (new Date().getMonth() + 1) + "-" + new Date().getDay() + " " + new Date().getHours() + ":" + new Date().getMinutes();
                    var content = "<div class='message_record'><div class='message_title'><span class='username'>我说</span><span class='datetime'>" + date + "</span></div><div class='message_content'>" + $("#msginputbox").val() + "</div></div>";
                    $("#chatarea").append(content);
                    $("#msginputbox").val("");
                    $("#chatarea")[0].scrollTop = 10000;
                });
            });

            //回车提交
            $("#msginputbox").bind("keypress", function (e) {
                if (e.keyCode == 13)
                    $("#btnsend").click();
            });
        });

        //触发事件销毁窗口
        function beforeUnload() {
            $.post("chatclient.aspx", { "id": "<%=Name%>", "destory": 1 });
            return "确认退出吗？";
        }
    </script>
    <title>聊天窗口 -- 与客服<%= Name %></title>
</head>
<body onbeforeunload="return beforeUnload()">
    <form id="form1" runat="server">
        <div>
            <h1>聊天窗口 -- <i><%= Title %></i></h1>
            <div id="chatarea">
                <div class="tip">
                    <div>红袖添香 www.hongxiu.com ，中国领先的女性文学网 欢迎您！<i><%= Title %></i></div>
                </div>
            </div>
            <h4>在下面输入聊天内容</h4>
            <textarea id="msginputbox"></textarea>
            <input id="btnsend" type="button" value="发送" />
        </div>
    </form>
</body>
</html>
