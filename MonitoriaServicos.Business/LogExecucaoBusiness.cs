using MonitoriaServicos.Business.Interface;
using MonitoriaServicosApi.Repository.Repository;
using MonitoriaServicosApi.Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitoriaServicos.Business
{
   public class LogExecucaoBusiness : ILogExecucaoBusiness
    {
        private readonly ILogExecucaoServicoRepository _logExecucaoServicoRepository;

        public LogExecucaoBusiness()
        {
            _logExecucaoServicoRepository = new LogExecucaoServicoRepository();
        }

        public List<dynamic> GetLogsExecucaoServico(string idServico, int pagina)
        {
            
            var objs = new List<dynamic>();

            var tamanhoPagina = 10;

            var queryLogsExec = _logExecucaoServicoRepository.GetLogExecucao(idServico);
            var paginacaoExec = queryLogsExec.OrderByDescending(o => o.DataInicio).Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();

            paginacaoExec.ForEach(logExec => {
                objs.Add(new {
                    servicoId = logExec.idServico,
                    nomeServico = logExec.NomeServico,
                    dataInicio = logExec.DataInicio.ToString(),
                    dataFim = logExec.DataFim == null ? "Agora" : logExec.DataFim.Value.ToString(),
                    tempoExecucao = logExec.DataFim == null ? "Em Execução" : logExec.TempoExecucao
                });
            });

            //var servicos = _logExecucaoServicoRepository.GetLogsExecucaoServicoById(idServico);
            //foreach (var servico in servicos)
            //{
            //    try
            //    {
            //        dynamic obj = new
            //        {
            //            servicoId = servico.idServico,
            //            nomeServico = servico.NomeServico,
            //            dataInicio = servico.DataInicio.ToString(),
            //            dataFim = servico.DataFim == null ? "Agora" : servico.DataFim.Value.ToString(),
            //            tempoExecucao = servico.DataFim == null ? "Em Execução" : servico.TempoExecucao
            //        };
            //        objs.Add(obj);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(ex.Message + servico.Id);
            //    }
            //}

            return objs/*.OrderByDescending(x => x.dataInicio).ToList()*/;
        }

        public long GetQtdLogsExec(string idServico)
        {
            return _logExecucaoServicoRepository.GetQtdLogsExec(idServico);
        }
    }
}
