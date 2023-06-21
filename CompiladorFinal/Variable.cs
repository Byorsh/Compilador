using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorFinal
{
    public class Variable
    {
        private string lexema;
        private int tipoVariable;

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

        public int TipoVariable
        {
            get
            {
                return tipoVariable;
            }

            set
            {
                tipoVariable = value;
            }
        }
    }
}
