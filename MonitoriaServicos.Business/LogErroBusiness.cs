using MonitoriaServicosApi.Business.Interface;
using MonitoriaServicosApi.Models.Models;
using MonitoriaServicosApi.Models.Models.Enum;
using MonitoriaServicosApi.Repository.Repository;
using MonitoriaServicosApi.Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitoriaServicosApi.Business
{
    public class LogErroBusiness : ILogErroBusiness
    {
        private readonly ILogErroRepository _logErroRepository;

        public LogErroBusiness()
        {
            _logErroRepository = new LogErroRepository();
        }

        public bool AtualizaStatusLog(dynamic logErro)
        {
            try
            {
                var data = $"{logErro.dataErro}";
                var log = new LogErroServico()
                {
                    ServicoId = logErro.servicoId,
                    DataErro = Convert.ToDateTime($"{logErro.dataErro}").Date,
                    Message = logErro.message,
                    Resolvido = !Convert.ToBoolean($"{ logErro.resolvido }"),
                    Origem = logErro.origem
                };

                var result = _logErroRepository.AtualizaStatusLogProAdv(log);
                    

                //var result = log.Origem == OrigemServicoEnum.ProAdv.ToString()
                //    ? _logErroRepository.AtualizaStatusLogProAdv(log)
                //    : _logErroRepository.AtualizaStatusLogInt(log);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<dynamic> GetLogErrosServicoPag(string idServico, int pagina)
        {
            try
            {
                var objs = new List<dynamic>();

                var tamanhoPagina = 10;

                var queryLogsErro = _logErroRepository.GetLogErroServico(idServico);
                var paginacaoErro = queryLogsErro.OrderByDescending(o => o.DataErro).Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();
                
                paginacaoErro.ForEach(log => {
                    objs.Add(new {
                        servicoId = log.ServicoId,
                        message = log.Message,
                        resolvido = log.Resolvido,
                        dataResolucao = log.Resolvido ? log.DataResolucao.Value.ToString("dd/MM/yyyy HH:mm") : "Pendente",
                        quantidade = log.Quantidade,
                        dataErro = log.DataErro.ToString("dd/MM/yyyy HH:mm"),
                    });
                });

                return objs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public long GetQtdErroGroup(string servicoId) 
        {
            try
            {
                return _logErroRepository.GetQtdErroGroup(servicoId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool SolucionarErrosServico(string idServico)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(idServico))
                    return _logErroRepository.SolucionarErrosServico(idServico);
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}