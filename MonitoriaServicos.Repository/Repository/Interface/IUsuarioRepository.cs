using MongoDB.Driver;
using MonitoriaServicosApi.Models.Models;

namespace MonitoriaServicosApi.Repository.Repository.Interface
{
    public interface IUsuarioRepository
    {
        Usuario GetUsuariosByExpression(FilterDefinition<Usuario> filter);
    }
}