using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoCocinoMas
{
    public class CompletarPedido
    {
        public int numero_pedido { get; set; }
        public List<CompletarLinea> lineas { get; set; }
    }

    public class CompletarLinea
    {
        public int id { get; set; }
        public int codigo { get; set; }
        public string lote { get; set; }
        public int completar{ get; set; }
    }

}
