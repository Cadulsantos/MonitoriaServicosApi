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

                var result = log.Origem == OrigemServicoEnum.ProAdv.ToString()
                    ? _logErroRepository.AtualizaStatusLogProAdv(log)
                    : _logErroRepository.AtualizaStatusLogInt(log);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<dynamic> GetLogErrosServico(string idServico, string origem)
        {
            try
            {
                var logAgrupado = new List<LogErroServico>();

                if (origem == OrigemServicoEnum.ProAdv.ToString())
                    logAgrupado = _logErroRepository.GetLogErroServicoProadv(idServico);
                else if (origem == OrigemServicoEnum.IntegracaoExterna.ToString())
                    logAgrupado = _logErroRepository.GetLogErroServicoIntegracaoBipBop(idServico);
                var objs = new List<dynamic>();

                //Parallel.ForEach(logAgrupado, new ParallelOptions { MaxDegreeOfParallelism = 25 }, log =>
                //{
                foreach (var log in logAgrupado)
                {
                    try
                    {
                        dynamic obj = new
                        {
                            servicoId = log.ServicoId,
                            message = log.Message,
                            resolvido = log.Resolvido,
                            dataResolucao = log.Resolvido ? log.DataResolucao.Value.ToString("dd/MM/yyyy HH:mm") : "Pendente",
                            quantidade = log.Quantidade,
                            dataErro = log.DataErro.ToString("dd/MM/yyyy HH:mm"),
                            origem = origem
                        };
                        objs.Add(obj);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + log.ServicoId);
                    }
                }
                //});

                return objs;
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
                var queryLogsErro = _logErroRepository.GetLogErroServico(idServico);



                return null;
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