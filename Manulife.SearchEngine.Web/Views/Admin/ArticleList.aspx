<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleList.aspx.cs" Inherits="Manulife.SearchEngine.Web.Admin.ArticleList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>文章列表</title>
</head>
<body>
    <form id="mainForm" runat="server">
        <div align="center">
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"
                DataObjectTypeName="Manulife.SearchEngine.Model.Article" DeleteMethod="Delete"
                InsertMethod="Add" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetAll" TypeName="Manulife.SearchEngine.Service.ArticleService"
                UpdateMethod="Update"></asp:ObjectDataSource>

        </div>
        <a href="ArticleEdit.aspx?action=AddNew">新增</a>
        <asp:ListView ID="lvArticles" runat="server" DataSourceID="ObjectDataSource1">
            <EmptyDataTemplate>
                <table runat="server" style="">
                    <tr>
                        <td>未返回数据。</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <ItemTemplate>
                <tr style="">
                    <td>
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="删除" /><a href="ArticleEdit.aspx?action=Edit&id=<%#Eval("Id") %>">编辑</a>
                    </td>
                    <td>
                        <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                    </td>
                    <td>
                        <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' />
                    </td>
                    <td>
                        <asp:Label ID="MsgLabel" runat="server" Text='<%# Eval("Msg") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <LayoutTemplate>
                <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr runat="server" style="">
                                    <th runat="server"></th>
                                    <th runat="server">Id</th>
                                    <th runat="server">Title</th>
                                    <th runat="server">Msg</th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" style=""></td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:ListView>
    </form>
</body>
</html>
