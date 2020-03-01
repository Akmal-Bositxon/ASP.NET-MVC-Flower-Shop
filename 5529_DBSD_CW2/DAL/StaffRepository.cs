using _00005529_DBSD_CW2.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace _00005529_DBSD_CW2.DAL
{
    public class StaffRepository
    {
        public string ConnectionStr
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["Gullar"].ConnectionString;
            }
        }
        public IList<Staff> GetAllStaffs()
        {
            List<Staff> empList = new List<Staff>();
            using (DbConnection connection = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandTimeout = 120;
                    cmd.CommandText = @"SELECT Emp_ID, FirstName, LastName, DateOfBirth, Position, WorkedHours, HiringDate FROM [dbo].[Employee]";
                    connection.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Staff cl = new Staff()
                            {
                                Emp_ID = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                DateOfBirth = reader.GetDateTime(3),
                                Position = reader.GetString(4),
                                WorkedHours = reader.GetInt32(5),
                                HiringDate = reader.GetDateTime(6)
                            };
                            empList.Add(cl);
                        }
                    }
                }
            }
            return empList;
        }
    }
}