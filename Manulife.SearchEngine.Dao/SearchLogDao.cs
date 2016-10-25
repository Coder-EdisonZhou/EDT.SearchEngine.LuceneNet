using Manulife.SearchEngine.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Manulife.SearchEngine.Dao
{
    public class SearchLogDao
    {
        public SearchLog Add
            (SearchLog searchLog)
        {
            string sql = "INSERT INTO T_SearchLogs (Id, Word, SearchDate)  VALUES (@Id, @Word, @SearchDate)";
            SqlParameter[] para = new SqlParameter[]
                {
                        new SqlParameter("@Id", ToDBValue(searchLog.Id)),
                        new SqlParameter("@Word", ToDBValue(searchLog.Word)),
                        new SqlParameter("@SearchDate", ToDBValue(searchLog.SearchDate)),
                };
            SqlHelper.ExecuteNonQuery(sql, para);
            return searchLog;
        }

        public int DeleteById(Guid id)
        {
            string sql = "DELETE T_SearchLogs WHERE Id = @Id";

            SqlParameter[] para = new SqlParameter[]
             {
                new SqlParameter("@Id", id)
             };

            return SqlHelper.ExecuteNonQuery(sql, para);
        }

        public int Delete(SearchLog searchLog)
        {
            return DeleteById(searchLog.Id);
        }

        public int Update(SearchLog searchLog)
        {
            string sql =
                "UPDATE T_SearchLogs " +
                "SET " +
            " Word = @Word"
                + ", SearchDate = @SearchDate"

            + " WHERE Id = @Id";


            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@Id", searchLog.Id)
                    ,new SqlParameter("@Word", ToDBValue(searchLog.Word))
                    ,new SqlParameter("@SearchDate", ToDBValue(searchLog.SearchDate))
            };

            return SqlHelper.ExecuteNonQuery(sql, para);
        }

        public SearchLog GetById(Guid id)
        {
            string sql = "SELECT * FROM T_SearchLogs WHERE Id = @Id";
            using (SqlDataReader reader = SqlHelper.ExecuteDataReader(sql, new SqlParameter("@Id", id)))
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

        public SearchLog ToModel(SqlDataReader reader)
        {
            SearchLog searchLog = new SearchLog();

            searchLog.Id = (Guid)ToModelValue(reader, "Id");
            searchLog.Word = (string)ToModelValue(reader, "Word");
            searchLog.SearchDate = (DateTime)ToModelValue(reader, "SearchDate");
            return searchLog;
        }

        public int GetTotalCount()
        {
            string sql = "SELECT count(*) FROM T_SearchLogs";
            return (int)SqlHelper.ExecuteScalar(sql);
        }

        public IEnumerable<SearchLog> GetPagedData(int minrownum, int maxrownum)
        {
            var list = new List<SearchLog>();
            string sql = "SELECT * from(SELECT *,row_number() over(order by Id) rownum FROM T_SearchLogs) t where rownum>=@minrownum and rownum<=@maxrownum";
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

        public IEnumerable<SearchLog> GetAll()
        {
            var list = new List<SearchLog>();
            string sql = "SELECT * FROM T_SearchLogs";
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
