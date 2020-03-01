using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace _5529_DBSD_CW2.DAL
{
    public static class ExtensionMethodDbCommand
    {
            public static DbParameter AddParameter(this DbCommand cmd, string name, object value, System.Data.DbType type)
            {
                var p = cmd.CreateParameter();
                p.ParameterName = name;
                if (value == null)
                {
                    p.Value = DBNull.Value;
                }
                else
                {
                    p.DbType = type;
                    p.Value = value;
                }
                cmd.Parameters.Add(p);
                return p;
            }
        }
    }
