using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorFinal
{
    public class Polish
    {
        private string lexema;
        private string salto;
        private string direccionamiento;

        public string Lexema
        {
            get
            {
                return lexema;
            }

            set
            {
                lexema = value;
            }
        }

        public string Salto
        {
            get
            {
                return salto;
            }

            set
            {
                salto = value;
            }
        }

        public string Direccionamiento
        {
            get
            {
                return direccionamiento;
            }

            set
            {
                direccionamiento = value;
            }
        }
    }
}
