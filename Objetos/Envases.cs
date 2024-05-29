using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public class Envases : Entidades, IEntidades
    {

        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.listado.Add(new Envase(resultado));
            }
        }

        /// <summary>
        /// Crea un elemento en la BBDD y lo agrega al listado
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Respuesta InsertSQL(Parametros parametros)
        {
            Respuesta r = new Respuesta();
            int id = parametros.BuscarInt("id");
            if (id > 0) return r.Error("Comando Crear incompatible debe tener ID nulo");
            Envase a = new Envase(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertEnvase, a.GetValoresInsertSQL());
            if (id > 0)
            {
                a.id = id;
                this.Agregar(a);
                r.entidad = a;
            }
            else
            {
                r.error = false;
                r.mensaje = "Insert SQL falló.";
            }
            return r;
        }

    }

    public class Envase : Entidad, IEntidad
    {
        public string nombre { get; set; }
        public int ancho { get; set; }
        public int largo { get; set; }
        public int alto { get; set; }
        public int volumen { get; set; }
        public int peso { get; set; }

        public Envase(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.nombre = parametros.Buscar("nombre");
            this.ancho = parametros.BuscarInt("ancho");
            this.largo = parametros.BuscarInt("largo");
            this.alto = parametros.BuscarInt("alto");
            this.volumen = parametros.BuscarInt("volumen");
            this.peso = parametros.BuscarInt("peso");
        }
        public Envase()
        {

        }
        public Envase(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.nombre = datos.GetString("nombre");
            this.ancho = datos.GetInt32("ancho");
            this.largo = datos.GetInt32("largo");
            this.alto = datos.GetInt32("alto");
            this.volumen = datos.GetInt32("volumen");
            this.peso = datos.GetInt32("peso");
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// </summary>
        /// <returns></returns>
        public object[] GetValores()
        {
            object[] valores = {
                this.id,
                this.nombre,
                this.ancho,
                this.largo,
                this.alto,
                this.volumen,
                this.peso
            };
            return valores;
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// Adaptado para llenar las tablas.
        /// </summary>
        /// <returns></returns>
        public object[] GetValoresTablas()
        {
            object[] valores = {
                this.id,
                this.nombre,
                this.ancho,
                this.largo,
                this.alto,
                this.volumen,
                this.peso
            };
            return valores;
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// Adaptado para crear consultas SQL
        /// </summary>
        /// <returns></returns>
        public string GetValoresInsertSQL()
        {
            return string.Format("('{0}', {1}, {2}, {3}, {4}, {5})",
                this.nombre,
                this.ancho,
                this.largo,
                this.alto,
                this.volumen,
                this.peso
                );
        }

        /// <summary>
        /// Esta funcion actualiza directamente en la BBDD
        /// Devuelve el resultado.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Respuesta UpdateSQL(Parametros parametros)
        {
            Respuesta r = Respuesta.Crear(false, 0, (Envase)this);
            int id = parametros.BuscarInt("id");
            string nombre = parametros.Buscar("nombre");
            int ancho = parametros.BuscarInt("ancho");
            int largo = parametros.BuscarInt("largo");
            int alto = parametros.BuscarInt("alto");
            int volumen = parametros.BuscarInt("volumen");
            int peso = parametros.BuscarInt("peso");

            object[] valores = {
                id,
                nombre,
                ancho,
                largo,
                alto,
                volumen,
                peso
            };
            if (!this.Comparar(id))
            {
                r.Error("ID no coincidente");
            }
            else
            {
                bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updateEnvase, valores);
                if (ok)
                {
                    this.id = id;
                    this.nombre = nombre;
                    this.ancho = ancho;
                    this.largo = largo;
                    this.alto = alto;
                    this.volumen = volumen;
                    this.peso = peso;
                }
                else
                {
                    r.error = false;
                    r.mensaje = "Actualización SQL falló.";
                }
            }
            return r;
        }

    }
}
