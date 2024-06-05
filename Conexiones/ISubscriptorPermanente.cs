using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Conexiones
{
    public interface ISubscriptorPermanente
    {
        void NuevaConexion(ConexionPermanente conexion);
        void ConexionCerrada(ConexionPermanente conexion);
        void MensajeEntrante(ConexionPermanente conexion, string mensaje);
    }
}
