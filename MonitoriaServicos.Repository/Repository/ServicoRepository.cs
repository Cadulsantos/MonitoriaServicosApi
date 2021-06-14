using MongoDB.Driver;
using MonitoriaServicos.Models.ViewModels;
using MonitoriaServicosApi.Models.Models;
using MonitoriaServicosApi.Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitoriaServicosApi.Repository.Repository
{
    public class ServicoRepository : IServicoRepository
    {
        private IMongoCollection<Servico> collection = Context.ConexaoProdMongo.GetCollection<Servico>("servicos");
        private IMongoCollection<LogErroServico> collectioLogErro = Context.ConexaoProdMongo.GetCollection<LogErroServico>("LogErrorServico");
        private IMongoCollection<LogExecucaoServico> collectionLogExecucao = Context.ConexaoProdMongo.GetCollection<LogExecucaoServico>("logExecucaoServico");

        public bool AtualizaServico(Servico servico)
        {
            var update = Builders<Servico>.Update
                .Set(x => x.NomeArgument, servico.NomeArgument)
                .Set(x => x.Ativo, servico.Ativo)
                .Set(x => x.Periodicidade, servico.Periodicidade)
                .Set(x => x.Descricao, servico.Descricao);

           var result = collection.UpdateOne(x => x.Id == servico.Id, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public List<Servico> GetServicos()
        {
            //GetServicos2();
            return collection.Find("{}").ToList();
        }

        public List<dynamic> GetServicos2()
        {
            try
            {
                var data = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                var listServico = (from s in collection.AsQueryable()
                                   //join logErro in collectioLogErro.AsQueryable() on s.Id equals logErro.ServicoId into erros
                                   //join logExec in collectionLogExecucao.AsQueryable() on s.Id equals logExec.idServico into ultimaExec
                                   //join logErro in collectioLogErro.AsQueryable().Where(x => !x.Resolvido && x.DataErro > DateTime.Today.AddDays(-30)) on s.Id equals logErro.Servico into erroCount
                                   //join logExec in collectionLogExecucao.AsQueryable().Where(x => x.DataInicio > data).OrderByDescending(o => o.DataInicio) on s.Id equals logExec.idServico into ultimaExec
                                   select new ServicoViewModel
                                   {
                                       Id = s.Id,
                                       Nome = s.Nome,
                                       NomeArgument = s.NomeArgument,
                                       Ativo = s.Ativo,
                                       Descricao = s.Descricao,
                                       Periodicidade = s.Periodicidade,
                                       //Data = ultimaExec.ToList().FirstOrDefault().DataInicio != null ? ultimaExec.ToList().FirstOrDefault().DataInicio.Date : DateTime.MinValue,
                                       //DataInicio = ultimaExec.ToList().FirstOrDefault().DataInicio != null ? ultimaExec.ToList().FirstOrDefault().DataInicio.Date.ToString() : "Serviço não Executado",
                                       //DataFim = ultimaExec.ToList().FirstOrDefault().DataFim != null ? ultimaExec.ToList().FirstOrDefault().DataFim.ToString() : "",
                                       Origem = s.Origem,
                                       //QuantidadeErros = erros.Count(),
                                       //Erro = erros.Count() > 0
                                       //LogsErro = new LogErroServico {
                                       //  Message = erros.First().Message
                                       //}
                                   }
                                   )
                                   .ToList()
                                   .OrderByDescending(o => o.QuantidadeErros)
                                   .ToList();

                var listSevicoErro = (
                    from servico in listServico
                    join erro in collectioLogErro.AsQueryable() on servico.Id equals erro.ServicoId into errosServico
                    select new ServicoViewModel
                    {
                        LogsErro = errosServico
                    }
                    into emailGroup
                    where emailGroup.LogsErro.Count() == 0
                    select emailGroup).ToList();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public List<Servico> GetServicosByExpression(FilterDefinition<Servico> filtro)
        {
            return collection.Find(filtro).ToList();
        }
    }
}