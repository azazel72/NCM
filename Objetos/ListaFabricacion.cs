using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoCocinoMas
{
    public class ListaFabricacion : List<ProductoFabricar>
    {

    }

    public class ProductoFabricar
    {
        public int indice { get; set; }
        public string codigo { get; set; }
        public string cantidad { get; set; }

        public ProductoFabricar()
        {

        }
    }

}
