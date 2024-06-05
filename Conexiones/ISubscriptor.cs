using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexiones
{
    public interface ISubscriptor
    {
        Task<string> MensajeRecibido(string[] comandos, Dictionary<string, string> parametros, string cuerpo);
        void ConexionEstablecida(string respuesta);
        void ConexionCerrada(string respuesta);
        void Excepcion(Exception e);
        void LogInformation(string log);
        void LogDebug(string log);
        void LogError(string log);
    }
}
