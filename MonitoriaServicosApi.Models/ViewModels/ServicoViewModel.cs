using MonitoriaServicosApi.Models.Models;
using MonitoriaServicosApi.Models.Models.Enum;
using System;
using System.Collections.Generic;

namespace MonitoriaServicos.Models.ViewModels
{
    public class ServicoViewModel
    {
        public string Id { get; set; }

        public string Nome { get; set; }
        
        public string NomeArgument { get; set; }

        public bool Ativo { get; set; }

        public string Descricao { get; set; }

        public string Periodicidade { get; set; }

        public DateTime Data { get; set; }

        public string DataInicio { get; set; }

        public string DataFim { get; set; }

        public OrigemServicoEnum Origem { get; set; }

        public IEnumerable<LogErroServico> LogsErro { get; set; }

        public int QuantidadeErros { get; set; }

        public bool Erro { get; set; }

    }
}
