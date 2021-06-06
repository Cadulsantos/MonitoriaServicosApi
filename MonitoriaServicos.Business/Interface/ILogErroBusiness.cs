using MonitoriaServicosApi.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonitoriaServicosApi.Business.Interface
{
    public interface ILogErroBusiness
    {
        bool AtualizaStatusLog(dynamic logErro);

        bool SolucionarErrosServico(string idServico);

        List<dynamic> GetLogErrosServicoPag(string idServico, int pagina);

        long GetQtdErroGroup(string servicoId);

    }
}
