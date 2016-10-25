using Manulife.SearchEngine.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Manulife.SearchEngine.Dao
{
    public class SearchLogStasticsDao
    {
        public SearchLogStastics Add
            (SearchLogStastics searchLog)
        {
            string sql = "INSERT INTO T_SearchLogStastics (Word, SearchCount)  VALUES (@Word, @SearchCount)";
            SqlParameter[] para = new SqlParameter[]
                {
                        new SqlParameter("@Word", ToDBValue(searchLog.Word)),
                        new SqlParameter("@SearchCount", ToDBValue(searchLog.SearchCount)),
                };
            SqlHelper.ExecuteNonQuery(sql, para);
            return searchLog;
        }

        public int DeleteByWord(string word)
        {
            string sql = "DELETE T_SearchLogStastics WHERE Word = @Word";

            SqlParameter[] para = new SqlParameter[]
             {
                new SqlParameter("@Word", word)
             };

            return SqlHelper.ExecuteNonQuery(sql, para);
        }

        public int Delete()
        {
            string sql = "DELETE FROM T_SearchLogStastics";

            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int Update(SearchLogStastics searchLog)
        {
            string sql =
                "UPDATE T_SearchLogStastics " +
                "SET " +
                "SearchCount = @SearchCount"
                + " WHERE Word = @Word";

            SqlParameter[] para = new SqlParameter[]
            {
                    new SqlParameter("@Word", ToDBValue(searchLog.Word)),
                    new SqlParameter("@SearchCount", ToDBValue(searchLog.SearchCount))
            };

            return SqlHelper.ExecuteNonQuery(sql, para);
        }

        public int Stastic()
        {
            string sql = @"insert into T_SearchLogStastics(Word,SearchCount)
                    select word,count(*) 
                    from T_SearchLogs
                    where SearchDate>=DateAdd(day,-7,getdate())
                    group by word;";

            return SqlHelper.ExecuteNonQuery(sql);
        }

        public DataTable GetSuggestion(string pattern)
        {
            string sql = @"select top 5 Word,SearchCount from T_SearchLogStastics where Word like @wordpattern"
                        + " order by SearchCount desc";
            SqlParameter para = new SqlParameter("@wordpattern", pattern + "%");

            return SqlHelper.ExecuteDataTable(sql,para);
        }

        public DataTable GetHotKeyword()
        {
            string sql = @"select top 5 Word,SearchCount from T_SearchLogStastics"
                        + " order by SearchCount desc";

            return SqlHelper.ExecuteDataTable(sql);
        }

        public SearchLogStastics GetById(string word)
        {
            string sql = "SELECT * FROM T_SearchLogStastics WHERE Word = @Word";
            using (SqlDataReader reader = SqlHelper.ExecuteDataReader(sql, new SqlParameter("@Word", word)))
            {
                if (reader.Read())
                {
                    return ToModel(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public SearchLogStastics ToModel(SqlDataReader reader)
        {
            SearchLogStastics searchLog = new SearchLogStastics();

            searchLog.Word = (string)ToModelValue(reader, "Word");
            searchLog.SearchCount = (long)ToModelValue(reader, "SearchCount");
            return searchLog;
        }

        public int GetTotalCount()
        {
            string sql = "SELECT count(*) FROM T_SearchLogStastics";
            return (int)SqlHelper.ExecuteScalar(sql);
        }

        public IEnumerable<SearchLogStastics> GetPagedData(int minrownum, int maxrownum)
        {
            var list = new List<SearchLogStastics>();
            string sql = "SELECT * from(SELECT *,row_number() over(order by Id) rownum FROM T_SearchLogStastics) t where rownum>=@minrownum and rownum<=@maxrownum";
            using (SqlDataReader reader = SqlHelper.ExecuteDataReader(sql,
                new SqlParameter("@minrownum", minrownum),
                new SqlParameter("@maxrownum", maxrownum)))
            {
                while (reader.Read())
                {
                    list.Add(ToModel(reader));
                }
            }
            return list;
        }

        public IEnumerable<SearchLogStastics> GetAll()
        {
            var list = new List<SearchLogStastics>();
            string sql = "SELECT * FROM T_SearchLogStastics";
            using (SqlDataReader reader = SqlHelper.ExecuteDataReader(sql))
            {
                while (reader.Read())
                {
                    list.Add(ToModel(reader));
                }
            }
            return list;
        }

        public object ToDBValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        public object ToModelValue(SqlDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return null;
            }
            else
            {
                return reader[columnName];
            }
        }
    }
}
