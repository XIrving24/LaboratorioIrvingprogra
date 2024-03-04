using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio__2
{
    internal class URL
    {
        string pagina;
        int veces;
        DateTime fecha;
        string link;

        public string Pagina { get => pagina; set => pagina = value; }
        public int Veces { get => veces; set => veces = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public string Link { get => link; set => link = value; }
    }
}
