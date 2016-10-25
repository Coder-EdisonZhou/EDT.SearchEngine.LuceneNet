<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchEngineV1.aspx.cs" Inherits="Manulife.SearchEngine.LuceneNet.Views.SearchEngineV1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>最简单的搜索引擎</title>
</head>
<body>
    <form id="mainForm" runat="server">
        <div align="center">
            <asp:Button ID="btnCreateIndex" runat="server" Text="Create Index" OnClick="btnCreateIndex_Click" />
            <asp:Label ID="lblIndexStatus" runat="server" Visible="false" />
            <hr />
            <asp:TextBox ID="txtKeyWords" runat="server" Text="" Width="250"></asp:TextBox>
            <asp:Button ID="btnGetSearchResult" runat="server" Text="Search" OnClick="btnGetSearchResult_Click" />
            <hr />
        </div>
        <div>
            <ul>
                <asp:Repeater ID="rptSearchResult" runat="server">
                    <ItemTemplate>
                        <li>Id:<%#Eval("Id") %><br />
                            <%#Eval("Msg") %></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </form>
</body>
</html>
