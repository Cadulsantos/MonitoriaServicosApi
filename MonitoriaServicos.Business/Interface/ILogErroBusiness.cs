using MonitoriaServicosApi.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonitoriaServicosApi.Business.Interface
{
    public interface ILogErroBusiness
    {
        List<dynamic> GetLogErrosServico(string idServico, string origem);

        bool AtualizaStatusLog(dynamic logErro);

        bool SolucionarErrosServico(string idServico);

        List<dynamic> GetLogErrosServicoPag(string idServico, int pagina);

        long GetQtdErroGroup(string servicoId);

    }
}
