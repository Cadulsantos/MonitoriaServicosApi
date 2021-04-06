using MongoDB.Driver;
using MonitoriaServicosApi.Models.Models;
using MonitoriaServicosApi.Repository.Repository.Interface;
using System.Collections.Generic;

namespace MonitoriaServicosApi.Repository.Repository
{
    public class ServicoRepository : IServicoRepository
    {
        private IMongoCollection<Servico> collection = Context.ConexaoProdMongo.GetCollection<Servico>("servicos");

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
            return collection.Find("{}").ToList();
        }
    }
}