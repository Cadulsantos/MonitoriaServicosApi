using System;
using System.Collections.Generic;
using System.Text;

namespace MonitoriaServicos.Business.Interface
{
    public interface ILogExecucaoBusiness
    {
        List<dynamic> GetLogsExecucaoServico(string idServico, int pagina);
    }
}
