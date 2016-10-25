using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Manulife.SearchEngine.Model;
using Manulife.SearchEngine.Service;
using Manulife.SearchEngine.Web.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Manulife.SearchEngine.Web
{
    public partial class Search : System.Web.UI.Page
    {
        protected string PagerHTML
        {
            get;
            private set;
        }
        public string Stastics
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // 绑定一周热词
            BindHotKeywords();

            if (Request["keyword"] == null)
            {
                return;
            }

            string keyword = Request["keyword"].ToString();
            // 绑定搜索结果
            BindPagerHtml(keyword);
            // 添加搜索记录
            AddSearchLog(keyword);
        }

        private void BindHotKeywords()
        {
            SearchLogStasticsService statService = new SearchLogStasticsService();
            var hotKeywords = statService.GetHotKeyword();

            rptHotList.DataSource = hotKeywords;
            rptHotList.DataBind();
        }

        private void BindPagerHtml(string keyword)
        {
            // 初始化自定义分页控件
            var pager = new SimplePager();
            pager.UrlFormat = "Search.aspx?pagenum={n}&keyword=" + Server.UrlEncode(keyword);
            pager.PageSize = 10;
            // 解析当前页码
            pager.TryParseCurrentPageIndex(Request["pagenum"]);
            // 开始计时
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // 计算起始行
            int startRowIndex = (pager.CurrentPageIndex - 1) * pager.PageSize;
            // 通过逻辑层取得分页
            int totalCount;
            BindSearchResult(keyword, startRowIndex, 10, out totalCount);
            // 停止计时
            stopwatch.Stop();
            // 显示统计结果
            Stastics = String.Format("找到相关网页约{0}篇，用时{1}毫秒", totalCount, stopwatch.ElapsedMilliseconds);
            // 渲染页面HTML代码
            pager.TotalCount = totalCount;
            PagerHTML = pager.Render();
        }

        private void AddSearchLog(string keyword)
        {
            SearchLog searchLog = new SearchLog();
            searchLog.Id = Guid.NewGuid();
            searchLog.SearchDate = DateTime.Now;
            searchLog.Word = keyword;

            SearchLogService searchLogService = new SearchLogService();
            searchLogService.Add(searchLog);
        }

        private void BindSearchResult(string keyword, int startIndex, int pageSize, out int totalCount)
        {
            string indexPath = Context.Server.MapPath("~/Index"); // 索引文档保存位置
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);

            #region v1.0 单条件查询
            //// 查询条件
            //PhraseQuery query = new PhraseQuery();
            //// 分词后加入查询
            //IEnumerable<string> keyList = SplitHelper.SplitWords(keyword);
            //foreach (var key in keyList)
            //{
            //    query.Add(new Term("msg", key));
            //}
            //// 两个词的距离大于100（经验值）就不放入搜索结果，因为距离太远相关度就不高了
            //query.SetSlop(100); 
            #endregion

            #region v2.0 多条件查询
            IEnumerable<string> keyList = SplitHelper.SplitWords(keyword);

            PhraseQuery queryTitle = new PhraseQuery();
            foreach (var key in keyList)
            {
                queryTitle.Add(new Term("title", key));
            }
            queryTitle.SetSlop(100);

            PhraseQuery queryMsg = new PhraseQuery();
            foreach (var key in keyList)
            {
                queryMsg.Add(new Term("msg", key));
            }
            queryMsg.SetSlop(100);

            BooleanQuery query = new BooleanQuery();
            query.Add(queryTitle, BooleanClause.Occur.SHOULD); // SHOULD => 可以有，但不是必须的
            query.Add(queryTitle, BooleanClause.Occur.SHOULD); // SHOULD => 可以有，但不是必须的
            #endregion

            // TopScoreDocCollector:盛放查询结果的容器
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            // 使用query这个查询条件进行搜索，搜索结果放入collector
            searcher.Search(query, null, collector);
            // 首先获取总条数
            totalCount = collector.GetTotalHits();
            // 从查询结果中取出第m条到第n条的数据
            ScoreDoc[] docs = collector.TopDocs(startIndex, pageSize).scoreDocs;
            // 遍历查询结果
            IList<SearchResult> resultList = new List<SearchResult>();
            for (int i = 0; i < docs.Length; i++)
            {
                // 拿到文档的id，因为Document可能非常占内存（DataSet和DataReader的区别）
                int docId = docs[i].doc;
                // 所以查询结果中只有id，具体内容需要二次查询
                // 根据id查询内容：放进去的是Document，查出来的还是Document
                Document doc = searcher.Doc(docId);
                SearchResult result = new SearchResult();
                result.Url = "ViewArticle.aspx?id=" + doc.Get("id");
                result.Title = HighlightHelper.HighLight(keyword, doc.Get("title"));
                result.Msg = HighlightHelper.HighLight(keyword, doc.Get("msg")) + "......";

                resultList.Add(result);
            }

            // 绑定到Repeater
            rptSearchResult.DataSource = resultList;
            rptSearchResult.DataBind();
        }
    }
}