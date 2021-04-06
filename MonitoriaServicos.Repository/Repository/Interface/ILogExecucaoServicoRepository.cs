using MonitoriaServicosApi.Models.Models;
using System.Collections.Generic;

namespace MonitoriaServicosApi.Repository.Repository.Interface
{
    public interface ILogExecucaoServicoRepository
    {
        LogExecucaoServico GetLogUltimaExecucaoServico(string idServico);
        List<LogExecucaoServico> GetLogsExecucaoServicoById(string idServico);

    }
}