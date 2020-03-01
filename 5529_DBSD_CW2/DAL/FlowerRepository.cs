using _00005529_DBSD_CW2.Models;
using _00005529_DBSD_CW2.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace _00005529_DBSD_CW2.DAL
{
    public class FlowerRepository
    {
        public string ConnectionStr
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["Gullar"].ConnectionString;
            }
        }
        public IList<Flower> GetAllFlowers()
        {
            List<Flower> results = new List<Flower>();
            using (DbConnection connection = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandTimeout = 120;
                    cmd.CommandText = @"SELECT FlowerId, FlowerName, DeliveredDate, Color, Price FROM [dbo].[Flower]";
                    connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Flower cl = new Flower()
                            {
                                FlowerId = reader.GetInt32(0),
                                FlowerName = reader.GetString(1),
                                DeliveredDate = reader.GetDateTime(2),
                                Color = reader.GetString(3),
                                Price=reader.GetInt32(4)

                            };
                            results.Add(cl);
                        }
                    }
                }
            }
            return results;
        }

        public IList<Flower> GetAllFlowersPaged(DateTime? DeliveredDateFilter, string sortField, int page, int pageSize, out int totalItemsCount, string ColorFilter, string FlowerNameFilter, string sortDate, string sortPrice)
        {
            List<Flower> results = new List<Flower>();
            using (var connection = new SqlConnection(ConnectionStr))
            {
                using (var cmd = connection.CreateCommand())
                {
                    string sql = @"SELECT Flower.FlowerId,  Flower.FlowerName,Flower.DeliveredDate,Flower.Color, Flower.Price
                                      FROM Flower";

                    string filter = "";
                    if (!string.IsNullOrWhiteSpace(FlowerNameFilter))
                    {
                        filter += (string.IsNullOrEmpty(filter) ? " WHERE " : " AND ")
                                    + " FlowerName LIKE @FlowerName + '%' ";
                        cmd.Parameters.AddWithValue("@FlowerName", FlowerNameFilter);
                    }
                    if (DeliveredDateFilter.HasValue)
                    {
                        filter += (string.IsNullOrEmpty(filter) ? " WHERE " : " AND ")
                            + " DeliveredDate >= @DeliveredDate ";
                        cmd.Parameters.AddWithValue("@DeliveredDate", DeliveredDateFilter);
                    }
                    if (!string.IsNullOrWhiteSpace(ColorFilter))
                    {
                        filter += (string.IsNullOrEmpty(filter) ? " WHERE " : " AND ")
                            + " Color = @ColorFilter ";
                        cmd.Parameters.AddWithValue("@ColorFilter", ColorFilter);
                    }

                    string sort = " ORDER BY FlowerId";
                    if (!string.IsNullOrEmpty(sortDate))
                    {
                        sort = " ORDER BY " + sortDate.Replace("_desc", "") + (sortDate.EndsWith("_desc") ? " DESC " : " ASC ");
                    }
                    if (!string.IsNullOrEmpty(sortPrice))
                    {
                        sort = " ORDER BY " + sortPrice.Replace("_desc", "") + (sortPrice.EndsWith("_desc") ? " DESC " : " ASC ");
                    }
                    if (!string.IsNullOrEmpty(sortField))
                    {
                        sort = " ORDER BY " + sortField.Replace("_desc", "") + (sortField.EndsWith("_desc") ? " DESC " : " ASC ");
                    }
                    string pagingSql = " OFFSET @RowsOffset ROWS FETCH NEXT @PageSize ROWS ONLY ;";
                    //params for paging
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    cmd.Parameters.AddWithValue("@RowsOffset", (page - 1) * pageSize);


                    cmd.CommandText = sql + filter + sort + pagingSql;
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Flower cl = new Flower()
                            {
                                FlowerId = reader.GetInt32(reader.GetOrdinal("FlowerId")),
                                FlowerName = reader.GetString(reader.GetOrdinal("FlowerName")),
                                DeliveredDate = reader.GetDateTime(reader.GetOrdinal("DeliveredDate")),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                Price = reader.GetInt32(reader.GetOrdinal("Price"))

                            };
                            results.Add(cl);
                        }
                    }
                }
            }

            totalItemsCount = 20;
            return results;
        }


        public Flower GetFlowerByID(int FlowerId)
        {
            IList<Flower> clientList = new List<Flower>();
            Flower cl = null;
            using (var connection = new SqlConnection(ConnectionStr))
            {
                using (var cmd = connection.CreateCommand())
                {

                    cmd.CommandText = @"SELECT Flower.FlowerId,  Flower.FlowerName,Flower.DeliveredDate,Flower.Color, Flower.Price
                                      FROM Flower
                                      WHERE FlowerId = @FlowerId";

                    cmd.Parameters.AddWithValue("@FlowerId", FlowerId);
                    connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cl = new Flower()
                            {
                                FlowerId = reader.GetInt32(reader.GetOrdinal("FlowerId")),
                                FlowerName = reader.GetString(reader.GetOrdinal("FlowerName")),
                                DeliveredDate = reader.GetDateTime(reader.GetOrdinal("DeliveredDate")),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                Price = reader.GetInt32(reader.GetOrdinal("Price"))

                            };
                        }
                    }
                }
            }
            return cl;
        }
    }



}
