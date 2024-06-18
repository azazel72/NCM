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

    public class Caja
    {
        public string codigo { get; set; }
        public int color { get; set; }
    }
}
