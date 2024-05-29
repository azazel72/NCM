using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoCocinoMas
{
    public class Respuesta
    {
        public bool error { get; set; }
        public int id { get; set; }
        public object entidad { get; set; }
        public string mensaje { get; set; }
        public object objeto { get; set; }

        public Respuesta(bool valor = false)
        {
            this.error = valor;
        }

        public Respuesta Error(string mensaje)
        {
            this.error = true;
            this.mensaje = mensaje;
            return this;
        }

        public static Respuesta Crear(bool e = false, int i = 0, Entidad en = null, string m = "", object o = null)
        {
            Respuesta r = new Respuesta();
            r.error = e;
            r.id = i;
            r.entidad = en;
            r.mensaje = m;
            r.objeto = o;
            return r;
        }
    }
}
