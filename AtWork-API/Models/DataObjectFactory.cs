using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API_Placement_record_management.Models
{
    public class DataObjectFactory
    {
        public static SqlConnection CreateNewConnection()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["atwork_dev"].ConnectionString);
        }
        public static void OpenConnection(SqlConnection sqlConnection)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        public static void CloseConnection(SqlConnection sqlConnection)
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public static void DisposeDataReader(SqlDataReader sqlDataReader)
        {
            if (sqlDataReader != null)
            {
                if (sqlDataReader.IsClosed == false)
                {
                    sqlDataReader.Close();
                }
                sqlDataReader.Dispose();
            }
        }
        public static void DisposeCommand(SqlCommand sqlCommand)
        {
            if (sqlCommand != null)
            {
                sqlCommand.Dispose();
            }
        }
    }
}