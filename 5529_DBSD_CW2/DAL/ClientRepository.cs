using _00005529_DBSD_CW2.DAL;
using _00005529_DBSD_CW2.Models;
using _5529_DBSD_CW2.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http.ModelBinding;

namespace _00005529_DBSD_CW2.DAL
{
    public class ClientRepository
    {
        public string ConnectionStr
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["Gullar"].ConnectionString;
            }
        }
        public IList<Client> GetAllClients()
        {
            List<Client> results = new List<Client>();
            using (DbConnection connection = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandTimeout = 120;
                    cmd.CommandText = @"SELECT [Customer_ID], [FirstName], [LastName], [DateOfBirth], [City], [Email], [password],[username] [Phone Number] FROM [dbo].[Customer]";
                    connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Client cl = new Client();
                            cl.Customer_ID = reader.GetInt32(0);
                            cl.FirstName = reader.GetString(1);
                            cl.LastName = reader.GetString(2);
                            cl.DateOfBirth = reader.GetDateTime(3);
                            cl.Country = reader.GetString(4);
                            cl.Email = reader.GetString(5);
                            cl.Password = reader.GetString(6);
                            if (!reader.IsDBNull(reader.GetOrdinal("username")))
                            {
                                cl.UserName = reader.GetString(reader.GetOrdinal("username"));
                            }
                            else
                            {
                                cl.UserName = null;
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("Phone Number")))
                            {
                                cl.PhoneNumber = reader.GetString(reader.GetOrdinal("Phone Number"));
                            }
                            else
                            {
                                cl.PhoneNumber = null;
                            }
                            results.Add(cl);
                        }
                    }
                }
            }
            return results;
        }

        public void CreateClient(Client cl, string comPassword)
        {
            using (var connection = new SqlConnection(ConnectionStr))
            {
                using (var cmd = connection.CreateCommand())
                {
                    if (comPassword == cl.Password)
                    {
                        cmd.CommandTimeout = 120;

                        cmd.CommandText = @"udpCreateClient";
                        cmd.CommandType = CommandType.StoredProcedure;
                   
                        cmd.Parameters.AddWithValue("@FirstName", cl.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", cl.LastName);
                        cmd.Parameters.AddWithValue("@DateOfBirth", cl.DateOfBirth);
                        cmd.Parameters.AddWithValue("@City", cl.Country);
                        cmd.Parameters.AddWithValue("@Email", cl.Email);
                        cmd.Parameters.AddWithValue("@password", cl.Password);
                        cmd.Parameters.AddWithValue("@UserName", cl.UserName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", cl.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Title", (object)cl.Title ?? DbType.String);
                        cmd.Parameters.AddWithValue("@Photo", (object)cl.Photo ?? SqlBinary.Null);
              
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void UpdateClient(Client cl)
        {

            using (DbConnection connection = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    //cmd.CommandText = @"UPDATE Customer SET [FirstName] = @FirstName, [LastName] = @LastName, [DateOfBirth] = @DateOfBirth,
                    //                  [City] = @City, [District] = @District,
                    //                  [Street] = @Street, [Email] = @Email, [password] = @password WHERE Customer_ID = @Customer_ID";
                    cmd.CommandText = @"udpUpdateClient";
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@Customer_ID";
                    p.DbType = DbType.Int32;
                    p.Value = cl.Customer_ID;
                    cmd.Parameters.Add(p);

                    p = cmd.CreateParameter();
                    p.ParameterName = "@LastName";
                    p.DbType = DbType.String;
                    p.Value = cl.LastName;
                    cmd.Parameters.Add(p);

                    p = cmd.CreateParameter();
                    p.ParameterName = "@FirstName";
                    p.DbType = DbType.String;
                    p.Value = cl.FirstName;
                    cmd.Parameters.Add(p);

                    p = cmd.CreateParameter();
                    p.ParameterName = "@DateOfBirth";
                    p.DbType = DbType.DateTime;
                    p.Value = cl.DateOfBirth;
                    cmd.Parameters.Add(p);

                    p = cmd.CreateParameter();
                    p.ParameterName = "@City";
                    p.DbType = DbType.String;
                    p.Value = cl.Country;
                    cmd.Parameters.Add(p);

                    p = cmd.CreateParameter();
                    p.ParameterName = "@Email";
                    p.DbType = DbType.String;
                    p.Value = cl.Email;
                    cmd.Parameters.Add(p);

                    p = cmd.CreateParameter();
                    p.ParameterName = "@password";
                    p.DbType = DbType.String;
                    p.Value = cl.Password;
                    cmd.Parameters.Add(p);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }
        public int GetClientByUsername(string Username)
        {
            int result=0;

            using (DbConnection connection = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandTimeout = 120;
                    cmd.CommandText = @"SELECT [Customer_ID], [FirstName], [LastName], [DateOfBirth], [City], [Email], [password],[username] [Phone Number] FROM [dbo].[Customer]
                                      WHERE username = @UserName";

                    cmd.AddParameter("@UserName", Username, DbType.String);
                    connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = reader.GetInt32(0);
                        }
                    }

                }
            }
            return result;
        }
        public Client GetClientByID(int ClientId)
        {
            IList<Client> clientList = new List<Client>();
            Client cl = null;
            using (DbConnection connection = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandTimeout = 120;
                    cmd.CommandText = @"SELECT [Customer_ID], [FirstName], [LastName], [DateOfBirth], [City], [Email], [password],[username], [Phone Number], [Photo] FROM [dbo].[Customer]
                                      WHERE Customer_ID = @Customer_ID";

                    cmd.AddParameter("@Customer_ID", ClientId, DbType.Int32);
                    connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cl = new Client();

                            cl.Customer_ID = reader.GetInt32(0);
                            cl.FirstName = reader.GetString(1);
                            cl.LastName = reader.GetString(2);
                            cl.DateOfBirth = reader.GetDateTime(3);
                            cl.Country = reader.GetString(4);
                            cl.Email = reader.GetString(5);
                            cl.Password = reader.GetString(6);
                            cl.Photo = reader.IsDBNull(reader.GetOrdinal("Photo")) ? null : (byte[])reader["Photo"];
                     


                            if (!reader.IsDBNull(reader.GetOrdinal("Phone Number")))
                            {
                                cl.PhoneNumber = reader.GetString(reader.GetOrdinal("Phone Number"));
                            }
                            else
                            {
                                cl.PhoneNumber = null;
                            }

                        }
                    }
                    
                }
            }
            return cl;
        }

        public IList<Client>FilterClients(string LastNameFilter, DateTime? DateOfBirthFilter, string CityFilter, string sortField, int page, int pageSize, out int totalItemsCount)
        {
            List<Client> results = new List<Client>();
            using (DbConnection connection = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    string sql = @"SELECT Customer.Customer_ID, Customer.FirstName, Customer.LastName, Customer.DateOfBirth, Customer.City, Customer.Email,Customer.password, Customer.username, Customer.[Phone Number]                                
                    FROM [dbo].[Customer]";
                                 //INNER JOIN PhoneNumber ON PhoneNumber.Customer_ID = Customer.Customer_ID";

                    string filter = "";
                    if (!string.IsNullOrEmpty(LastNameFilter))
                    {
                        filter += (string.IsNullOrEmpty(filter) ? " WHERE " : " AND ")
                            + " LastName like @LastName + '%' ";
                        cmd.AddParameter("@LastName", LastNameFilter, DbType.String);
                    }
                    if (!string.IsNullOrEmpty(CityFilter))
                    {
                        filter += (string.IsNullOrEmpty(filter) ? " WHERE " : " AND ")
                            + " City like @City + '%' ";
                        cmd.AddParameter("@City", CityFilter, DbType.String);
                    }
                    if (DateOfBirthFilter.HasValue)
                    {
                        filter += (string.IsNullOrEmpty(filter) ? " WHERE " : " AND ")
                            + " DateOfBirth >= @DateOfBirth ";
                        cmd.AddParameter("@DateOfBirth", DateOfBirthFilter, DbType.DateTime);
                    }

                    string sort = " ORDER BY Customer_ID";
                    if (!string.IsNullOrEmpty(sortField))
                    {
                        sort = " ORDER BY " + sortField.Replace("_desc", "") + (sortField.EndsWith("_desc") ? " DESC " : " ASC ");
                    }
                    string pagingSql = " OFFSET @RowsOffset ROWS FETCH NEXT @PageSize ROWS ONLY ;";
                    //params for paging
                    cmd.AddParameter("@PageSize", pageSize, DbType.Int32);
                    cmd.AddParameter("@RowsOffset", (page - 1) * pageSize, DbType.Int32);


                    cmd.CommandText = sql + filter + sort + pagingSql;
                    connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Client cl = new Client();
                            cl.Customer_ID = reader.GetInt32(0);
                            cl.FirstName = reader.GetString(1);
                            cl.LastName = reader.GetString(2);
                            cl.DateOfBirth = reader.GetDateTime(3);
                            cl.Country = reader.GetString(4);
                            cl.Email = reader.GetString(5);
                            cl.Password = reader.GetString(6);
                            if (!reader.IsDBNull(reader.GetOrdinal("username")))
                            {
                                cl.UserName = reader.GetString(reader.GetOrdinal("username"));
                            }
                            else
                            {
                                cl.UserName = null;
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("Phone Number")))
                            {
                                cl.PhoneNumber = reader.GetString(reader.GetOrdinal("Phone Number"));
                            }
                            else
                            {
                                cl.PhoneNumber = null;
                            }

                            results.Add(cl);
                        }
                    }
                }
            }

            totalItemsCount = 20;
            return results;
        }

        public void DeleteById(int id)
        {


            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM Customer WHERE Customer_ID='{id}'";
                    cmd.AddParameter("@Customer_ID", id, DbType.Int32);

                    conn.Open();
                    cmd.ExecuteNonQuery();


                }
            }


        }
        public bool isUniqueName(string username)
        {
            using (var conn = new SqlConnection(ConnectionStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select count(username) as [number] from Customer Where username =@username";
                    cmd.Parameters.AddWithValue("@username", username);
                    conn.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            if (reader.GetInt32(reader.GetOrdinal("number")) > 0)
                            {
                                return false;
                            }
                        }
                        }
                  
                }
                return true;
            }
        }
        public bool Login(string username, string password)
        {
            try
            {
                using (DbConnection conn = new SqlConnection(ConnectionStr))
                {
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"select dbo.udfLogin(@username, @password)";
                        cmd.AddParameter("@username", username, DbType.String);
                        cmd.AddParameter("@password", password, DbType.String);
                       
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return result != null && (int)result == 1;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        
    }
}