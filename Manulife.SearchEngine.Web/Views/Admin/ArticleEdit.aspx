<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleEdit.aspx.cs" Inherits="Manulife.SearchEngine.Web.Admin.ArticleEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发布文章</title>
</head>
<body>
    <form id="mainForm" runat="server">
        <div align="center">
            标题：<asp:TextBox ID="txtTitle" runat="server" Width="330px"></asp:TextBox>
            <br />
            正文：<br />
            <asp:TextBox ID="txtMsg" runat="server" Height="282px" TextMode="MultiLine"
                Width="417px"></asp:TextBox>
            <br />
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="保存" />
        </div>
    </form>
</body>
</html>
