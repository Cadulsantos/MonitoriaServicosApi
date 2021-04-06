using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MonitoriaServicosApi.Models.Models
{
    [BsonIgnoreExtraElements]
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("login")]
        public string Login { get; set; }

        [BsonElement("senha")]
        public string Senha { get; set; }

        [BsonElement("nome")]
        public string Nome { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("ativo")]
        public bool Ativo { get; set; }

        //[BsonElement("")]
        //public bool IsAdmin  { get; set; }

        [BsonElement("perfil")]
        public int Perfil { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonElement("dataCadastro")]
        public DateTime DataCadastro { get; set; }

    }
}