using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace NoCocinoMas
{
    public class Operarios : Entidades, IEntidades
    {
        public bool ValidarClave(object id, string pin)
        {
            Operario o = (Operario) this.BuscarId(id);
            return (o != null && o.pin.Equals(pin));
        }

        public void VincularRoles(Roles roles)
        {
            foreach (Operario operario in this.listado)
            {
                operario.VincularRol(roles);
            }
        }

        /// <summary>
        /// Agregamos el conjunto de objetos de una consulta MySql
        /// </summary>
        /// <param name="resultado"></param>
        public void Agregar(MySqlDataReader resultado)
        {
            while (resultado.Read())
            {
                this.listado.Add(new Operario(resultado));
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
            Operario a = new Operario(parametros);
            if (Gestor.gestor.roles.BuscarId(a.rol_id) == null) r.Error("Rol no válido.");
            id = ConectorSQL.CrearEntidades(ConectorSQL.insertOperario, a.GetValoresInsertSQL());
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

    public class Operario : Entidad, IEntidad
    {
        public string nombre { get; set; }
        public int rol_id { get; set; }
        public string pin { get; set; }
        public int almacen_id { get; set; }
        public int color { get; set; }
        public int accion { get; set; }
        //objetos
        //[JsonIgnore]
        public Rol rol { get; set; }

        public Operario(Parametros parametros)
        {
            this.id = parametros.BuscarInt("id");
            this.nombre = parametros.Buscar("nombre");
            this.rol_id = parametros.BuscarInt("rol_id");
            this.pin = parametros.Buscar("pin");
            this.almacen_id = 0;
            this.color = 0;
            this.accion = 0;
        }
        public Operario(int id, string nombre, int rol_id, string pin, int almacen_id = 0, int color = 0, int accion = 0)
        {
            this.id = id;
            this.nombre = nombre;
            this.rol_id = rol_id;
            this.pin = pin;
            this.almacen_id = almacen_id;
            this.color = color;
            this.accion = accion;
        }
        public Operario(MySqlDataReader datos)
        {
            this.id = datos.GetInt32("id");
            this.nombre = datos.GetString("nombre");
            this.rol_id = datos.GetInt32("rol_id");
            this.pin = datos.GetString("pin");
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
                this.rol_id,
                this.pin
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
                this.rol_id,
                this.pin
            };
            return valores;
        }

        /// <summary>
        /// Devuelve un string con los valores necesarios para un insert SQL.
        /// </summary>
        /// <returns></returns>
        public string GetValoresInsertSQL()
        {
            return string.Format("('{0}', {1}, '{2}')",
                this.nombre,
                this.rol_id,
                this.pin
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
            Respuesta r = Respuesta.Crear(false, 0, (Operario) this);
            int id = parametros.BuscarInt("id");
            string nombre = parametros.Buscar("nombre");
            int rol_id = parametros.BuscarInt("rol_id");
            string pin = parametros.Buscar("pin");
            object[] valores = {
                id,
                nombre,
                rol_id,
                pin
            };
            if (!this.Comparar(id))
            {
                r.Error("ID no coincidente");
            }
            else
            {
                bool ok = ConectorSQL.ActualizarEntidades(ConectorSQL.updateOperario, valores);
                if (ok)
                {
                    this.nombre = nombre;
                    this.rol_id = rol_id;
                    this.pin = pin;
                }
                else
                {
                    r.error = false;
                    r.mensaje = "Actualización SQL falló.";
                }
            }
            return r;

        }

        /// <summary>
        /// Asocia el rol correspondiente de un listado
        /// Esta opcion no está integrada en el constructor para no obligar a disponer de los Roles desde un principio
        /// </summary>
        /// <param name="roles"></param>
        public void VincularRol(Roles roles)
        {
            this.rol = (Rol)roles.BuscarId(this.rol_id);
            //this.rol = (Rol) (from rol in roles.roles where rol.id.Equals(this.rol_id) select rol).Single();
        }

        public void VincularRol(Rol rol)
        {
            this.rol = rol;
        }

    }
}
