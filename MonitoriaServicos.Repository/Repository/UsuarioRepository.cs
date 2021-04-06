using MongoDB.Driver;
using MonitoriaServicosApi.Models.Models;
using MonitoriaServicosApi.Repository.Repository.Interface;

namespace MonitoriaServicosApi.Repository.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private IMongoCollection<Usuario> collection = Context.ConexaoHomologMongo.GetCollection<Usuario>("usuario");

        public Usuario GetUsuariosByExpression(FilterDefinition<Usuario> filter)
        {
            return collection.Find(filter).FirstOrDefault();
            //return collection.Find("{}").FirstOrDefault();
        }
    }
}