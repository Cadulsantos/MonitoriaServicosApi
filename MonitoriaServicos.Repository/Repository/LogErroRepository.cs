using MongoDB.Driver;
using MonitoriaServicosApi.Models.Models;
using MonitoriaServicosApi.Models.Models.Enum;
using MonitoriaServicosApi.Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitoriaServicosApi.Repository.Repository
{
    public class LogErroRepository : ILogErroRepository
    {
        private IMongoCollection<LogErroServico> collectionProadv = Context.ConexaoProdMongo.GetCollection<LogErroServico>("LogErrorServico");
        private IMongoCollection<LogErroServico> collectionInt = Context.ConexaoProdBipbopMongo.GetCollection<LogErroServico>("LogErrorServico");

        public long GetQtdErro(Servico servico)
        {

            return collectionProadv.CountDocuments(x => x.ServicoId == servico.Id && !x.Resolvido);
          
        }

        public List<dynamic> GetQtdLogsErro()
        {        
            List<dynamic> objs = new List<dynamic>();

            var result = collectionProadv.Aggregate()
               .Match(x => !x.Resolvido)
               .Project(p => new LogErroServico { ServicoId = p.ServicoId } )
               //.Group(y => y.ServicoId,
               // g => new
               // {
               //     ServicoId = g.Key,
               //     Count = g.Count()
               // })
               .ToList();

            return null;
        }

        public long GetQtdErroGroup(string servicoId)
        {

            var result = collectionProadv.Aggregate()
               .Match(x => x.ServicoId == servicoId && !x.Resolvido)
               .Group(y => new { y.Message, DataErro = new DateTime(y.DataErro.Year, y.DataErro.Month, y.DataErro.Day) },
                g => new LogErroServico
                {
                    ServicoId = g.First().ServicoId
                })
               .ToList()
               .Count();

            return result;
        }


        public IQueryable<LogErroServico> GetLogErroServico(string idServico)
        {
            var logsErro = from logErro in collectionProadv.AsQueryable()
                           where logErro.ServicoId == idServico && !logErro.Resolvido
                           group logErro by new
                           {
                               logErro.Message,
                               //logErro.Resolvido,
                               DataErro = new DateTime(logErro.DataErro.Year, logErro.DataErro.Month, logErro.DataErro.Day)
                           }
                           into grpLogErro
                           select new LogErroServico
                           {
                               ServicoId = grpLogErro.First().ServicoId,
                               Metodo = grpLogErro.First().Metodo,
                               Message = grpLogErro.First().Message,
                               Quantidade = grpLogErro.Count(),
                               Resolvido = grpLogErro.First().Resolvido,
                               DataErro = grpLogErro.Last().DataErro,
                               DataResolucao = grpLogErro.First().DataResolucao
                           };

           return logsErro;
        }

        public bool SolucionarErrosServico(string idServico)
        {
            var update = Builders<LogErroServico>.Update
                .Set(s => s.Resolvido, true)
                .Set(s => s.DataResolucao, DateTime.Now);

            var result = collectionProadv.UpdateMany(x => x.ServicoId == idServico && !x.Resolvido, update).MatchedCount > 0;

           return result;

        }

        public bool AtualizaStatusLogProAdv(LogErroServico logErro)
        {
            var filter = Builders<LogErroServico>.Filter.Where(x => !x.Resolvido && x.ServicoId == logErro.ServicoId
            && x.Message == logErro.Message && x.DataErro >= logErro.DataErro && x.DataErro < logErro.DataErro.AddDays(1));
            var update = Builders<LogErroServico>.Update
                .Set(x => x.Resolvido, logErro.Resolvido)
                .Set(x => x.DataResolucao, DateTime.Now);
            var result = collectionProadv.UpdateMany(filter, update) ?? collectionInt.UpdateMany(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public bool AtualizaStatusLogInt(LogErroServico logErro)
        {
            var filter = Builders<LogErroServico>.Filter.Where(x => !x.Resolvido && x.ServicoId == logErro.ServicoId
            && x.Message == logErro.Message && x.DataErro >= logErro.DataErro && x.DataErro < logErro.DataErro.AddDays(1));
            var update = Builders<LogErroServico>.Update
                .Set(x => x.Resolvido, logErro.Resolvido)
                .Set(x => x.DataResolucao, DateTime.Now);
            var result = collectionInt.UpdateMany(filter, update) ?? collectionInt.UpdateMany(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

       
    }
}