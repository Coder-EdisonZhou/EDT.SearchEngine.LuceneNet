using PanGu;
using PanGu.HighLight;
using System;

namespace Manulife.SearchEngine.LuceneNet.Common
{
    public static class HighlightHelper
    {
        /// <summary>
        /// 搜索结果高亮显示
        /// PS:需要添加PanGu.HighLight.dll的引用
        /// </summary>
        /// <param name="keyword"> 关键字 </param>
        /// <param name="content"> 搜索结果 </param>
        /// <returns> 高亮后结果 </returns>
        public static string HighLight(string keyword, string content)
        {
            // 创建HTMLFormatter,参数为高亮单词的前后缀
            SimpleHTMLFormatter simpleHTMLFormatter =
                new SimpleHTMLFormatter("<font style=\"font-style:normal;font-weight:bold;color:#cc0000;\"><b>", "</b></font>");
            // 创建 Highlighter ，输入HTMLFormatter 和 盘古分词对象Semgent
            Highlighter highlighter =
                            new Highlighter(simpleHTMLFormatter,
                            new Segment());
            // 设置每个摘要段的字符数
            highlighter.FragmentSize = 1000;
            // 获取最匹配的摘要段
            return highlighter.GetBestFragment(keyword, content);
        }
    }
}
