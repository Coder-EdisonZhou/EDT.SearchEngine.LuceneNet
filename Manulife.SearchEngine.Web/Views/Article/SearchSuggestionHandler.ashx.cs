using Manulife.SearchEngine.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;

namespace Manulife.SearchEngine.Web.Views.Article
{
    /// <summary>
    /// SearchSuggestionHandler 的摘要说明
    /// </summary>
    public class SearchSuggestionHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            // 注意这里传过来的参数name是term
            string keyword = context.Request["term"];

            IList<string> keywordList = new List<string>();
            SearchLogStasticsService statService = new SearchLogStasticsService();
            DataTable dt = statService.GetSuggestion(keyword);
            foreach (DataRow dr in dt.Rows)
            {
                keywordList.Add(Convert.ToString(dr["Word"]));
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string json = jss.Serialize(keywordList);
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}