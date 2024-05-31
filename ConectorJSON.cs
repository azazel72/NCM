using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NoCocinoMas
{
    public class ConectorJSON
    {
        public static void GuardarObjeto(string ruta, object objeto)
        {
            string json = JsonSerializer.Serialize(objeto);
            System.IO.File.WriteAllText(ruta, json);
        }

        public static T CargarObjeto<T>(string ruta)
        {
            T t = default(T);
            try
            {
                string jsonString = System.IO.File.ReadAllText(ruta);
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return t;
        }
    }
}
