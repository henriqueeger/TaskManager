using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TaskManager.DataAccess
{
    public static class DataAccessMethods
    {
        static string connString;

        static DataAccessMethods()
        {
            connString = ConfigurationManager.ConnectionStrings["Supero"].ConnectionString;
        }

        public static SqlParameter CreateSqlParameter(string paramName, object value,
            SqlDbType type, int size = -1, bool nullIfEmpty = false,
            ParameterDirection direction = ParameterDirection.Input)
        {

            SqlParameter p = new SqlParameter(paramName, type);
            p.Value = (value == null ||
                (nullIfEmpty && type == SqlDbType.VarChar && ((string)value).Length == 0))
                ? DBNull.Value : value;
            if (size >= 0)
                p.Size = size;
            p.Direction = direction;
            return p;
        }
        public static SqlCommand GetSqlCommand(string commandText, CommandType type, params SqlParameter[] pars)
        {
            return GetSqlCommand(commandText, type, null, pars);
        }
        public static SqlCommand GetSqlCommand(string commandText, CommandType type, SqlTransaction tran,
            params SqlParameter[] pars)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = type;
            cmd.CommandText = commandText;
            if (tran != null)
            {
                cmd.Transaction = tran;
                cmd.Connection = tran.Connection;
            }
            for (int i = 0; i < pars.Length; i++)
            {
                cmd.Parameters.Add(pars[i]);
            }
            return cmd;
        }
        public static SqlTransaction GetSqlTransaction(IsolationLevel il)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction(il);
            return tran;
        }

        #region Extension methods para SqlTransaction
        public static void CommitAndCloseConnection(this SqlTransaction tran)
        {
            SqlConnection conn = tran.Connection;
            tran.Commit();
            conn.Close();
        }
        public static void RollbackAndCloseConnection(this SqlTransaction tran)
        {
            SqlConnection conn = tran.Connection;
            tran.Rollback();
            if (conn != null) conn.Close();
        }
        #endregion

        public static void ExecuteStmt(string stmt, params SqlParameter[] pars)
        {
            ExecuteStmt(stmt, null, pars);
        }
        public static void ExecuteStmt(string stmt, SqlTransaction tran, params SqlParameter[] pars)
        {
            Execute(stmt, CommandType.Text, tran, pars);
        }
        public static void ExecuteSP(string spName, params SqlParameter[] pars)
        {
            ExecuteSP(spName, null, pars);
        }
        public static void ExecuteSP(string spName, SqlTransaction tran, params SqlParameter[] pars)
        {
            Execute(spName, CommandType.StoredProcedure, tran, pars);
        }
        private static void Execute(string commandText, CommandType type,
            SqlTransaction tran, params SqlParameter[] pars)
        {

            SqlCommand cmd = GetSqlCommand(commandText, type, pars);
            SqlConnection conn = null;
            try
            {
                if (tran == null)
                {
                    conn = new SqlConnection(connString);
                    conn.Open();
                    cmd.Connection = conn;
                }
                else
                {
                    cmd.Transaction = tran;
                    cmd.Connection = tran.Connection;
                }
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (tran == null)
                {
                    conn.Close();
                    conn.Dispose();
                }

            }
        }

        public static DataTable GetDataTableFromStmt(string stmt, params SqlParameter[] pars)
        {
            return GetDataTableFromStmt(stmt, null, pars);
        }
        public static DataTable GetDataTableFromStmt(string stmt, SqlTransaction tran, params SqlParameter[] pars)
        {
            return GetDataTable(stmt, CommandType.Text, tran, pars);
        }
        public static DataTable GetDataTableFromSP(string spName, params SqlParameter[] pars)
        {
            return GetDataTableFromSP(spName, null, pars);
        }
        public static DataTable GetDataTableFromSP(string spName, SqlTransaction tran, params SqlParameter[] pars)
        {
            return GetDataTable(spName, CommandType.StoredProcedure, tran, pars);
        }
        private static DataTable GetDataTable(string commandText, CommandType type,
            SqlTransaction tran, SqlParameter[] pars)
        {

            DataTable dt = new DataTable();
            SqlCommand cmd = GetSqlCommand(commandText, type, pars);
            SqlConnection conn = null;
            SqlDataReader dr = null;
            try
            {
                if (tran == null)
                {
                    conn = new SqlConnection(connString);
                    conn.Open();
                    cmd.Connection = conn;
                }
                else
                {
                    cmd.Transaction = tran;
                    cmd.Connection = tran.Connection;
                }
                dr = cmd.ExecuteReader();
                dt.Load(dr);
                dr.Close();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
                cmd.Dispose();
                if (tran == null)
                {
                    conn.Close();
                    conn.Dispose();
                }

            }
            return dt;
        }
    }
}