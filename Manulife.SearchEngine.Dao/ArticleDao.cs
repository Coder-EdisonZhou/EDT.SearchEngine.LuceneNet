using Manulife.SearchEngine.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Manulife.SearchEngine.Dao
{
    public class ArticleDao
    {
        public Article Add(Article Article)
        {
            string sql = "INSERT INTO T_Articles (Title, Msg)  output inserted.Id VALUES (@Title, @Msg)";
            SqlParameter[] para = new SqlParameter[]
                {
                        new SqlParameter("@Title", ToDBValue(Article.Title)),
                        new SqlParameter("@Msg", ToDBValue(Article.Msg)),
                };

            long newId = (long)SqlHelper.ExecuteScalar(sql, para);
            return GetById(newId);
        }

        public int DeleteById(long id)
        {
            string sql = "DELETE T_Articles WHERE Id = @Id";

            SqlParameter[] para = new SqlParameter[]
             {
                new SqlParameter("@Id", id)
             };

            return SqlHelper.ExecuteNonQuery(sql, para);
        }

        public int Delete(Article Article)
        {
            return DeleteById(Article.Id);
        }

        public int Update(Article Article)
        {
            string sql =
                "UPDATE T_Articles " +
                "SET " +
            " Title = @Title"
                + ", Msg = @Msg"
            + " WHERE Id = @Id";


            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@Id", Article.Id)
                    ,new SqlParameter("@Title", ToDBValue(Article.Title))
                    ,new SqlParameter("@Msg", ToDBValue(Article.Msg))
            };

            return SqlHelper.ExecuteNonQuery(sql, para);
        }

        public Article GetById(long id)
        {
            string sql = "SELECT * FROM T_Articles WHERE Id = @Id";
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

        public Article ToModel(SqlDataReader reader)
        {
            Article Article = new Article();

            Article.Id = (long)ToModelValue(reader, "Id");
            Article.Title = (string)ToModelValue(reader, "Title");
            Article.Msg = (string)ToModelValue(reader, "Msg");
            return Article;
        }

        public int GetTotalCount()
        {
            string sql = "SELECT count(*) FROM T_Articles";
            return (int)SqlHelper.ExecuteScalar(sql);
        }

        public IEnumerable<Article> GetPagedData(int minrownum, int maxrownum)
        {
            var list = new List<Article>();
            string sql = "SELECT * from(SELECT *,row_number() over(order by Id) rownum FROM T_Articles) t where rownum>=@minrownum and rownum<=@maxrownum";
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

        public IEnumerable<Article> GetAll()
        {
            var list = new List<Article>();
            string sql = "SELECT * FROM T_Articles";
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
