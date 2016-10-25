using log4net;
using Manulife.SearchEngine.Model;
using Manulife.SearchEngine.Service;
using System.Collections.Generic;
using System.Threading;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System.IO;
using System.Web.Hosting;

namespace Manulife.SearchEngine.Web.Common
{
    public class IndexManager
    {
        public static readonly IndexManager Instance = new IndexManager();
        private static readonly string IndexPath = HostingEnvironment.MapPath("~/Index");
        private static ILog log = LogManager.GetLogger(typeof(IndexManager));

        private IndexManager()
        { }

        static IndexManager()
        { }

        public void Start()
        {
            Thread thread = new Thread(WatchIndexTask);
            thread.IsBackground = true;
            thread.Start();
            log.Debug("IndexManager has been lunched successfully!");
        }

        private Queue<IndexTask> indexQueue = new Queue<IndexTask>();
        private void WatchIndexTask()
        {
            while (true)
            {
                if (indexQueue.Count > 0)
                {
                    // 索引文档保存位置
                    FSDirectory directory = FSDirectory.Open(new DirectoryInfo(IndexPath), new NativeFSLockFactory());
                    bool isUpdate = IndexReader.IndexExists(directory); //判断索引库是否存在
                    log.Debug(string.Format("The status of index : {0}", isUpdate));
                    if (isUpdate)
                    {
                        //  如果索引目录被锁定（比如索引过程中程序异常退出），则首先解锁
                        //  Lucene.Net在写索引库之前会自动加锁，在close的时候会自动解锁
                        //  不能多线程执行，只能处理意外被永远锁定的情况
                        if (IndexWriter.IsLocked(directory))
                        {
                            log.Debug("The index is existed, need to unlock.");
                            IndexWriter.Unlock(directory);  //unlock:强制解锁，待优化
                        }
                    }
                    //  创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)
                    //  补充:使用IndexWriter打开directory时会自动对索引库文件上锁
                    IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate,
                        IndexWriter.MaxFieldLength.UNLIMITED);
                    log.Debug(string.Format("Total number of task : {0}", indexQueue.Count));

                    while (indexQueue.Count > 0)
                    {
                        IndexTask task = indexQueue.Dequeue();
                        long id = task.TaskId;
                        ArticleService articleService = new ArticleService();
                        Article article = articleService.GetById(id);

                        if (article == null)
                        {
                            continue;
                        }

                        //  一条Document相当于一条记录
                        Document document = new Document();
                        //  每个Document可以有自己的属性（字段），所有字段名都是自定义的，值都是string类型
                        //  Field.Store.YES不仅要对文章进行分词记录，也要保存原文，就不用去数据库里查一次了
                        document.Add(new Field("id", id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        //  需要进行全文检索的字段加 Field.Index. ANALYZED
                        //  Field.Index.ANALYZED:指定文章内容按照分词后结果保存，否则无法实现后续的模糊查询 
                        //  WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词，还保存词之间的距离
                        document.Add(new Field("title", article.Title, Field.Store.YES, Field.Index.ANALYZED,
                            Field.TermVector.WITH_POSITIONS_OFFSETS));
                        document.Add(new Field("msg", article.Msg, Field.Store.YES, Field.Index.ANALYZED,
                            Field.TermVector.WITH_POSITIONS_OFFSETS));
                        if (task.TaskType != TaskTypeEnum.Add)
                        {
                            //  防止重复索引，如果不存在则删除0条
                            writer.DeleteDocuments(new Term("id", id.ToString()));// 防止已存在的数据 => delete from t where id=i
                        }

                        //  把文档写入索引库
                        writer.AddDocument(document);

                        log.Debug(string.Format("Index {0} has been writen to index library!", id.ToString()));
                    }

                    writer.Close(); // Close后自动对索引库文件解锁
                    directory.Close();  //  不要忘了Close，否则索引结果搜不到

                    log.Debug("The index library has been closed!");
                }
                else
                {
                    Thread.Sleep(2000);
                }
            }
        }

        public void AddArticle(IndexTask task)
        {
            task.TaskType = TaskTypeEnum.Add;
            indexQueue.Enqueue(task);
        }

        public void UpdateArticle(IndexTask task)
        {
            task.TaskType = TaskTypeEnum.Update;
            indexQueue.Enqueue(task);
        }
    }

    public class IndexTask
    {
        public long TaskId { get; set; }

        public TaskTypeEnum TaskType { get; set; }
    }

    public enum TaskTypeEnum
    {
        Add,
        Update
    }
}
