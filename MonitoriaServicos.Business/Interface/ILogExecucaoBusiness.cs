using System.Collections.Generic;

namespace MonitoriaServicos.Business.Interface
{
    public interface ILogExecucaoBusiness
    {
        List<dynamic> GetLogsExecucaoServico(string idServico, int pagina);

        long GetQtdLogsExec(string idServico);
    }
}