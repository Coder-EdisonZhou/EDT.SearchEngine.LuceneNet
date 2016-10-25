using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manulife.SearchEngine.Model
{
    [Serializable()]
    public class SearchLog
    {
        public Guid Id
        {
            get;
            set;
        }
        public string Word
        {
            get;
            set;
        }
        public DateTime SearchDate
        {
            get;
            set;
        }
    }
}
