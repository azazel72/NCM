using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public class Ubicaciones : Entidades, IEntidades
    {

        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.listado.Add(new Ubicacion(resultado));
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
            Ubicacion a = new Ubicacion(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertUbicacion, a.GetValoresInsertSQL());
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

    public class Ubicacion : Entidad, IEntidad
    {
        public string nombre { get; set; }
        public int linea { get; set; }
        public int inicio { get; set; }
        public int ancho { get; set; }

        public Ubicacion(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.nombre = parametros.Buscar("nombre");
            this.linea = parametros.BuscarInt("linea");
            this.inicio = parametros.BuscarInt("inicio");
            this.ancho = parametros.BuscarInt("ancho");
        }
        public Ubicacion()
        {

        }
        public Ubicacion(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.nombre = datos.GetString("nombre");
            this.linea = datos.GetInt32("linea");
            this.inicio = datos.GetInt32("inicio");
            this.ancho = datos.GetInt32("ancho");
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
                this.linea,
                this.inicio,
                this.ancho
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
                this.linea,
                this.inicio,
                this.ancho
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
            return string.Format("('{0}', {1}, {2}, {3})",
                this.nombre,
                this.linea,
                this.inicio,
                this.ancho
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
            Respuesta r = Respuesta.Crear(false, 0, (Ubicacion)this);
            int id = parametros.BuscarInt("id");
            string nombre = parametros.Buscar("nombre");
            int linea = parametros.BuscarInt("linea");
            int inicio = parametros.BuscarInt("inicio");
            int ancho = parametros.BuscarInt("ancho");

            object[] valores = {
                id,
                nombre,
                linea,
                inicio,
                ancho
            };
            if (!this.Comparar(id))
            {
                r.Error("ID no coincidente");
            }
            else
            {
                bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updateUbicacion, valores);
                if (ok)
                {
                    this.id = id;
                    this.nombre = nombre;
                    this.linea = linea;
                    this.inicio = inicio;
                    this.ancho = ancho;
                }
                else
                {
                    r.error = false;
                    r.mensaje = "Actualización SQL falló.";
                }
            }
            return r;
        }

        public string ToCSVCalculate()
        {
            //calculamos el inicio segun el valor a encender
            int offset = (8 - ((this.ancho / 5) + (this.ancho % 5))) / 2;
            if (offset < 0) offset = 0;
            // Sumar los grupos completos y el residuo para obtener el resultado
            return string.Format("{0},{1},{2}",
                this.linea,
                this.inicio - offset,
                this.ancho
                );
        }
        public string ToCSV()
        {
            return string.Format("{0},{1},{2}",
                this.linea,
                this.inicio,
                this.ancho
                );
        }
        public string ToCSV(int i)
        {
            return string.Format("{0},{1},{2}",
                this.linea,
                this.inicio-1,
                i
                );
        }

    }
}
