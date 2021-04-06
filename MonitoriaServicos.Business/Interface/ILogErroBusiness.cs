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
    }
}
