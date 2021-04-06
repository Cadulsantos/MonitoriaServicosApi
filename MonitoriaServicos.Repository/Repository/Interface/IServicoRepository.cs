using MonitoriaServicosApi.Models.Models;
using System.Collections.Generic;

namespace MonitoriaServicosApi.Repository.Repository.Interface
{
    public interface IServicoRepository
    {
        List<Servico> GetServicos();
        bool AtualizaServico(Servico servico);
    }
}