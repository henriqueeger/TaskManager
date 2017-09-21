using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TaskManager.Models;

namespace TaskManager.DataAccess
{
    public class DataAccess
    {
        public static DataTable GetTarefas(int taskID)
        {
            return DataAccessMethods.GetDataTableFromSP("Tarefas.GetTarefas",
                DataAccessMethods.CreateSqlParameter("@taskID", taskID, SqlDbType.Int));
        }
        public static void SetTarefas(Task tarefa, SqlTransaction tran = null)
        {

            DataAccessMethods.ExecuteSP("Tarefas.SetTarefas", tran,
                DataAccessMethods.CreateSqlParameter("@TaskID", tarefa.TaskID, SqlDbType.Int),
                DataAccessMethods.CreateSqlParameter("@DesTitulo", tarefa.DesTitulo, SqlDbType.VarChar, 40),
                DataAccessMethods.CreateSqlParameter("@DesDescricao", tarefa.DesDescricao, SqlDbType.VarChar, 100),
                DataAccessMethods.CreateSqlParameter("@IndStatus", tarefa.IndStatus, SqlDbType.VarChar, 10),
                DataAccessMethods.CreateSqlParameter("@DatCriacao", tarefa.DatCriacao, SqlDbType.Date),
                DataAccessMethods.CreateSqlParameter("@DatUltAlteracao", tarefa.DatUltAlteracao, SqlDbType.Date),
                DataAccessMethods.CreateSqlParameter("@DatConclusao", tarefa.DatConclusao, SqlDbType.Date));
        }
        public static void DelTarefas(int taskID, SqlTransaction tran = null)
        {
            DataAccessMethods.ExecuteSP("Tarefas.DelTarefas",
                DataAccessMethods.CreateSqlParameter("@taskID", taskID, SqlDbType.Int));
        }
    }
}