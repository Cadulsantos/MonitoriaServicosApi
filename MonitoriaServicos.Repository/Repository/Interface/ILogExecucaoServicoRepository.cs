using MonitoriaServicosApi.Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace MonitoriaServicosApi.Repository.Repository.Interface
{
    public interface ILogExecucaoServicoRepository
    {
        LogExecucaoServico GetLogUltimaExecucaoServico(string idServico);

        List<LogExecucaoServico> GetLogsExecucaoServicoById(string idServico);

        IQueryable<LogExecucaoServico> GetLogExecucao(string idServico);
    }
}