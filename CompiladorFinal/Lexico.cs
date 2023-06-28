using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompiladorFinal
{
    class Lexico
    {
        public List<Error> listaError; //SALIDA
        public List<Token> listaToken; //atributo es la SALIDA del lexico.
        public List<Variable> listaVariables;
        public List<Polish> listaPolish;
        Sintactico sintaxis = new Sintactico();

        private string codigoFuente;    // atributo que representa la ENTRADA del lexico.
        private int linea;

        private int[,] matrizTransicion =
        {
//             0     1     2     3     4     5     6     7     8     9    10    11    12    13    14    15    16    17    18    19    20    21    22    23    24    25    26    27    28    29    30    31    32

//          | dig | let |  _  |  .  |  "  |	 '	|  +  |  -  |  *  |  /  |  <  |  >  |  =  |  !  |  |  |  {  |  }  |  (  |  )  |  [  |  ]  |  ,  |  ;  |  :  |  ?  |  #  |  &  |  \  | esp | ent | tab | eof | DESC |

/* q0 */    {  2  ,  1  , -40 , -36 ,  5  ,  7  ,  9  ,  10 ,  11 ,  12 ,  17 ,  18 ,  16 ,  13 ,  15 , -29 , -30 , -27 , -28 , -25 , -26 , -41 , -31 , -32 , -39 , -42 ,  14 , -43 ,  0  ,  0  ,  0  ,  0  , -500, },
/* q1 */	{  1  ,  1  ,  1  , -1  , -500, -500,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 ,  -1 , -500, },
/* q2 */	{  2  , -501, -501,  3  , -501, -501,  -2 ,  -2 ,  -2 ,  -2 ,  -2 ,  -2 ,  -2 , -501,  -2 , -501,  -2 , -501,  -2 , -501,  -2 ,  -2 ,  -2 ,  -2 , -501, -501,  -2 , -501,  -2 ,  -2 ,  -2 ,  -2 , -500, },
/* q3 */    {  4  , -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -501, -500, },
/* q4 */    {  4  , -502, -502, -502, -502, -502,  -3 ,  -3 ,  -3 ,  -3 ,  -3 ,  -3 ,  -3 , -502,  -3 , -502,  -3 , -502,  -3 , -502,  -3 ,  -3 ,  -3 ,  -3 , -502, -502,  -3 , -502,  -3 , -503, -503, -503, -500, },
/* q5 */    {  6  ,  6  ,  6  ,  6  ,  -4 ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  , -503, -503, -503, -500, },
/* q6 */    {  6  ,  6  ,  6  ,  6  ,  -4 ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  ,  6  , -503, -503, -503, -500, },
/* q7 */    {  8  ,  8  ,  8  ,  8  ,  8  ,  -5 ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  ,  8  , -504, -504, -504, -500, },
/* q8 */    { -504, -504, -504, -504, -504,  -5 , -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -504, -500, },
/* q9 */    {  -6 ,  -6 , -500, -500,  -6 ,  -6 , -17 , -500, -500, -500, -500, -500, -10 , -500, -500, -500, -500,  -6 , -500,  -6 , -500, -500, -500, -500, -500, -500, -500, -500,  -6 , -500, -500, -500, -500, },
/* q10 */   {  -7 ,  -7 , -500, -500,  -7 ,  -7 , -500, -18 , -500, -500, -500, -500, -11 , -500, -500, -500, -500,  -7 , -500,  -7 , -500, -500, -500, -500, -500, -500, -500, -500,  -7 , -500, -500, -500, -500, },
/* q11 */   {  -8 , -8  , -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -12 , -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -8  , -8  , -500, -8  , -500, },
/* q12 */   {  -9 , -9  , -500, -500, -500, -500, -500, -500,  20 ,  19 , -500, -500, -13 , -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -9  , -9  , -500, -9  , -500, },
/* q13 */   { -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -20 , -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -14 , -14 , -14 , -14 , -500, },
/* q14 */   { -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -15 , -505, -505, -505, -505, -505, -505, },
/* q15 */   { -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -16 , -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, -505, },
/* q16 */   { -37 , -37 , -500, -500, -37 , -37 , -500, -500, -500, -500, -500, -500, -19 , -500, -500, -500, -500, -37 , -500, -37 , -500, -500, -500, -500, -500, -500, -500, -500, -37 , -37 , -37 , -37 , -500, },
/* q17 */   { -21 , -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -23 , -500, -500, -500, -500, -21 , -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -21 , -500, -500, -500, -500, },
/* q18 */   { -22 , -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -24 , -500, -500, -500, -500, -22 , -500, -500, -500, -500, -500, -500, -500, -500, -500, -500, -22 , -500, -500, -500, -500, },
/* q19 */   {  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 ,  19 , -50 ,  19 ,  19 , -500, },
/* q20 */   {  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  21 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 , -500, -500, },
/* q21 */   {  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 , -51 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 ,  20 , -500, -500, }


        };

        /// <summary>
        /// Cosntructor 
        /// </summary>
        /// <param name="codigo">el contenido del archivo que abrimos</param>
        public Lexico()
        {

        } //constructor


        /// <summary>
        /// metodo para regresar el token de la palabra reservada
        /// </summary>
        /// <param name="lexema">cadena que compone el token</param>
        /// <returns></returns>
        private int esPalabraReservada(string lexema)
        {
            switch (lexema)
            {
                case "int":
                    return -55;
                case "char":
                    return -56;
                case "double":
                    return -57;
                case "string":
                    return -58;
                case "float":
                    return -59;
                case "si":
                    return -60;
                case "para":
                    return -61;
                case "hacer":
                    return -62;
                case "mientras":
                    return -63;
                case "intentar":
                    return -64;
                case "atrapar":
                    return -65;
                case "sino":
                    return -66;
                case "romper":
                    return -67;
                case "privado":
                    return -68;
                case "publico":
                    return -69;
                case "estatico":
                    return -70;
                case "void":
                    return -71;
                case "nuevo":
                    return -72;
                case "clase":
                    return -73;
                case "como":
                    return -74;
                case "protegido":
                    return -75;
                case "caso":
                    return -76;
                case "switch":
                    return -77;
                case "usando":
                    return -78;
                case "porDefecto":
                    return -79;
                case "regresar":
                    return -80;
                case "read":
                    return -81;
                case "write":
                    return -82;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// es el metodo que me regrese el siguiente caracter del codigo fuente
        /// </summary>
        /// <returns></returns>
        private char SiguienteCaracter(int i)
        {
            return Convert.ToChar(codigoFuente.Substring(i, 1));
        }

        private int RegresarColumna(char caracter)
        {

            if (char.IsDigit(caracter))
            {
                return 0;
            }
            else if (char.IsLetter(caracter))
            {
                return 1;
            }
            else if (caracter.Equals('_'))
            {
                return 2;
            }
            else if (caracter.Equals('.'))
            {
                return 3;
            }
            else if (caracter.Equals('"'))
            {
                return 4;
            }
            else if (caracter.Equals('\''))
            {
                return 5;
            }
            else if (caracter.Equals('+'))
            {
                return 6;
            }
            else if (caracter.Equals('-'))
            {
                return 7;
            }
            else if (caracter.Equals('*'))
            {
                return 8;
            }
            else if (caracter.Equals('/'))
            {
                return 9;
            }
            else if (caracter.Equals('<'))
            {
                return 10;
            }
            else if (caracter.Equals('>'))
            {
                return 11;
            }
            else if (caracter.Equals('='))
            {
                return 12;
            }
            else if (caracter.Equals('!'))
            {
                return 13;
            }

            else if (caracter.Equals('|'))
            {
                return 14;
            }
            else if (caracter.Equals('{'))
            {
                return 15;
            }

            else if (caracter.Equals('}'))
            {
                return 16;
            }
            else if (caracter.Equals('('))
            {
                return 17;
            }

            else if (caracter.Equals(')'))
            {
                return 18;
            }
            else if (caracter.Equals('['))
            {
                return 19;
            }

            else if (caracter.Equals(']'))
            {
                return 20;
            }
            else if (caracter.Equals(','))
            {
                return 21;
            }

            else if (caracter.Equals(';'))
            {
                return 22;
            }
            else if (caracter.Equals(':'))
            {
                return 23;
            }

            else if (caracter.Equals('?'))
            {
                return 24;
            }
            else if (caracter.Equals('#'))
            {
                return 25;
            }

            else if (caracter.Equals('&'))
            {
                return 26;
            }

            else if (caracter.Equals('\n'))//enter
            {
                return 29;
            }


            else if (char.IsWhiteSpace(caracter))
            {
                return 28;
            }

            else if (caracter.Equals('\t')) //tab
            {
                return 30;
            }

            else  //simbolo desconocido
            {
                return 32;
            }


        }

        private TipoToken esTipo(int estado)
        {
            switch (estado)
            {
                case -1:
                    return TipoToken.Identificador;

                case -2:
                    return TipoToken.Numeroentero;
                case -3:
                    return TipoToken.Numerodecimal;
                case -4:
                    return TipoToken.Cadena;
                case -5:
                    return TipoToken.Caracter;
                case -6:
                    return TipoToken.OperadorAritmetico;
                case -7:
                    return TipoToken.OperadorAritmetico;
                case -8:
                    return TipoToken.OperadorAritmetico;
                case -9:
                    return TipoToken.OperadorAritmetico;
                case -10:
                    return TipoToken.OperadorAritmetico;
                case -11:
                    return TipoToken.OperadorAritmetico;
                case -12:
                    return TipoToken.OperadorAritmetico;
                case -13:
                    return TipoToken.OperadorAritmetico;
                case -14:
                    return TipoToken.OperadorLogico;
                case -15:
                    return TipoToken.OperadorLogico;
                case -16:
                    return TipoToken.OperadorLogico;
                case -17:
                    return TipoToken.OperadorAritmetico;
                case -18:
                    return TipoToken.OperadorAritmetico;
                case -19:
                    return TipoToken.OperadorRelacional;
                case -20:
                    return TipoToken.OperadorRelacional;
                case -21:
                    return TipoToken.OperadorRelacional;
                case -22:
                    return TipoToken.OperadorRelacional;
                case -23:
                    return TipoToken.OperadorRelacional;
                case -24:
                    return TipoToken.OperadorRelacional;
                case -25:
                    return TipoToken.SimboloSimple;
                case -26:
                    return TipoToken.SimboloSimple;
                case -27:
                    return TipoToken.SimboloSimple;
                case -28:
                    return TipoToken.SimboloSimple;
                case -29:
                    return TipoToken.SimboloSimple;
                case -30:
                    return TipoToken.SimboloSimple;
                case -31:
                    return TipoToken.SimboloSimple;
                case -32:
                    return TipoToken.SimboloSimple;
                case -33:
                    return TipoToken.SimboloSimple;
                case -34:
                    return TipoToken.SimboloSimple;
                case -35:
                    return TipoToken.SimboloSimple;
                case -36:
                    return TipoToken.SimboloSimple;
                case -37:
                    return TipoToken.OperadorAsignacion;
                case -38:
                    return TipoToken.SimboloSimple;
                case -39:
                    return TipoToken.SimboloSimple;
                case -40:
                    return TipoToken.SimboloSimple;
                case -41:
                    return TipoToken.SimboloSimple;
                case -42:
                    return TipoToken.SimboloSimple;
                case -43:
                    return TipoToken.SimboloSimple;
                case -50:
                    return TipoToken.Comentario;
                case -51:
                    return TipoToken.ComentarioLargo;
                case -55:
                    return TipoToken.PalabraReservada;
                case -56:
                    return TipoToken.PalabraReservada;
                case -57:
                    return TipoToken.PalabraReservada;
                case -58:
                    return TipoToken.PalabraReservada;
                case -59:
                    return TipoToken.PalabraReservada;
                case -60:
                    return TipoToken.PalabraReservada;
                case -61:
                    return TipoToken.PalabraReservada;
                case -62:
                    return TipoToken.PalabraReservada;
                case -63:
                    return TipoToken.PalabraReservada;
                case -64:
                    return TipoToken.PalabraReservada;
                case -65:
                    return TipoToken.PalabraReservada;
                case -66:
                    return TipoToken.PalabraReservada;
                case -67:
                    return TipoToken.PalabraReservada;
                case -68:
                    return TipoToken.PalabraReservada;
                case -69:
                    return TipoToken.PalabraReservada;
                case -70:
                    return TipoToken.PalabraReservada;
                case -71:
                    return TipoToken.PalabraReservada;
                case -72:
                    return TipoToken.PalabraReservada;
                case -73:
                    return TipoToken.PalabraReservada;
                case -74:
                    return TipoToken.PalabraReservada;
                case -75:
                    return TipoToken.PalabraReservada;
                case -76:
                    return TipoToken.PalabraReservada;
                case -77:
                    return TipoToken.PalabraReservada;
                case -78:
                    return TipoToken.PalabraReservada;
                case -79:
                    return TipoToken.PalabraReservada;
                case -80:
                    return TipoToken.PalabraReservada;
                case -81:
                    return TipoToken.PalabraReservada;
                case -82:
                    return TipoToken.PalabraReservada;
                default:
                    return TipoToken.NADA;

            }
        }

        private Error ManejoErrores(int estado)
        {
            string mensajeError;

            switch (estado)
            {
                case -499:
                    mensajeError = "simbolo desconocido";
                    break;
                case -500:
                    mensajeError = "Esperaba un identificador valido";
                    break;
                case -501:
                    mensajeError = "Formato incorrecto : Se esperaba un numero entero";
                    break;
                case -502:
                    mensajeError = "Formato incorrecto : Se esperaba un numero decimal";
                    break;
                case -503:
                    mensajeError = "Formato incorrecto : Se esperaba una cadena o falto cerrar cadena";
                    break;
                case -504:
                    mensajeError = "Formato incorrecto : Se esperaba una caracter o falto cerrar caracter";
                    break;
                case -505:
                    mensajeError = "Funcion incompleta : Falta un simbolo";
                    break;
                default:
                    mensajeError = "Error inesperado";
                    break;
            }
            return new Error() { Codigo = estado, MensajeError = mensajeError, Tipo = tipoError.Lexico, Linea = linea };


        }


        public List<Token> EjecutarLexico(string codigoFuenteInterface)
        {
            codigoFuente = codigoFuenteInterface + " ";
            listaToken = new List<Token>();  // inicializar
            listaError = new List<Error>();  // inicializar
            int estado = 0; //  la fila de la matriz y el estado actual del AFD
            int columna; // presenta la columna de la matriz

            char caracterActual;
            string lexema = string.Empty;

            linea = 1;

            for (int puntero = 0; puntero < codigoFuente.ToCharArray().Length; puntero++)
            {
                caracterActual = SiguienteCaracter(puntero);

                if (caracterActual.Equals('\n'))
                {
                    linea++;
                }

                lexema += caracterActual;

                columna = RegresarColumna(caracterActual);
                estado = matrizTransicion[estado, columna];

                if (estado < 0 && estado > -499) //detectar estados finales
                {
                    if (lexema.Length > 1 && estado != -4 && estado != -5 && estado != -17 && estado != -10 && estado != -18 && estado != -11 && estado != -12 && estado != -13 && estado != -20 && estado != -19 && estado != -23 && estado != -24 && estado != -15 && estado != -16)
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }


                    Token nuevoToken = new Token() { ValorToken = estado, Lexema = lexema, Linea = linea };

                    if (estado == -1)
                        nuevoToken.ValorToken = esPalabraReservada(nuevoToken.Lexema);

                    nuevoToken.TipoToken = esTipo(nuevoToken.ValorToken);

                    listaToken.Add(nuevoToken); //agrego el tokena a la lista

                    /*inicializo valores*/
                    estado = 0;
                    columna = 0;
                    lexema = string.Empty;
                }
                else if (estado <= -499)
                {
                    listaError.Add(ManejoErrores(estado));

                    estado = 0;
                    columna = 0;
                    lexema = string.Empty;
                }

                else if (estado == 0)
                {
                    columna = 0;
                    lexema = string.Empty;
                }
            }

            return listaToken; // el resultado final del lexico
        } //metodo principal de la clase lexico

        public void Sintaxis()
        {
            listaVariables = new List<Variable>();
            listaPolish = new List<Polish>();
            int cont = 0;
            int vuelta = 0;

            while (cont != listaToken.Count)
            {
                if (listaToken[cont].ValorToken == -29)//{
                {
                    cont++;
                    while (cont + 1 != listaToken.Count)
                    {
                        cont = sintaxis.bloque(listaToken, cont, vuelta);
                        vuelta++;
                    }

                    if (listaToken[cont].ValorToken == -30)//}
                    {
                        while (sintaxis.listaError.Count>0)
                        {
                            Error errorAux = sintaxis.listaError.ElementAt(0);
                            listaError.Add(errorAux);
                            sintaxis.listaError.RemoveAt(0);
                        }

                        listaVariables = sintaxis.listaVariables;
                        listaPolish = sintaxis.listaPolish;
                        break;
                    }
                    else
                    {
                        listaError.Add(sintaxis.ManejoErroresSintactico(-506, listaToken[cont].Linea));
                        break;
                    }


                }
                else
                {
                    listaError.Add(sintaxis.ManejoErroresSintactico(-506, listaToken[cont].Linea));
                    break;
                }
            }
        }

        public void CrearEnsamblador()
        {
            Ensamblador ensamblador = new Ensamblador();

            ensamblador.crearDocumentoEnsamblador(listaVariables, listaPolish, sintaxis.totalT);
        }

    }
}
