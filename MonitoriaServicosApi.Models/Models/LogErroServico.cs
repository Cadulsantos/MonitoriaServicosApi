using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MonitoriaServicosApi.Models.Models
{
    [BsonIgnoreExtraElements]
    public class LogErroServico
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }

        public string Servico { get; set; }

        public string Metodo { get; set; }

        public string ServicoId  { get; set; }

        public string Message { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataErro { get; set; }

        public bool Resolvido { get; set; }

        public DateTime? DataResolucao { get; set; }

        public int Quantidade { get; set; }

        public string Origem { get; set; }
    }
}