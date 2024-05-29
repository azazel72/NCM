using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public class Centralitas : Entidades, IEntidades
    {

        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.listado.Add(new Centralita(resultado));
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
            Centralita a = new Centralita(parametros);
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertCentralita, a.GetValoresInsertSQL());
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

        public Centralita BuscarAlias(string alias)
        {
            foreach (Centralita c in this.listado)
            {
                if (c.alias == alias)
                {
                    return c;
                }
            }
            return null;
        }
    }

    public class Centralita : Entidad, IEntidad
    {
        public string alias { get; set; }
        public string nombre { get; set; }
        public string ip { get; set; }

        public Centralita(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.alias = parametros.Buscar("alias");
            this.nombre = parametros.Buscar("nombre");
            this.ip = Gestor.ipTest;
        }
        public Centralita()
        {

        }
        public Centralita(int id, string alias, string nombre)
        {
            this.id = id;
            this.alias = alias;
            this.nombre = nombre;
            this.ip = Gestor.ipTest;
        }
        public Centralita(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.alias = datos.GetString("alias");
            this.nombre = datos.GetString("nombre");
            this.ip = Gestor.ipTest;
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// </summary>
        /// <returns></returns>
        public object[] GetValores()
        {
            object[] valores = {
                this.id,
                this.alias,
                this.nombre,
                this.ip
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
                this.alias,
                this.nombre,
                this.ip
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
            return string.Format("('{0}','{1}')", this.alias, this.nombre);
        }

        /// <summary>
        /// Esta funcion actualiza directamente en la BBDD
        /// Devuelve el resultado.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public Respuesta UpdateSQL(Parametros parametros)
        {
            Respuesta r = Respuesta.Crear(false, 0, (Centralita)this);
            int id = parametros.BuscarInt("id");
            string alias = parametros.Buscar("alias");
            string nombre = parametros.Buscar("nombre");
            object[] valores = {
                id,
                alias,
                nombre
            };
            if (!this.Comparar(id))
            {
                r.Error("ID no coincidente");
            }
            else
            {
                bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updateCentralita, valores);
                if (ok)
                {
                    this.alias = alias;
                    this.nombre = nombre;
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
