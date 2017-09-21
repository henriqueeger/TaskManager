using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{
    public class Task
    {
        public int TaskID { get; set; }
        public string DesTitulo { get; set; }
        public string IndStatus { get; set; }
        public string DesDescricao { get; set; }
        public DateTime DatCriacao { get; set; }
        public DateTime DatUltAlteracao { get; set; }
        public DateTime DatConclusao { get; set; }

        public Task() { }
        public Task(int _taskID, string _desTitulo, string _indStatus, string _desDescricao)
        {
            this.TaskID = _taskID;
            this.DesTitulo = _desTitulo;
            this.DesDescricao = _desDescricao;
            this.IndStatus = _indStatus;

        }
    }
}