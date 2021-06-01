using MonitoriaServicosApi.Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace MonitoriaServicosApi.Repository.Repository.Interface
{
    public interface ILogErroRepository
    {
        List<LogErroServico> GetLogErroServicoProadv(string idServico);

        List<LogErroServico> GetLogErroServicoIntegracaoBipBop(string idServico);

        bool AtualizaStatusLogProAdv(LogErroServico logErro);

        bool AtualizaStatusLogInt(LogErroServico logErro);

        long GetQtdErro(Servico servico);

        long GetQtdErroGroup(string servicoId);

        bool SolucionarErrosServico(string idServico);

        IQueryable<LogErroServico> GetLogErroServico(string idServico);
        
    }
}