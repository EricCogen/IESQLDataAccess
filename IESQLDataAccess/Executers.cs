using System;
using System.Data;
using System.Data.SqlClient;

namespace CCIH.Utilities.IESQLDataAccess
{
    public class Executers
    {
        public string Status { get; private set; }
        private string _dbConn = string.Empty;
        public int commandTimeOut = 120;

        public Executers(string dbConnectionString)
        {
            _dbConn = dbConnectionString;
        }

        public Executers(string Server, string DataBase, string UserName, string Password)
        {
            _dbConn = $"Server={Server};Database={DataBase};User Id={UserName};Password={Password};";
        }

        public Executers(string Server, string DataBase, bool Trusted_Connection = true)
        {
            _dbConn = $"Server={Server};Database={DataBase};Trusted_Connection=True;";
        }

        private void LoadParams(ref SqlCommand cmd, Parameters Params, string proc)
        {
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = commandTimeOut;
                cmd.CommandText = proc;

                for (int p = 0; p < Params.parameters.Count; p++)
                {
                    if (Params.parameters[p].maxLength < 1)
                    {
                        cmd.Parameters.Add(Params.parameters[p].parameterName, Params.parameters[p].dbType).Value = Params.parameters[p].parameterValue;
                    }
                    else
                    {
                        cmd.Parameters.Add(Params.parameters[p].parameterName, Params.parameters[p].dbType, Params.parameters[p].maxLength).Value = Params.parameters[p].parameterValue;
                    }
                }

                cmd.Connection = new SqlConnection(_dbConn);
                cmd.Connection.Open();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Execute A Stored Procedure
        /// </summary>
        /// <param name="nameOfStoredProcedure">Name Of The Stored Procedure To Execute</param>
        /// <param name="Params">Parameters</param>
        /// <example>
        /// Parameters p = new Parameters();
        /// p.Add("@FirstName", SqlDbType.VarChar, "Eric");
        /// Executers exec = new Executers(dbConnectionString);
        /// exec.Execute("dbo.MyStoredProcedure", myParams);
        /// </example>
        public void Execute(string nameOfStoredProcedure, Parameters Params)
        {
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand();
                LoadParams(ref cmd, Params, nameOfStoredProcedure);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Status = $"Message: {ex.Message}\r\nStackTrace: {ex.StackTrace}";
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Execute A Stored Procedure: Fill A DataTable
        /// </summary>
        /// <param name="nameOfStoredProcedure">Name Of The Stored Procedure To Execute</param>
        /// <param name="Params">Parameters</param>
        /// <param name="DT">DataTable</param>
        /// <example>
        /// DataTable myDataTable = new DataTable();
        /// Parameters p = new Parameters();
        /// p.Add("@FirstName", SqlDbType.VarChar, "Eric");
        /// 
        /// Executers exec = new Executers(dbConnectionString);
        /// exec.Execute("dbo.MyStoredProcedure", myParams, myDataTable);
        /// 
        /// if(!string.IsNullOrEmpty(exec.Status))
        /// {
        ///     //An Error Occurred: Check The exec.Status variable;
        /// }
        /// else if(myDataTable != null && myDataTable.Rows != null)
        /// {
        ///     ForEach(DataRow dr in myDataTable.Rows)
        ///     {
        ///         string myColumnName = dr.Field<string>("myColumnName");
        ///     }
        /// }
        /// </example>
        public void Execute(string nameOfStoredProcedure, Parameters Params, ref DataTable DT)
        {
            DT = Execute(Params, nameOfStoredProcedure);
        }

        /// <summary>
        /// Execute A Stored Procedure: Returns A DataTable;
        /// </summary>
        /// <param name="Params">Parameters</param>
        /// <param name="nameOfStoredProcedure">Name Of The Stored Procedure To Execute</param>
        /// <returns>DataTable</returns>
        public DataTable Execute(Parameters Params, string nameOfStoredProcedure)
        {
            DataTable DT = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand();
                LoadParams(ref cmd, Params, nameOfStoredProcedure);
                DT.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                Status = $"Message: {ex.Message}\r\nStackTrace: {ex.StackTrace}";
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
            return DT;
        }
    }
}
