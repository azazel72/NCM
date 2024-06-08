using Conexiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NoCocinoMas.Objetos
{
    public class MensajeClienteServidor
    {
        [JsonIgnore]
        public string id_conexion { get; set; }
        [JsonIgnore]
        public ConexionPermanente conexion { get; set; }
        public string accion { get; set; }
        public string caja { get; set; }
        public int indice_modulo { get; set; }
        public int transportista { get; set; }
        public int numero_pedido { get; set; }
        public string mensaje { get; set; }
        public string mensajeError { get; set; }
        public object datos { get; set; }
        public int codigo_producto { get; set; }
        public bool encender_posiciones { get; set; }
        public bool mostrar_pedidos { get; set; }
    }
}
