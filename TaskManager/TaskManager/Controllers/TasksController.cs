using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;
using TaskManager.Models;
using TaskManager.DataAccess;
using System.Data;

namespace TaskManager.Controllers
{
    public class TasksController : ApiController
    {
        private static List<Task> tarefas = new List<Task>();
        public List<Task> Get(int _taskID)
        {
            return Utils.ExtensionMethods.ToList<Task>(DataAccess.DataAccess.GetTarefas(_taskID));
        }
        public string Post([FromBody] Task tarefa)
        {
            DateTime dataAtual = DateTime.Now;

            if (string.IsNullOrEmpty(tarefa.DesTitulo))
                return "Título da tarefa deve ser informado!";
            if (string.IsNullOrEmpty(tarefa.DesDescricao))
                return "Descrição da tarefa deve ser informada!";

            tarefa.DatUltAlteracao = dataAtual;

            if (tarefa.TaskID == 0)
                tarefa.DatCriacao = dataAtual;
            if (tarefa.TaskID > 0 && tarefa.IndStatus == "C") 
                tarefa.DatConclusao = dataAtual;

            SqlTransaction tran = null;
            try
            {

                tran = DataAccessMethods.GetSqlTransaction(IsolationLevel.ReadCommitted);
                DataAccess.DataAccess.SetTarefas(tarefa, tran);
                tran.CommitAndCloseConnection();
            }
            catch (SqlException ex)
            {

                if (tran != null)
                    tran.RollbackAndCloseConnection();
                return ex.Message;
            }
            return "ok";
        }
        public void Delete(int taskID)
        {
            SqlTransaction tran = null;
            try
            {

                tran = DataAccessMethods.GetSqlTransaction(IsolationLevel.ReadCommitted);
                DataAccess.DataAccess.DelTarefas(taskID, tran);
                tran.CommitAndCloseConnection();
            }
            catch (SqlException ex)
            {
                if (tran != null)
                    tran.RollbackAndCloseConnection();
            }
        }
    }
}
