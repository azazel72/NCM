using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;
using System;

namespace NoCocinoMas
{
    public class Bloqueos : IEntidades
    {
        List<Bloqueo> bloqueos = new List<Bloqueo>();

        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.bloqueos.Add(new Bloqueo(resultado));
            }
        }

        public Bloqueo BuscarMarca(int alias, string tarea)
        {
            foreach (Bloqueo b in this.bloqueos) {
                if (b.modulo_alias.Equals(alias) && b.tarea_id.Equals(tarea))
                {
                    return b;
                }
            }
            return null;
        }

        public Respuesta InsertSQL(Parametros parametros)
        {
            return null;
        }

        public void Vaciar()
        {
            this.bloqueos.Clear();
        }

        public string ValoresSQL()
        {
            return null;
        }
    }

    public class Bloqueo
    {
        public int modulo_alias { get; set; }
        public string tarea_id { get; set; }
        public int linea { get; set; }
        public int led_inicial { get; set; }
        public int ancho { get; set; }
        public int primera_linea { get; set; }

        public Bloqueo(MySqlDataReader datos)
        {
            this.modulo_alias = datos.GetInt32("modulo_alias");
            this.tarea_id = datos.GetString("tarea_id");
            this.linea = datos.GetInt32("linea");
            this.led_inicial = datos.GetInt32("led_inicial");
            this.ancho = datos.GetInt32("ancho");
            this.primera_linea = datos.GetInt32("primera_linea");
        }

    }
}
