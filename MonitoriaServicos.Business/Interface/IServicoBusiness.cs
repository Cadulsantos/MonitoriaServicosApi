﻿using MonitoriaServicosApi.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace  MonitoriaServicosApi.Business.Interface
{
    public interface IServicoBusiness
    {
        List<dynamic> GetServicos();

        List<dynamic> GetLogsExecucaoServico(string idServico);

        bool AtualizaServico(dynamic informacoes);

        List<dynamic> GetServicosFiltro(dynamic filtroServico);
    }
}
