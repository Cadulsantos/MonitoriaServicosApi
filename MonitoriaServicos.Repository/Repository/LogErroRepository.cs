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
            var result = new long();

            if (servico.Origem == OrigemServicoEnum.ProAdv)
                result = collectionProadv.CountDocuments(x => x.ServicoId == servico.Id
                 && !x.Resolvido && x.DataErro > DateTime.Today.AddDays(-30));
            else if (servico.Origem == OrigemServicoEnum.IntegracaoExterna)
                result = collectionInt.CountDocuments(x => x.ServicoId == servico.Id
                 && !x.Resolvido && x.DataErro > DateTime.Today.AddDays(-30));

            return result;
        }

        public List<LogErroServico> GetLogErroServicoProadv(string idServico)
        {
            #region pagination

            //int pageSize = 5;
            //int page = 1;
            //var filter = Builders<LogErroServico>.Filter.Where(x => x.ServicoId == idServico && x.DataErro > DateTime.Now.Date.AddDays(-30));

            //var data = collectionProadv.Find(filter)
            //    .SortByDescending(x => x.DataErro)
            //    .Skip((page - 1) * pageSize)
            //    .Limit(pageSize)
            //    .ToList();

            //var count = collectionProadv.CountDocuments(filter);

            #endregion pagination

            //var result = collectionProadv.Aggregate()
            //    .Match(x => x.ServicoId == idServico && x.DataErro > DateTime.Now.Date.AddDays(-30))
            //    .Group(y => new { y.Metodo, y.Message, y.Resolvido, DataErro = new DateTime(y.DataErro.Year, y.DataErro.Month, y.DataErro.Day) },
            //    g => new LogErroServico
            //    {
            //        ServicoId = g.First().ServicoId,
            //        Metodo = g.First().Metodo,
            //        Message = g.First().Message,
            //        Quantidade = g.Count(),
            //        Resolvido = g.First().Resolvido,
            //        DataErro = g.Last().DataErro,
            //        DataResolucao = g.First().DataResolucao
            //    })
            //    .SortByDescending(x => x.DataErro)/*.ThenBy(t => t.DataErro)*/
            //    .Limit(1000)
            //    .ToList();

            var result = collectionProadv.Aggregate()
                .Match(x => x.ServicoId == idServico && x.DataErro > DateTime.Now.Date.AddDays(-30) && !x.Resolvido)
                .ToList()
                .GroupBy(key => new { /*key.Metodo,*/ key.Message, key.Resolvido, DataErro = new DateTime(key.DataErro.Year, key.DataErro.Month, key.DataErro.Day) })
                .Select(s => new LogErroServico
                {
                    ServicoId = s.First().ServicoId,
                    Metodo = s.First().Metodo,
                    Message = s.First().Message,
                    Quantidade = s.Count(),
                    Resolvido = s.First().Resolvido,
                    DataErro = s.Last().DataErro,
                    DataResolucao = s.First().DataResolucao
                })
                .OrderByDescending(o => o.DataErro)
                .ToList();
            return result;
        }

        public IQueryable<LogErroServico> GetLogErroServico(string idServico)
        {
            var logsErro = from logErro in collectionProadv.AsQueryable()
                           where logErro.ServicoId == idServico && !logErro.Resolvido
                           group logErro by new
                           {
                               logErro.Message.Length,
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

        public List<LogErroServico> GetLogErroServicoIntegracaoBipBop(string idServico)
        {
            var result = collectionInt.Aggregate()
                .Match(x => x.ServicoId == idServico && x.DataErro > DateTime.Now.Date.AddDays(-30))
                .Group(y => new { y.Metodo, y.Message, y.Resolvido, DataErro = new DateTime(y.DataErro.Year, y.DataErro.Month, y.DataErro.Day) },
                g => new LogErroServico
                {
                    ServicoId = g.First().ServicoId,
                    Metodo = g.First().Metodo,
                    Message = g.First().Message,
                    Quantidade = g.Count(),
                    Resolvido = g.First().Resolvido,
                    DataErro = g.Last().DataErro,
                    DataResolucao = g.First().DataResolucao
                })
                .Limit(10000)
                .SortByDescending(x => x.DataErro)/*.ThenBy(t => t.DataErro)*/
                .ToList();

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