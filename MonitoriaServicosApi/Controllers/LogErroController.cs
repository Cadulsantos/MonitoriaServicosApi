using Microsoft.AspNetCore.Mvc;
using MonitoriaServicosApi.Business;
using MonitoriaServicosApi.Business.Interface;
using Newtonsoft.Json;
using System;

namespace MonitoriaServicosApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LogErroController : ControllerBase
    {
        private readonly ILogErroBusiness _logErroBusiness;

        public LogErroController()
        {
            _logErroBusiness = new LogErroBusiness();
        }

        [Route("GetLogErrosServico/{idServico}/{origem}")]
        [HttpGet]
        public ActionResult GetLogErrosServico(string idServico, string origem)
        {
            try
            {
                return Ok(_logErroBusiness.GetLogErrosServico(idServico, origem));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("GetLogErrosServicoPag")]
        [HttpGet]
        public ActionResult GetLogErrosServico(string idServico, int pagina)
        {
            try
            {
                return Ok(_logErroBusiness.GetLogErrosServicoPag(idServico, pagina));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("GetQtdErroGroup")]
        [HttpGet]
        public ActionResult GetQtdErroGroup(string idServico)
        {
            try
            {
                return Ok(_logErroBusiness.GetQtdErroGroup(idServico));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("AtualizaStatusLog")]
        [HttpPost]
        public ActionResult AtualizaStatusLog(object logErro)
        {
            try
            {
                dynamic informacoes = JsonConvert.DeserializeObject<object>(logErro.ToString());
                return Ok(_logErroBusiness.AtualizaStatusLog(informacoes));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("GetSolucionarErrosServico/{idServico}")]
        [HttpGet]
        public ActionResult GetSolucionarErrosServico(string idServico)
        {
            try
            {
                
                return Ok(_logErroBusiness.SolucionarErrosServico(idServico));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}