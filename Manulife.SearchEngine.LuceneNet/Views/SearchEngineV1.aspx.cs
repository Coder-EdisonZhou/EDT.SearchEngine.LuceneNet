using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Manulife.SearchEngine.LuceneNet.Common;
using Manulife.SearchEngine.LuceneNet.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Manulife.SearchEngine.LuceneNet.Views
{
    public partial class SearchEngineV1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 检查是否已存在生成的索引文件
                //CheckIndexData();
            }
        }

        private void CheckIndexData()
        {
            string indexPath = Context.Server.MapPath("~/Index"); // 索引文档保存位置
            var files = System.IO.Directory.GetFiles(indexPath);
            if (files.Length > 0)
            {
                btnCreateIndex.Visible = false;
                lblIndexStatus.Text = "A Simple Search Engine";
                lblIndexStatus.Visible = true;
            }
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        protected void btnCreateIndex_Click(object sender, EventArgs e)
        {
            string indexPath = Context.Server.MapPath("~/Index"); // 索引文档保存位置
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            bool isUpdate = IndexReader.IndexExists(directory); //判断索引库是否存在
            if (isUpdate)
            {
                //  如果索引目录被锁定（比如索引过程中程序异常退出），则首先解锁
                //  Lucene.Net在写索引库之前会自动加锁，在close的时候会自动解锁
                //  不能多线程执行，只能处理意外被永远锁定的情况
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);  //unlock:强制解锁，待优化
                }
            }
            //  创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)
            //  补充:使用IndexWriter打开directory时会自动对索引库文件上锁
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate,
                IndexWriter.MaxFieldLength.UNLIMITED);

            for (int i = 1000; i < 1100; i++)
            {
                string txt = File.ReadAllText(Context.Server.MapPath("~/Upload/Articles/") + i + ".txt");
                //  一条Document相当于一条记录
                Document document = new Document();
                //  每个Document可以有自己的属性（字段），所有字段名都是自定义的，值都是string类型
                //  Field.Store.YES不仅要对文章进行分词记录，也要保存原文，就不用去数据库里查一次了
                document.Add(new Field("id", i.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                //  需要进行全文检索的字段加 Field.Index. ANALYZED
                //  Field.Index.ANALYZED:指定文章内容按照分词后结果保存，否则无法实现后续的模糊查询 
                //  WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词，还保存词之间的距离
                document.Add(new Field("msg", txt, Field.Store.YES, Field.Index.ANALYZED,
                    Field.TermVector.WITH_POSITIONS_OFFSETS));
                //  防止重复索引，如果不存在则删除0条
                writer.DeleteDocuments(new Term("id", i.ToString()));// 防止已存在的数据 => delete from t where id=i
                //  把文档写入索引库
                writer.AddDocument(document);
                Console.WriteLine("索引{0}创建完毕", i.ToString());
            }

            writer.Close(); // Close后自动对索引库文件解锁
            directory.Close();  //  不要忘了Close，否则索引结果搜不到

            lblIndexStatus.Text = "索引文件创建成功！";
            lblIndexStatus.Visible = true;
            btnCreateIndex.Enabled = false;
        }

        /// <summary>
        /// 获取搜索结果
        /// </summary>
        protected void btnGetSearchResult_Click(object sender, EventArgs e)
        {
            string keyword = txtKeyWords.Text;

            string indexPath = Context.Server.MapPath("~/Index"); // 索引文档保存位置
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            // 查询条件
            PhraseQuery query = new PhraseQuery();
            // 等同于 where contains("msg",kw)
            query.Add(new Term("msg", keyword));
            // 两个词的距离大于100（经验值）就不放入搜索结果，因为距离太远相关度就不高了
            query.SetSlop(100);
            // TopScoreDocCollector:盛放查询结果的容器
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            // 使用query这个查询条件进行搜索，搜索结果放入collector
            searcher.Search(query, null, collector);
            // 从查询结果中取出第m条到第n条的数据
            // collector.GetTotalHits()表示总的结果条数
            ScoreDoc[] docs = collector.TopDocs(0, collector.GetTotalHits()).scoreDocs;
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
                result.Id = Convert.ToInt32(doc.Get("id"));
                result.Msg = HighlightHelper.HighLight(keyword, doc.Get("msg"));

                resultList.Add(result);
            }

            // 绑定到Repeater
            rptSearchResult.DataSource = resultList;
            rptSearchResult.DataBind();
        }
    }
}