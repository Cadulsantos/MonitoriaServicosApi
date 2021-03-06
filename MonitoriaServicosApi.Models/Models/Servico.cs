using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MonitoriaServicosApi.Models.Models.Enum;
using System.Collections.Generic;

namespace MonitoriaServicosApi.Models.Models
{
    [BsonIgnoreExtraElements]
    public class Servico
    {
        public Servico()
        {
            Tags = new List<string>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nomeServico")]
        public string Nome { get; set; }

        [BsonElement("nomeArgument")]
        public string NomeArgument { get; set; }

        [BsonElement("status")]
        public bool Ativo { get; set; }

        [BsonElement("descricao")]
        public string Descricao { get; set; }

        [BsonElement("periodicidade")]
        public string Periodicidade { get; set; }

        [BsonElement("origem")]
        public OrigemServicoEnum Origem { get; set; }

        [BsonElement("tags")]
        public List<string> Tags { get; set; }
    }
}