using Microsoft.AspNetCore.Mvc;
using MonitoriaServicosApi.Business;
using MonitoriaServicosApi.Business.Interface;
using Newtonsoft.Json;
using System;

namespace MonitoriaServicosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicoController : ControllerBase
    {
        private readonly IServicoBusiness _servicoBusiness;

        public ServicoController()
        {
            _servicoBusiness = new ServicoBusiness();
        }

        [Route("GetServicos")]
        [HttpGet]
        public ActionResult GetServicos()
        {
            try
            {
                return Ok(_servicoBusiness.GetServicos());
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
        }

        [Route("GetServicosFiltro")]
        [HttpPost]
        public ActionResult GetServicosFiltro(object filtroServico)
        {
            try
            {
                dynamic filtro = JsonConvert.DeserializeObject<object>(filtroServico.ToString());
                return Ok(_servicoBusiness.GetServicosFiltro(filtro));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("AtualizaServico")]
        [HttpPost]
        public ActionResult AtualizaServico(object servico)
        {
            try
            {
                dynamic informacoes = JsonConvert.DeserializeObject<object>(servico.ToString());
                return Ok(_servicoBusiness.AtualizaServico(informacoes));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("GetLogsExecucaoServico/{idServico}")]
        [HttpGet]
        public ActionResult GetLogsExecucaoServico(string idServico)
        {
            try
            {
                return Ok(_servicoBusiness.GetLogsExecucaoServico(idServico));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}