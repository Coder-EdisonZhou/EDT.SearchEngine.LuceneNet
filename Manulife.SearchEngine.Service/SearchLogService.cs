using Manulife.SearchEngine.Model;
using Manulife.SearchEngine.Dao;
using System.Collections.Generic;
using System;

namespace Manulife.SearchEngine.Service
{
    public class SearchLogService
    {
        public SearchLog Add(SearchLog searchLog)
        {
            return new SearchLogDao().Add(searchLog);
        }

        public int DeleteById(Guid id)
        {
            return new SearchLogDao().DeleteById(id);
        }

        public int Delete(SearchLog searchLog)
        {
            return new SearchLogDao().Delete(searchLog);
        }
        public int Update(SearchLog searchLog)
        {
            return new SearchLogDao().Update(searchLog);
        }


        public SearchLog GetById(Guid id)
        {
            return new SearchLogDao().GetById(id);
        }
        public int GetTotalCount()
        {
            return new SearchLogDao().GetTotalCount();
        }

        public IEnumerable<SearchLog> GetPagedData(int minrownum, int maxrownum)
        {
            return new SearchLogDao().GetPagedData(minrownum, maxrownum);
        }

        public IEnumerable<SearchLog> GetAll()
        {
            return new SearchLogDao().GetAll();
        }
    }
}
