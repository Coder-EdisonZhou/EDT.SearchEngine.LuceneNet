using System;
using Manulife.SearchEngine.Service;

namespace Manulife.SearchEngine.Web
{
    public partial class ViewArticle : System.Web.UI.Page
    {
        protected string ArticleTitle { get; set; }
        protected string ArticleMsg { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            long id = Convert.ToInt64(Request["id"]);

            ArticleService articleService = new ArticleService();
            var art = articleService.GetById(id);
            if (art == null)
            {
                Response.Write("文章不存在");
                Response.End();
            }

            ArticleTitle = art.Title;
            ArticleMsg = art.Msg;
        }
    }
}