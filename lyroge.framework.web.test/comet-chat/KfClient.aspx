<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KfClient.aspx.cs" Inherits="KfClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="../js/jquip.min.js"></script>
    <link href="css/css.css" rel="stylesheet" />
    <script>
        var i = 0;
        var interval = null;
        $(function () {
            $("#msginputbox").focus();
            function getKfMessage() {
                $.get("monitor/kf.aspx?id=<%=Name%>", function (data) {
                    if (data && data.indexOf("expired") == -1 && data.indexOf("nouser") == -1 && data.indexOf("haveuser") == -1) {
                        $("#chatarea").append(data)[0].scrollTop = 10000;
                    }
                    else if (data.indexOf("haveuser") != -1) {
                        $(".waiting_tip").hide();
                        $(".have_tip").show();
                    }
                    else if (data.indexOf("nouser") != -1) {
                        $(".waiting_tip").show();
                        $(".have_tip").hide();
                        $(".waiting_tip span").show();
                        $(".message_record").remove();
                        i = 0;
                    }
                    getKfMessage();
                });
            }

            $("#btnsend").click(function () {
                if ($.trim($("#msginputbox").val()) == "")
                    return false;
                $.post("kfclient.aspx", { "id": "<%=Name%>", "msg": $("#msginputbox").val() }, function (d) {
                    if (d == "True") {
                        var date = new Date().getFullYear() + "-" + (new Date().getMonth() + 1) + "-" + new Date().getDate() + " " + new Date().getHours() + ":" + new Date().getMinutes();
                        var content = "<div class='message_record'><div class='message_title'><span class='username'>我说</span><span class='datetime'>" + date + "</span></div><div class='message_content'>" + $("#msginputbox").val() + "</div></div>";
                        $("#chatarea").append(content);
                    }
                    $("#msginputbox").val("");
                    $("#chatarea")[0].scrollTop = 10000;
                });
            });

            //回车提交
            $("#msginputbox").bind("keypress", function (e) {
                if (e.keyCode == 13)
                    $("#btnsend").click();
            });

            getKfMessage();

            interval = setInterval("setDotHtml()", 1000);
        });

        //触发事件销毁窗口
        function beforeUnload() {
            $.post("kfclient.aspx", { "id": "<%=Name%>", "destory": 1 });
            return "确定离开吗？";
        }

        //获取点数
        function getDot(len) {
            var item = [];
            for (var j = 0; j < len; j++)
                item.push(".");
            return item.join("");
        }

        //设置点点文本
        function setDotHtml() {
            i = (i % 40) + 1;
            $(".dotarea").html(getDot(i));
        }
    </script>
    <title>客服窗口 -- 客服<% =Name %></title>
</head>
<body onbeforeunload="return beforeUnload()">
    <form id="form1" runat="server">
        <div>
            <h1>客服<%= Name %> -- <i>与用户对话窗口</i></h1>
            <div id="chatarea">
                <div class="tip">
                    <div class="waiting_tip"><span style="display: none;">用户已断开，</span>正在等待用户接入<span class="dotarea"></span></div>
                    <div class="have_tip">已有用户接入，可以与用户对话了</div>
                </div>
            </div>
            <h4>在下面输入聊天内容</h4>
            <textarea id="msginputbox"></textarea>
            <input id="btnsend" type="button" value="发送" />
        </div>
    </form>
</body>
</html>
