using Manulife.SearchEngine.Model;
using Manulife.SearchEngine.Dao;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System;

namespace Manulife.SearchEngine.Service
{
    public class SearchLogStasticsService
    {
        public SearchLogStastics Add(SearchLogStastics searchLog)
        {
            return new SearchLogStasticsDao().Add(searchLog);
        }

        public int DeleteById(string word)
        {
            return new SearchLogStasticsDao().DeleteByWord(word);
        }

        public int Delete()
        {
            return new SearchLogStasticsDao().Delete();
        }
        public int Update(SearchLogStastics searchLog)
        {
            return new SearchLogStasticsDao().Update(searchLog);
        }


        public SearchLogStastics GetById(string word)
        {
            return new SearchLogStasticsDao().GetById(word);
        }
        public int GetTotalCount()
        {
            return new SearchLogStasticsDao().GetTotalCount();
        }

        public IEnumerable<SearchLogStastics> GetPagedData(int minrownum, int maxrownum)
        {
            return new SearchLogStasticsDao().GetPagedData(minrownum, maxrownum);
        }

        public IEnumerable<SearchLogStastics> GetAll()
        {
            return new SearchLogStasticsDao().GetAll();
        }

        public int Stastic()
        {
            return new SearchLogStasticsDao().Stastic();
        }

        public DataTable GetSuggestion(string pattern)
        {
            return new SearchLogStasticsDao().GetSuggestion(pattern);
        }

        public DataTable GetHotKeyword()
        {
            // 首先判断缓存中是否有记录
            var cacheData = HttpRuntime.Cache["HotKeywords"];
            if (cacheData == null)
            {
                var hotKeywords = new SearchLogStasticsDao().GetHotKeyword();
                // 将结果放入缓存，并设定1小时替换一次缓存
                HttpRuntime.Cache.Insert("HotKeywords",hotKeywords,null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                return hotKeywords;
            }
            else
            {
                return cacheData as DataTable;
            }
        }
    }
}
