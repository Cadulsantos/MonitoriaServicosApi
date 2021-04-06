using MongoDB.Driver;
using MonitoriaServicosApi.Business.Interface;
using MonitoriaServicosApi.Models.Models;
using MonitoriaServicosApi.Repository.Repository;
using MonitoriaServicosApi.Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoriaServicosApi.Business
{
    public class ServicoBusiness : IServicoBusiness
    {
        private readonly IServicoRepository _servicoRepository;
        private readonly ILogExecucaoServicoRepository _logExecucaoServicoRepository;
        private readonly ILogErroRepository _logErroRepository;

        public ServicoBusiness()
        {
            _servicoRepository = new ServicoRepository();
            _logExecucaoServicoRepository = new LogExecucaoServicoRepository();
            _logErroRepository = new LogErroRepository();
        }

        public bool AtualizaServico(dynamic informacoes)
        {
            var servico = new Servico
            {
                Id = informacoes.id,
                NomeArgument = informacoes.nomeArgument,
                Ativo = informacoes.ativo == "true" ? true : false,
                Periodicidade = informacoes.periodicidade,
                Descricao = informacoes.descricao
            };
            return _servicoRepository.AtualizaServico(servico);
        }

        public List<dynamic> GetLogsExecucaoServico(string idServico)
        {
            var servicos = _logExecucaoServicoRepository.GetLogsExecucaoServicoById(idServico);
            var objs = new List<dynamic>();

            foreach (var servico in servicos)
            {
                try
                {
                    dynamic obj = new
                    {
                        servicoId = servico.idServico,
                        nomeServico = servico.NomeServico,
                        dataInicio = servico.DataInicio.ToString(),
                        dataFim = servico.DataFim == null ? "Agora" : servico.DataFim.Value.ToString(),
                        tempoExecucao = servico.DataFim == null ? "Em Execução" : servico.TempoExecucao
                    };
                    objs.Add(obj);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + servico.Id);
                }
            }

            return objs.OrderByDescending(x => x.dataInicio).ToList();
        }

        public List<dynamic> GetServicos()
        {
            var servicos = _servicoRepository.GetServicos();
            var objs = new List<dynamic>();

            Parallel.ForEach(servicos,
                new ParallelOptions { MaxDegreeOfParallelism = 25 },
                servico =>
                {
                    //foreach (var servico in servicos)
                    //{
                    try
                    {
                        var logExecucao = _logExecucaoServicoRepository.GetLogUltimaExecucaoServico(servico.Id);
                        var qtdErro = _logErroRepository.GetQtdErro(servico);

                        dynamic obj = new
                        {
                            id = servico.Id,
                            nome = servico.Nome,
                            nomeArgument = servico.NomeArgument,
                            ativo = servico.Ativo,
                            descricao = servico.Descricao,
                            periodicidade = servico.Periodicidade,
                            data = logExecucao != null && logExecucao.DataInicio.Date != null ? logExecucao.DataInicio : DateTime.MinValue,
                            dataInicio = logExecucao != null && logExecucao.DataInicio != null ? logExecucao.DataInicio.ToString() : "Serviço não Executado",
                            dataFim = logExecucao != null && logExecucao.DataFim != null ? logExecucao.DataFim.ToString() : "",
                            origem = servico.Origem.ToString(),
                            quantidadeErros = qtdErro.ToString(),
                            erro = qtdErro > 0 ? true : false
                        };
                        objs.Add(obj);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + servico.Id);
                    }
                    //}
                });

            return objs.OrderByDescending(x => x.data).ToList();
        }

        public List<dynamic> GetServicosFiltro(dynamic filtroServico)
        {
            var objs = new List<dynamic>();

            var filtro = Builders<Servico>.Filter;

            var result = filtro.Where(x => x.Id != null);

            if (filtroServico.nomeArgument != null && filtroServico.nomeArgument != "")
            {
                string nome = Convert.ToString(filtroServico.nomeArgument);
                result &= filtro.Where(x => x.NomeArgument.ToLower().Contains(nome.ToLower()));
            }

            if (filtroServico.ativo != null)
            {
                bool ativo = Convert.ToBoolean(filtroServico.ativo);
                result &= filtro.Where(x => x.Ativo == ativo);
            }

            var servicos = _servicoRepository.GetServicosByExpression(result);

            //Parallel.ForEach(servicos,
            //    new ParallelOptions { MaxDegreeOfParallelism = 25 },
            //    servico =>
            //    {
            foreach (var servico in servicos)
            {
                try
                {
                    var logExecucao = _logExecucaoServicoRepository.GetLogUltimaExecucaoServico(servico.Id);
                    var qtdErro = _logErroRepository.GetQtdErro(servico);

                    objs.Add(new
                    {
                        id = servico.Id,
                        nome = servico.Nome,
                        nomeArgument = servico.NomeArgument,
                        ativo = servico.Ativo,
                        descricao = servico.Descricao,
                        periodicidade = servico.Periodicidade,
                        data = logExecucao != null && logExecucao.DataInicio.Date != null ? logExecucao.DataInicio : DateTime.MinValue,
                        dataInicio = logExecucao != null && logExecucao.DataInicio != null ? logExecucao.DataInicio.ToString() : "Serviço não Executado",
                        dataFim = logExecucao != null && logExecucao.DataFim != null ? logExecucao.DataFim.ToString() : "",
                        origem = servico.Origem.ToString(),
                        quantidadeErros = qtdErro.ToString(),
                        erro = qtdErro > 0 ? true : false
                    });
                    //objs.Add(obj);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + servico.Id);
                }
            }
            //});

            if (filtroServico.erro != null)
            {
                bool erro = Convert.ToBoolean(filtroServico.erro);
                objs = objs.Where(x => x.erro == erro).ToList();
            }

            return objs.OrderByDescending(x => x.data).ToList();
        }
    }
}