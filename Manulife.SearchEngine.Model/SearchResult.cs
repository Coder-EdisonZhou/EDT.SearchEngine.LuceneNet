using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manulife.SearchEngine.Model
{
    [Serializable()]
    public class SearchResult
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Msg { get; set; }
    }
}
