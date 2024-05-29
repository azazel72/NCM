using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace NoCocinoMas
{
    public interface IEntidades
    {
        void Agregar(MySqlDataReader resultado);
        void Vaciar();
        string ValoresSQL();
        Respuesta InsertSQL(Parametros parametros);
    }

    public interface IEntidad
    {
        object[] GetValores();
        object[] GetValoresTablas();
        string GetValoresInsertSQL();
        Respuesta UpdateSQL(Parametros parametros);
    }
}
