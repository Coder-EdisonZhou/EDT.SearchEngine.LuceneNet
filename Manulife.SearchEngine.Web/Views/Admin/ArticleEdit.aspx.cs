using System;
using Manulife.SearchEngine.Model;
using Manulife.SearchEngine.Service;
using Manulife.SearchEngine.Web.Common;

namespace Manulife.SearchEngine.Web.Admin
{
    public partial class ArticleEdit : System.Web.UI.Page
    {
        private ArticleService articleService;

        public ArticleEdit()
        {
            articleService = new ArticleService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string action = Request["action"];
                if (action == "Edit")
                {
                    long id = Convert.ToInt64(Request["id"]);
                    Article art = articleService.GetById(id);
                    txtTitle.Text = art.Title;
                    txtMsg.Text = art.Msg;
                }
                else if (action == "AddNew")
                {
                    // do nothing!
                }
                else
                {
                    throw new Exception("action错误！");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string action = Request["action"];
            if (action == "Edit")
            {
                long id = Convert.ToInt64(Request["id"]);
                Article art = articleService.GetById(id);
                art.Title = txtTitle.Text;
                art.Msg = txtMsg.Text;

                // 更新数据库
                articleService.Update(art);

                // 更新索引库
                IndexTask task = new IndexTask();
                task.TaskId = id;
                IndexManager.Instance.UpdateArticle(task);

                Response.Redirect("ArticleList.aspx");
            }
            else if (action == "AddNew")
            {
                Article art = new Article();
                art.Title = txtTitle.Text;
                art.Msg = txtMsg.Text;

                // 更新数据库
                art = articleService.Add(art);

                // 更新索引库
                IndexTask task = new IndexTask();
                task.TaskId = art.Id;
                IndexManager.Instance.AddArticle(task);

                Response.Redirect("ArticleList.aspx");
            }
            else
            {
                throw new Exception("action错误！");
            }
        }
    }
}