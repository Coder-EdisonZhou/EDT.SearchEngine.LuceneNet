<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewArticle.aspx.cs" Inherits="Manulife.SearchEngine.Web.ViewArticle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=ArticleTitle%></title>
    <link href="/assets/css/post.css" rel="stylesheet" />
</head>
<body>
    <form id="mainForm" runat="server">
        <section class="page">
            <hgroup>
                <h1 class="page-head"><a href="#" title="<%=ArticleTitle%>"><%=ArticleTitle%></a></h1>
            </hgroup>
            <section class="panel">
                <%=ArticleMsg%>
            </section>
        </section>
    </form>
    <div id="footerArea" class="page-foot">
        <p>Copyright &copy; 2016 <a href="http://edisonchou.cnblogs.com" target="_blank">Edison Chou</a></p>
    </div>
</body>
</html>
