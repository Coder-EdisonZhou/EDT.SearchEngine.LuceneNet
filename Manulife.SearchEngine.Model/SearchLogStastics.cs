using System;

namespace Manulife.SearchEngine.Model
{
    [Serializable()]
    public class SearchLogStastics
    {
        public string Word { get; set; }
        public long SearchCount { get; set; }
    }
}
