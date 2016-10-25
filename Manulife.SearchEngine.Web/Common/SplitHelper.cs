using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using System;
using System.Collections.Generic;
using System.IO;

namespace Manulife.SearchEngine.Web.Common
{
    public static class SplitHelper
    {
        /// <summary>
        /// 对keyword进行分词，将分词的结果返回
        /// </summary>
        public static IEnumerable<string> SplitWords(string keyword)
        {
            IList<string> list = new List<string>();
            Analyzer analyzer = new PanGuAnalyzer();
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(keyword));
            Token token = null;

            while ((token = tokenStream.Next()) != null)
            {
                // token.TermText()为当前分的词
                string word = token.TermText();
                list.Add(word);
            }

            return list;
        }
    }
}
