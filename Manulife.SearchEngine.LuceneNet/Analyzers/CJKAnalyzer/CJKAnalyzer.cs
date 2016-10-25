using System.Collections;
using System.IO;
using Lucene.Net.Analysis;

namespace Manulife.SearchEngine.LuceneNet.Analyzers
{
    /**/
    /**
 * Filters CJKTokenizer with StopFilter.
 *
 * @author Che, Dong
 */
    public class CJKAnalyzer : Analyzer
    {
        //~ Static fields/initializers ---------------------------------------------

        /**/
        /**
     * An array containing some common English words that are not usually
     * useful for searching and some double-byte interpunctions.
     */
        public static string[] STOP_WORDS = {
                                                 "a", "and", "are", "as", "at", "be",
                                                 "but", "by", "for", "if", "in",
                                                 "into", "is", "it", "no", "not",
                                                 "of", "on", "or", "s", "such", "t",
                                                 "that", "the", "their", "then",
                                                 "there", "these", "they", "this",
                                                 "to", "was", "will", "with", "",
                                                 "www"
                                             };

        //~ Instance fields --------------------------------------------------------

        /**/
        /**
     * stop word list
     */
        private Hashtable stopTable;

        //~ Constructors -----------------------------------------------------------

        /**/
        /**
     * Builds an analyzer which removes words in {@link #STOP_WORDS}.
     */
        public CJKAnalyzer()
        {
            stopTable = StopFilter.MakeStopSet(STOP_WORDS);
        }

        /**/
        /**
     * Builds an analyzer which removes words in the provided array.
     *
     * @param stopWords stop word array
     */
        public CJKAnalyzer(string[] stopWords)
        {
            stopTable = StopFilter.MakeStopSet(stopWords);
        }

        //~ Methods ----------------------------------------------------------------

        /**/
        /**
     * get token stream from input
     *
     * @param fieldName lucene field name
     * @param reader    input reader
     * @return TokenStream
     */
        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            TokenStream ts = new CJKTokenizer(reader);
            return new StopFilter(ts, stopTable);
            //return new StopFilter(new CJKTokenizer(reader), stopTable);
        }
    }
}

