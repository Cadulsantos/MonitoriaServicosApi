using Microsoft.AspNetCore.Mvc;
using MonitoriaServicos.Business;
using MonitoriaServicos.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoriaServicos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogExecucaoController : ControllerBase
    {
        private readonly ILogExecucaoBusiness _logExecucaoBusiness;

        public LogExecucaoController()
        {
            _logExecucaoBusiness = new LogExecucaoBusiness();
        }

        [Route("GetLogsExecucaoServico")]
        [HttpGet]
        public ActionResult GetLogsExecucaoServico(string idServico, int pagina)
        {
            try
            {
                return Ok(_logExecucaoBusiness.GetLogsExecucaoServico(idServico, pagina));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("GetQtdLogsExec")]
        [HttpGet]
        public ActionResult GetQtdLogsExec(string idServico)
        {
            try
            {
                return Ok(_logExecucaoBusiness.GetQtdLogsExec(idServico));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
