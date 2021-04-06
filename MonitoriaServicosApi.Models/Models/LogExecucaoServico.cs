using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MonitoriaServicosApi.Models.Models
{
    [BsonIgnoreExtraElements]
    public class LogExecucaoServico
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string idServico { get; set; }

        [BsonElement("NomeServico")]
        public string NomeServico { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataInicio { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DataFim { get; set; }

        public string TempoExecucao { get; set; }

    }
}