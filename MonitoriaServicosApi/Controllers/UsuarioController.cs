using Microsoft.AspNetCore.Mvc;
using MonitoriaServicosApi.Business;
using MonitoriaServicosApi.Business.Interface;
using System;

namespace MonitoriaServicosApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioBusiness _usuarioBusiness;

        public UsuarioController()
        {
            _usuarioBusiness = new UsuarioBusiness();
        }

        [Route("AutenticacaoUsuario")]
        [HttpPost]
        public ActionResult AutenticacaoUsuario(dynamic usuario)
        {
            try
            {
                return Ok(_usuarioBusiness.AutenticacaoUsuario(usuario));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}