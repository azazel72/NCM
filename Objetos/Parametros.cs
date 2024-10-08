using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoCocinoMas
{
    public class Parametros : List<Parametro>
    {
        public string Buscar(string nombre)
        {
            Parametro par = this.Find(p => p.name == nombre);
            return par != null ? par.value : null;
        }

        public int BuscarInt(string nombre)
        {
            Parametro par = this.Find(p => p.name == nombre);
            try
            {
                return par?.value != null ? int.Parse(par.value) : 0;
            }
            catch
            {
                return 0;
            }
        }

        public long BuscarLong(string nombre)
        {
            Parametro par = this.Find(p => p.name == nombre);
            try
            {
                return par?.value != null ? long.Parse(par.value) : 0;
            }
            catch
            {
                return 0;
            }
        }


        public DateTime BuscarFecha(string nombre)
        {
            Parametro par = this.Find(p => p.name == nombre);
            try
            {
                return par.value != null ? DateTime.Parse(par.value) : default(DateTime);
            }
            catch
            {
                return default(DateTime);
            }
        }

    }

    public class Parametro
    {
        public string name { get; set; }
        public string value { get; set; }

        public Parametro()
        {

        }
        public Parametro(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }

}
