<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="Search.aspx.cs" Inherits="Manulife.SearchEngine.Web.Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Engine Demo</title>
    <link href="/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/assets/css/ui-lightness/jquery-ui-1.8.2.custom.css" rel="stylesheet" />
    <link href="/assets/css/default.css" rel="stylesheet" />
</head>
<body>
    <form id="mainForm" method="get" action="Search.aspx">
        <div id="searchArea" class="operate">
            <img id="imgLogo" alt="logo" src="/assets/images/logo.jpg" class="logo" />
            <br />
            <input id="txtKeyword" name="keyword" type="text" class="keywords" placeholder="Keyword" onkeypress="startSearch()" value="<%= Request["keyword"] %>" />
            <input id="btnSearch" type="submit" class="btn btn-info search" value="Search" onclick="return checkKeywordNull();" />
        </div>
        <div id="contentArea" class="content">
            <div id="hotkeyArea" class="hotkeys">
                <!-- 搜索热词 -->
                <span class="hottip">一周搜索热词：</span>
                <ul id="hotKeywords">
                    <asp:Repeater ID="rptHotList" runat="server">
                        <ItemTemplate>
                            <li><a href='ViewArticle.aspx?id=<%# Eval("Word") %>'><%# Eval("Word") %></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <br />
            </div>
            <!-- 耗时提示 -->
            <span><%= Stastics %></span>
            <!-- 搜索结果 -->
            <ul id="searchResults">
                <asp:Repeater ID="rptSearchResult" runat="server">
                    <ItemTemplate>
                        <li><a href="<%#Eval("Url") %>"><%#Eval("Title") %></a><p><%#Eval("Msg") %></p>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div id="pagerArea" class="pager"><%= PagerHTML %></div>
    </form>
    <div id="footerArea" class="footer">
        <p>Copyright &copy; 2016 <a href="http://edisonchou.cnblogs.com" target="_blank">Edison Chou</a></p>
    </div>
</body>
</html>
<script type="text/javascript" src="/assets/js/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="/assets/js/jquery-ui-1.8.2.custom.min.js"></script>
<script type="text/javascript">
    function checkKeywordNull() {
        var keyword = document.getElementById('txtKeyword').value;
        if (keyword == null || keyword == '') {
            return false;
        }
        return true;
    }

    function startSearch() {
        if (event.keyCode == 13) {
            document.getElementById('btnSearch').click();
        }
    }

    $(function () {
        $("#txtKeyword").autocomplete({
            source: "SearchSuggestionHandler.ashx",
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.value);
                $("#mainForm").submit();
            }
        });
        $("#txtKeyword").focus();
    });
</script>
