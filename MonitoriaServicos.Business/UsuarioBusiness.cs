using MongoDB.Driver;
using MonitoriaServicosApi.Business.Interface;
using MonitoriaServicosApi.Models.Models;
using MonitoriaServicosApi.Repository.Repository;
using MonitoriaServicosApi.Repository.Repository.Interface;
using Newtonsoft.Json;
using System;

namespace MonitoriaServicosApi.Business
{
    public class UsuarioBusiness : IUsuarioBusiness
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioBusiness()
        {
            _usuarioRepository = new UsuarioRepository();

        }

        public object AutenticacaoUsuario(dynamic filtro)
        {
            try
            {
                var usuarioFiltro = JsonConvert.DeserializeObject<object>(filtro.ToString());

                string login = usuarioFiltro.login.ToString();
                string senha = usuarioFiltro.senha?.ToString();

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(senha))
                    throw new Exception("Campos de login ou senha vazio!");


                var filter = Builders<Usuario>.Filter.Where(x => x.Login == login &&  x.Senha == senha);

                var usuario = _usuarioRepository.GetUsuariosByExpression(filter);
                
                if(usuario == null)
                    throw new Exception("Usuario não encontrado!");

                usuario.Senha = string.Empty;

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}