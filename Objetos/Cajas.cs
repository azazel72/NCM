using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public class Cajas : List<Caja>
    {
        public Caja BuscarCodigo(string codigo)
        {
            return this.FirstOrDefault(x => x.codigo == codigo);
        }
    }

    public class Caja : IEntidad
    {
        public string codigo { get; set; }
        public int color { get; set; }

        public object[] GetValores()
        {
            throw new System.NotImplementedException();
        }

        public string GetValoresInsertSQL()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Devuelve un array de objetos con los valores.
        /// Adaptado para llenar las tablas.
        /// </summary>
        /// <returns></returns>
        public object[] GetValoresTablas()
        {
            object[] valores = {
            this.codigo,
            this.color
            };
            return valores;
        }

        public Respuesta UpdateSQL(Parametros parametros)
        {
            throw new System.NotImplementedException();
        }
    }
}
