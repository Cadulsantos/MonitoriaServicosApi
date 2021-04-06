using System;
using System.Collections.Generic;
using System.Text;

namespace MonitoriaServicosApi.Business.Interface
{
    public interface IUsuarioBusiness
    {
        object AutenticacaoUsuario(dynamic usuario);
    }
}
