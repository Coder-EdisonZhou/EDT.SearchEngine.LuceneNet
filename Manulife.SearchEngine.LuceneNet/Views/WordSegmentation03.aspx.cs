using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using System;
using System.IO;

namespace Manulife.SearchEngine.LuceneNet.Views
{
    public partial class WordSegmentation03 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGetSegmentation_Click(object sender, EventArgs e)
        {
            string words = txtWords.Text;
            if (string.IsNullOrEmpty(words))
            {
                return;
            }

            Analyzer analyzer = new PanGuAnalyzer(); // CJK分词 → 二元分词
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(words));
            Token token = null;

            while ((token = tokenStream.Next()) != null) // 只要还有词，就不返回null
            {
                string word = token.TermText(); // token.TermText() 取得当前分词
                Response.Write(word + "   |  ");
            }
        }
    }
}