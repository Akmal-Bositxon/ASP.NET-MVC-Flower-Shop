using _00005529_DBSD_CW2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace _00005529_DBSD_CW2.DAL
{
    public class WishlistRepository
    {
        public string ConnectionStr
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["Gullar"].ConnectionString;
            }
        }

        public void CreateWishList(Wishlist book, int flid, int usid)
        {
            using (var connection = new SqlConnection(ConnectionStr))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"udpCreateWishList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Customer_ID", usid);
                    cmd.Parameters.AddWithValue("@FlowerId", flid);
                    cmd.Parameters.AddWithValue("@NumberOfFlowers", book.NumberOfFlowers);
                    cmd.Parameters.AddWithValue("@comment", book.Comments);
                    cmd.Parameters.AddWithValue("@CreatedDate", book.CreatedDate);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }
        public IList<Wishlist> GetAllWishlist(int page, int pageSize, out int totalItemsCount, int userid)
        {
            List<Wishlist> results = new List<Wishlist>();
            using (var connection = new SqlConnection(ConnectionStr))
            {
                using (var cmd = connection.CreateCommand()) {

                    string sql = @"SELECT f.FlowerId, f.FlowerName,f.Color, f.Price, w.CreatedDate, w.NumberOfFlowers,w.comment,w.WishId
                                      FROM WishList w LEFT JOIN Flower f ON f.FlowerId = w.FlowerId Where w.Customer_ID=" + userid + " ";


                    //string pagingSql = " OFFSET @RowsOffset ROWS FETCH NEXT @PageSize ROWS ONLY ;";
                    //params for paging
                    //cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    //cmd.Parameters.AddWithValue("@RowsOffset", (page - 1) * pageSize);


                    cmd.CommandText = sql;
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Wishlist book = new Wishlist();
                            book.WishId = reader.GetInt32(7);
                            book.Customer_id = userid;
                            book.FlowerId = reader.GetInt32(0);
                            book.CreatedDate = reader.GetDateTime(4);
                            book.NumberOfFlowers = reader.GetInt32(5);
                            book.Comments = reader.GetString(6);
                            book.Flow = new Flower();
                            book.Flow.FlowerId = reader.GetInt32(0);
                            book.Flow.FlowerName = reader.GetString(1);
                            book.Flow.Color = reader.GetString(2);
                            book.Flow.Price = reader.GetInt32(3);

                            results.Add(book);
                        }
                    }
                }
            }

            totalItemsCount = 20;
            return results;
        }
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(ConnectionStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM WishList WHERE WishId = @WishId";
                    cmd.Parameters.AddWithValue("@WishId", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public IList<Report> GenereateReport()
        {
          
            var result = new List<Report>();
            using (var connection = new SqlConnection(ConnectionStr))
            {
                using (var cmd = connection.CreateCommand())
                {
                
                    string sql = @"SELECT f.Customer_ID,f.username,f.FirstName,f.LastName, count(w.WishId) as [wishes] 
FROM WishList w LEFT JOIN Customer f ON f.Customer_ID = w.Customer_ID Group By f.Customer_ID,f.username,f.FirstName,f.LastName";


            


                    cmd.CommandText = sql;
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Report book = new Report();
                            book.FirstName = reader.GetString(2);
                            book.username = reader.GetString(1);
                            book.LastName = reader.GetString(3);
                            book.WishListCount = reader.GetInt32(4);

                            result.Add(book);
                        }
                    }
                }
            }

  

            return result;
        }

    } 
    }