using MonitoriaServicosApi.Models.Models;
using System.Collections.Generic;

namespace MonitoriaServicosApi.Repository.Repository.Interface
{
    public interface ILogErroRepository
    {
        List<LogErroServico> GetLogErroServicoProadv(string idServico);
        List<LogErroServico> GetLogErroServicoIntegracaoBipBop(string idServico);
        bool AtualizaStatusLogProAdv(LogErroServico logErro);

        bool AtualizaStatusLogInt(LogErroServico logErro);
        long GetQtdErro(Servico servico);
    }
}