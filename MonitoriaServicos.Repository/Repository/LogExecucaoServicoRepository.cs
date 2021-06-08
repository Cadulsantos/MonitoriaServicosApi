using MongoDB.Driver;
using MonitoriaServicosApi.Models.Models;
using MonitoriaServicosApi.Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitoriaServicosApi.Repository.Repository
{
    public class LogExecucaoServicoRepository : ILogExecucaoServicoRepository
    {
        private IMongoCollection<LogExecucaoServico> collection = Context.ConexaoProdMongo.GetCollection<LogExecucaoServico>("logExecucaoServico");
        private static DateTime _competencia = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        public LogExecucaoServico GetLogUltimaExecucaoServico(string idServico)
        {
            var result = collection.Find(x => x.idServico == idServico).SortByDescending(s => s.DataInicio).ToList();

            return result.FirstOrDefault();
        }

        public List<LogExecucaoServico> GetLogsExecucaoServicoById(string idServico)
        {
            var teste = _competencia.Date.ToString();
            var result = collection.Find(x => x.idServico == idServico /*&& x.DataInicio >= _competencia*/).ToList();
            return result;
        }

        public IQueryable<LogExecucaoServico> GetLogExecucao(string idServico)
        {
            var logsExec = from logExec in collection.AsQueryable()
                           where logExec.idServico == idServico
                           select logExec;

            return logsExec;
                           
        }
    }
}