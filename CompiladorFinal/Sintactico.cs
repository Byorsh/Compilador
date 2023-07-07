using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompiladorFinal
{
    class Sintactico
    {
        public List<Variable> listaVariables = new List<Variable>();
        public List<Error> listaError = new List<Error>();
        public List<string> listaPostfix = new List<string>();
        public List<string> listaAuxPostfix = new List<string>();
        public List<int> listaPostfixToken = new List<int>();
        public List<int> listaAuxPostfixToken = new List<int>();
        public List<Polish> listaPolish = new List<Polish>();
        public int totalT = 0;
        int tokenTipo, vueltaAux, iteradorIF = 0, iteradorWhile = 0;
        int ifList = 0, ifListSolved = 0;
        string lexema, stringR, stringW, direccion, saltoActual;
        bool condicionIf = false, condicionWhile = false, condicionElse = false, saltoPendiente = false;
        Polish nuevoPolish;

        //controlador principal
        public int bloque(List<Token> listaToken, int posicion, int vuelta)
        {
            //reinicio
            if (vuelta == 0)
            {
                listaVariables.Clear();
                listaError.Clear();
                listaAuxPostfix.Clear();
                listaPostfix.Clear();
                listaPolish.Clear();
                iteradorIF = 0;
                iteradorWhile = 0;
                condicionIf = false;
                condicionWhile = false;
                condicionElse = false;
                totalT = 0;
                ifList = 0;
                ifListSolved = 0;

            }
            //int, double, string, char
            if (listaToken[posicion].ValorToken == -55 || listaToken[posicion].ValorToken == -56 || listaToken[posicion].ValorToken == -57 || listaToken[posicion].ValorToken == -58)
            {
                tokenTipo = listaToken[posicion].ValorToken;
                posicion = crearVariable(listaToken, posicion);
            }
            //id
            else if (listaToken[posicion].ValorToken == -1)
            {
                int posicionActual = posicion;
                posicion = verificarOperador(listaToken, posicion);
                

                if (listaError.Count == 0)
                {
                    agregarPostfijo(listaToken, posicionActual);
                    MandaraPolish(listaToken, posicionActual);
                }
            }
            //if - si
            else if (listaToken[posicion].ValorToken == -60)
            {
                posicion = condicionaIf(listaToken, posicion);
            }
            //while - mientras
            else if (listaToken[posicion].ValorToken == -63)
            {
                posicion = condicionalWhile(listaToken, posicion);
            }
            //read
            else if (listaToken[posicion].ValorToken == -81)
            {
                int posicionActual = posicion;
                posicion = lineaR(listaToken, posicion);
                if (listaError.Count == 0)
                {
                    MandaraPolish(listaToken, posicionActual);
                }
            }
            //write
            else if (listaToken[posicion].ValorToken == -82)
            {
                int posicionActual = posicion;
                posicion = lineaW(listaToken, posicion);
                if (listaError.Count == 0)
                {
                    MandaraPolish(listaToken, posicionActual);
                }
            }
            vueltaAux = vuelta + 1;
            return posicion;
        }

        public int crearVariable(List<Token> listaToken, int posicion)
        {
            //;
            while (listaToken[posicion].ValorToken != -31)
            {
                posicion++;
                //id - ,(coma)
                if (listaToken[posicion].ValorToken == -1 || listaToken[posicion].ValorToken == -41)
                {
                    //id
                    if (listaToken[posicion].ValorToken == -1)
                    {
                        int iAux = 0;
                        lexema = listaToken[posicion].Lexema;
                        if (listaVariables.Count == 0)
                        {
                            agregarVariable(lexema);
                        }
                        else
                        {
                            for (int i = 0; i < listaVariables.Count; i++)
                            {
                                if (listaVariables[i].Lexema == listaToken[posicion].Lexema)
                                {
                                    listaError.Add(ManejoErroresSemantico(-510, listaToken[posicion].Linea));
                                    break;
                                }
                                iAux = i;

                            }
                            if (listaVariables[iAux].Lexema != listaToken[posicion].Lexema)
                            {
                                agregarVariable(lexema);
                            }
                        }
                    }

                }
                //int, double, string, char,{, si, para, hacer, mientras, sino, switch, read, write, id, }
                else if (listaToken[posicion].ValorToken == -55 || listaToken[posicion].ValorToken == -56 || listaToken[posicion].ValorToken == -57 
                    || listaToken[posicion].ValorToken == -58 || listaToken[posicion].ValorToken == -29 || listaToken[posicion].ValorToken == -60 
                    || listaToken[posicion].ValorToken == -61 || listaToken[posicion].ValorToken == -62 || listaToken[posicion].ValorToken == -63 
                    || listaToken[posicion].ValorToken == -66 || listaToken[posicion].ValorToken == -77 || listaToken[posicion].ValorToken == -78 
                    || listaToken[posicion].ValorToken == -79 || listaToken[posicion].ValorToken == -1 || listaToken[posicion].ValorToken == -30)
                {
                    listaError.Add(ManejoErroresSintactico(-507, listaToken[posicion].Linea));
                    posicion--;
                    break;
                   
                }
            }
            return posicion + 1;
        }

        internal int condicionaIf(List<Token> listaToken, int posicion)
        {
            iteradorIF++;
            ifList++;
            posicion++;
            //(
            if (listaToken[posicion].ValorToken == -27)
            {
                posicion++;
                condicionIf = true;
                posicion = verificarCondicional(listaToken, posicion);
                //{
                if (listaToken[posicion].ValorToken == -29)
                {
                    nuevoPolish = new Polish() { Lexema = "BRF", Direccionamiento = direccion, Salto = null };
                    listaPolish.Add(nuevoPolish);
                    posicion++;
                    //}
                    while (listaToken[posicion].ValorToken != -30)
                    {              
                        int posicionActual = bloque(listaToken, posicion, vueltaAux);
                        posicion = posicionActual;
                    }
                    saltoPendiente = true;
                    saltoActual = "A" + iteradorIF;
                    posicion++;
                    //sino - else
                    if (listaToken[posicion].ValorToken == -66)
                    {
                        
                        direccion = "E" + iteradorIF;
                        nuevoPolish = new Polish() { Lexema = "BRI", Direccionamiento = direccion, Salto = null };
                        listaPolish.Add(nuevoPolish);
                        condicionElse = true;
                        posicion++;
                        //{
                        if (listaToken[posicion].ValorToken == -29)
                        {
                            saltoPendiente = true;
                            
                            posicion++;
                            //}
                            while (listaToken[posicion].ValorToken != -30)
                            {
                                int posicionActual = bloque(listaToken, posicion, vueltaAux);
                                posicion = posicionActual;
                            }
                            iteradorIF++;
                            saltoActual = "E" + iteradorIF;
                            saltoPendiente = true;
                            
                        }
                        else
                        {
                            listaError.Add(ManejoErroresSintactico(-506, listaToken[posicion].Linea));
                        }
                        posicion++;
                    }
                    

                }
            }
            int posAux = posicion;
            
            //}
            while ((listaToken[posicion].ValorToken == -30 || listaToken[posicion].ValorToken == -60) && saltoPendiente == true)
            {
                if (posAux + 1 == listaToken.Count || listaToken[posicion].ValorToken == -60 || listaToken[posicion].ValorToken == -30 || listaToken[posicion].ValorToken == -63)
                {
                    if (condicionElse == true)
                    {
                        nuevoPolish = new Polish() { Lexema = "Fin Else", Direccionamiento = null, Salto = saltoActual };
                        listaPolish.Add(nuevoPolish);
                        condicionElse = false;
                    }
                    else if (condicionIf == false || iteradorIF >= 1)
                    {
                        if (ifList == ifListSolved)
                        {
                            saltoPendiente = false;
                        }
                        else
                        {
                            nuevoPolish = new Polish() { Lexema = "Fin If", Direccionamiento = null, Salto = "A" + iteradorIF };
                            listaPolish.Add(nuevoPolish);
                            ifListSolved++;
                            if (iteradorIF == 1)
                            {
                                saltoPendiente = false;
                            }
                        }
                    }

                    
                    if (iteradorIF > 1 || condicionIf == false)
                    {
                        iteradorIF--;
                        
                    }
                }
                else
                {
                    saltoPendiente = false;
                }
                
            }
            return posicion;
        }

        internal int condicionalWhile(List<Token> listaToken, int posicion)
        {
            iteradorWhile++;
            int itWhileAux = iteradorWhile;
            posicion++;
            //(
            if (listaToken[posicion].ValorToken == -27)
            {
                condicionWhile = true;
                posicion++;
                posicion = verificarCondicional(listaToken, posicion);
                //{
                if (listaToken[posicion].ValorToken == -29)
                {
                    saltoActual = "W" + iteradorWhile;
                    nuevoPolish = new Polish() { Lexema = "BRF", Direccionamiento = saltoActual, Salto = null };
                    listaPolish.Add(nuevoPolish);
                    posicion++;
                    //}
                    while (listaToken[posicion].ValorToken != -30)
                    {
                        int posicionActual = bloque(listaToken, posicion, vueltaAux);
                        posicion = posicionActual;
                    }
                    
                    nuevoPolish = new Polish() { Lexema = "BRI", Direccionamiento = "X" + itWhileAux, Salto = null };
                    listaPolish.Add(nuevoPolish);
                    if (itWhileAux > 1)
                    {
                        itWhileAux--;
                    }
                    saltoActual = "W" + iteradorWhile;
                    saltoPendiente = true;
                }
                else
                {
                    listaError.Add(ManejoErroresSintactico(-506, listaToken[posicion].Linea));
                }
            }

            int posAux = posicion;
            //}
            while (listaToken[posicion].ValorToken == -30 && posAux + 2 == listaToken.Count && saltoPendiente == true)
            {
                if (condicionWhile == true)
                {
                    nuevoPolish = new Polish() { Lexema = "Fin While", Direccionamiento = null, Salto = "W" + iteradorWhile };
                }
                listaPolish.Add(nuevoPolish);
                if (iteradorWhile > 1)
                {
                    iteradorWhile--;
                }
                else if(iteradorWhile == 1)
                {
                    saltoPendiente = false;
                    condicionWhile = false;
                }
            }
            return posicion + 1;
        }

        //Este es una pela bastante confuso de polish xd
        internal int verificarCondicional(List<Token> listaToken, int posicion)
        {
            int operador;
            List<int> listaTokens = new List<int>();
            List<string> listaLexemas = new List<string>();
 
            if (condicionIf == true)
            {
                direccion = "A" + iteradorIF;
                ifListSolved++;
            }
            else if (condicionWhile == true)
            {
                direccion = "X" + iteradorWhile;
            }
            //id, entero, decimal, cadena, caracter
            if (listaToken[posicion].ValorToken == -1 || listaToken[posicion].ValorToken == -2 || listaToken[posicion].ValorToken == -3 || listaToken[posicion].ValorToken == -4 || listaToken[posicion].ValorToken == -5)
            {
                //id
                if (listaToken[posicion].ValorToken == -1)
                {
                    verificarVariablesDefinidas(listaToken, posicion);
                }
                if (condicionWhile == true) { nuevoPolish = new Polish() { Lexema = listaToken[posicion].Lexema, Direccionamiento = null, Salto = direccion }; }
                if (condicionIf == true) { nuevoPolish = new Polish() { Lexema = listaToken[posicion].Lexema, Direccionamiento = null, Salto = null }; }
                if (condicionIf == true && condicionElse == true && saltoPendiente == true) { nuevoPolish = new Polish() { Lexema = listaToken[posicion].Lexema, Direccionamiento = null, Salto = saltoActual }; }
                listaPolish.Add(nuevoPolish);

                listaTokens.Add(listaToken[posicion].ValorToken);

                listaLexemas.Add(listaToken[posicion].Lexema);

                posicion++;
                //)
                while (listaToken[posicion].ValorToken != -28)
                {
                    //==, !=, <, >, <=, >=
                    if (listaToken[posicion].ValorToken == -19 || listaToken[posicion].ValorToken == -20 || listaToken[posicion].ValorToken == -21 ||
                        listaToken[posicion].ValorToken == -22 || listaToken[posicion].ValorToken == -23 || listaToken[posicion].ValorToken == -24)
                    {
                        operador = listaToken[posicion].ValorToken;
                        string operadorAux = listaToken[posicion].Lexema;

                        

                        posicion++;
                        //id, entero, decimal, cadena, caracter
                        if (listaToken[posicion].ValorToken == -1 || listaToken[posicion].ValorToken == -2 || listaToken[posicion].ValorToken == -3 || listaToken[posicion].ValorToken == -4 || listaToken[posicion].ValorToken == -5)
                        {
                            if (listaToken[posicion].ValorToken == -1)
                            {
                                verificarVariablesDefinidas(listaToken, posicion);
                            }
                            nuevoPolish = new Polish() { Lexema = listaToken[posicion].Lexema, Direccionamiento = null, Salto = null };
                            listaPolish.Add(nuevoPolish);
                            nuevoPolish = new Polish() { Lexema = operadorAux, Direccionamiento = null, Salto = null };
                            listaPolish.Add(nuevoPolish);

                            listaTokens.Add(listaToken[posicion].ValorToken);

                            listaLexemas.Add(listaToken[posicion].Lexema);

                            posicion++;
                        }

                        listaTokens.Add(operador);

                        listaLexemas.Add(operadorAux);
                    }

                    //{
                    if (listaToken[posicion].ValorToken == -29)
                    {
                        listaError.Add(ManejoErroresSintactico(-508, listaToken[posicion].Linea));
                    }
                }

                int auxP = posicion;
                verificarTipos(listaTokens, listaLexemas, listaToken[auxP - 1].Linea); 
            }
            totalT++;
            return posicion + 1;
        }

        internal int verificarOperador(List<Token> listaToken, int posicion)
        {
            verificarVariablesDefinidas(listaToken, posicion);

            posicion++;
            //=
            if (listaToken[posicion].ValorToken == -37)
            {

                posicion++;
                //id, entero, decimal, cadena, caracter
                if (listaToken[posicion].ValorToken == -1 || listaToken[posicion].ValorToken == -2 || listaToken[posicion].ValorToken == -3 || listaToken[posicion].ValorToken == -4 || listaToken[posicion].ValorToken == -5)
                {
                    //id
                    if (listaToken[posicion].ValorToken == -1)
                    {
                        verificarVariablesDefinidas(listaToken, posicion);
                    }

                    posicion++;
                    //;
                    while (listaToken[posicion].ValorToken != -31)
                    {
                        //+, -, *, /
                        if (listaToken[posicion].ValorToken == -6 || listaToken[posicion].ValorToken == -7 || listaToken[posicion].ValorToken == -8 || listaToken[posicion].ValorToken == -9)
                        {

                            posicion++;
                            // identificador, num entero, num decimal, string, char
                            if (listaToken[posicion].ValorToken == -1 || listaToken[posicion].ValorToken == -2 || listaToken[posicion].ValorToken == -3 || listaToken[posicion].ValorToken == -4 || listaToken[posicion].ValorToken == -5)
                            {
                                //id
                                if (listaToken[posicion].ValorToken == -1)
                                {
                                    verificarVariablesDefinidas(listaToken, posicion);
                                }
                                //num entero, num decimal, string, char
                                else if (listaToken[posicion].ValorToken == -2 || listaToken[posicion].ValorToken == -3 || listaToken[posicion].ValorToken == -4 || listaToken[posicion].ValorToken == -5)
                                {

                                }
                                posicion++;
                            }
                            else
                            {
                                listaError.Add(ManejoErroresSintactico(-509, listaToken[posicion].Linea));
                                break;
                            }
                        }
                        
                        //int, char, double, string, {, }, si, para, hacer, mientras, sino, switch, read, write
                        else if (listaToken[posicion].ValorToken == -55 || listaToken[posicion].ValorToken == -56 || listaToken[posicion].ValorToken == -57 
                            || listaToken[posicion].ValorToken == -58 || listaToken[posicion].ValorToken == -29 || listaToken[posicion].ValorToken == -60 
                            || listaToken[posicion].ValorToken == -61 || listaToken[posicion].ValorToken == -62 || listaToken[posicion].ValorToken == -63 
                            || listaToken[posicion].ValorToken == -66 || listaToken[posicion].ValorToken == -77 || listaToken[posicion].ValorToken == -78 || listaToken[posicion].ValorToken == -79 || listaToken[posicion].ValorToken == -30)
                        {
                            listaError.Add(ManejoErroresSintactico(-507, listaToken[posicion].Linea));
                            posicion--;
                            break;

                        }
                    }

                }
            }
            return posicion + 1;
        }

        internal int lineaR(List<Token> listaToken, int posicion)
        {
            posicion++;
            //(
            if (listaToken[posicion].ValorToken == -27)
            {
                posicion++;
                //id, entero, decimal, cadena, caracter
                if (listaToken[posicion].ValorToken == -1 || listaToken[posicion].ValorToken == -2 || listaToken[posicion].ValorToken == -3 || listaToken[posicion].ValorToken == -4 || listaToken[posicion].ValorToken == -5)
                {
                    stringR = listaToken[posicion].Lexema;
                    posicion++;
                    //)
                    if (listaToken[posicion].ValorToken == -28)
                    {
                        posicion++;
                        //;
                        if (listaToken[posicion].ValorToken == -31)
                        {
                        }
                        else
                        {
                            listaError.Add(ManejoErroresSintactico(-507, listaToken[posicion].Linea));
                            posicion--;
                        }
                    }
                }
            }
            return posicion + 1;
        }

        internal int lineaW(List<Token> listaToken, int posicion)
        {
            posicion++;
            //(
            if (listaToken[posicion].ValorToken == -27)
            {
                posicion++;
                //id, entero, decimal, cadena, caracter
                if (listaToken[posicion].ValorToken == -1 || listaToken[posicion].ValorToken == -2 || listaToken[posicion].ValorToken == -3 || listaToken[posicion].ValorToken == -4 || listaToken[posicion].ValorToken == -5)
                {
                    stringW = listaToken[posicion].Lexema;
                    posicion++;
                    //)
                    if (listaToken[posicion].ValorToken == -28)
                    {
                        posicion++;
                        //;
                        if (listaToken[posicion].ValorToken == -31)
                        {
                        }
                        else
                        {
                            listaError.Add(ManejoErroresSintactico(-507, listaToken[posicion].Linea));
                            posicion--;
                        }

                    }
                }
            }
            return posicion + 1;
        }

        internal void agregarVariable(string lexico)
        {
            Variable nuevaVariable = new Variable() { TipoVariable = tokenTipo, Lexema = lexema };
            listaVariables.Add(nuevaVariable);
        }

        internal void verificarVariablesDefinidas(List<Token> listaToken, int posicion)
        {
            lexema = listaToken[posicion].Lexema;
            if (listaVariables.Count > 0)
            {
                for (int i = 0; i < listaVariables.Count; i++)
                {
                    if (lexema == listaVariables[i].Lexema)
                    {
                        
                        break;
                    }
                    if (i + 1 == listaVariables.Count)
                    {
                        listaError.Add(ManejoErroresSemantico(-511, listaToken[posicion].Linea));
                        break;
                    }
                }
            }
            else
            {
                listaError.Add(ManejoErroresSemantico(-511, listaToken[posicion].Linea));
            }
        }

        //Errores
        public Error ManejoErroresSintactico(int estado, int linea)
        {
            string mensajeError;

            switch (estado)
            {
                case -506:
                    mensajeError = "Falta una llave";
                    break;
                case -507:
                    mensajeError = "Se esperaba un ';'";
                    break;
                case -508:
                    mensajeError = "Falta cerrar un parentesis";
                    break;
                case -509:
                    mensajeError = "Palabra / Simbolo no permitido";
                    break;

                default:
                    mensajeError = "Error inesperado";
                    break;
            }
            return new Error() { Codigo = estado, MensajeError = mensajeError, Tipo = tipoError.Sintactico, Linea = linea };


        }

        public Error ManejoErroresSemantico(int estado, int linea)
        {
            string mensajeError;

            switch (estado)
            {
                case -510:
                    mensajeError = "Variable duplicada";
                    break;
                case -511:
                    mensajeError = "Variable usada pero no definida";
                    break;
                case -512:
                    mensajeError = "Incopatibilidad de tipos";
                    break;

                default:
                    mensajeError = "Error inesperado";
                    break;
            }
            return new Error() { Codigo = estado, MensajeError = mensajeError, Tipo = tipoError.Semantico, Linea = linea };


        }

        //Seccion postfijo
        public void agregarPostfijo(List<Token> listatoken, int posicion)
        {
            listaPostfix.Clear();
            listaPostfixToken.Clear();
            listaAuxPostfixToken.Clear();
            listaAuxPostfix.Clear();
            while (listatoken[posicion].ValorToken != -31)
            {
                string lexema = listatoken[posicion].Lexema;
                int valortoken = listatoken[posicion].ValorToken;
                if (valortoken == -1 || valortoken == -2 || valortoken == -3 || valortoken == -4 || valortoken == -5)//operando
                {

                    listaPostfix.Add(lexema);
                    listaPostfixToken.Add(valortoken);
                    
                }

                else//operador
                {
                    if (listaAuxPostfix.Count == 0) { listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken); }
                    else
                    {
                        string ultimoperdadorlistaAux = listaAuxPostfix.ElementAt(listaAuxPostfix.Count - 1);
                        int ultimotokenlistaAux = listaAuxPostfixToken.ElementAt(listaAuxPostfixToken.Count - 1);
                        switch (valortoken)
                        {
                            case -8://   *
                            case -9://   /
                                if (consultarPrioridad(ultimoperdadorlistaAux) <= consultarPrioridad(lexema) && ultimoperdadorlistaAux != "=")
                                {
                                    listaPostfix.Add(ultimoperdadorlistaAux); listaPostfixToken.Add(ultimotokenlistaAux);

                                    listaAuxPostfix.Remove(ultimoperdadorlistaAux); listaAuxPostfixToken.Remove(ultimotokenlistaAux);

                                    listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken);

                                }
                                else
                                {
                                    listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken);
                                }

                                break;
                            case -6://   +
                            case -7://   -
                                if (consultarPrioridad(ultimoperdadorlistaAux) <= consultarPrioridad(lexema) && ultimoperdadorlistaAux != "=")
                                {
                                    listaPostfix.Add(ultimoperdadorlistaAux); listaPostfixToken.Add(ultimotokenlistaAux);

                                    listaAuxPostfix.Remove(ultimoperdadorlistaAux); listaAuxPostfixToken.Remove(ultimotokenlistaAux);

                                    ultimoperdadorlistaAux = listaAuxPostfix.ElementAt(listaAuxPostfix.Count - 1);
                                    ultimotokenlistaAux = listaAuxPostfixToken.ElementAt(listaAuxPostfixToken.Count - 1);

                                    listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken);
                                    if (consultarPrioridad(ultimoperdadorlistaAux) <= consultarPrioridad(lexema) && ultimoperdadorlistaAux != "=")
                                    {
                                        listaPostfix.Add(ultimoperdadorlistaAux); listaPostfixToken.Add(ultimotokenlistaAux);

                                        listaAuxPostfix.Remove(ultimoperdadorlistaAux); listaAuxPostfixToken.Remove(ultimotokenlistaAux);
                                    }
                                }
                                else
                                {
                                    listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken);
                                }
                                break;
                            case -22://   >
                            case -24://   >=
                            case -21://   <
                            case -23://   <=
                            case -19://   ==
                            case -20://   !=
                                if (consultarPrioridad(ultimoperdadorlistaAux) <= consultarPrioridad(lexema) && ultimoperdadorlistaAux != "=")
                                {
                                    listaPostfix.Add(ultimoperdadorlistaAux); listaPostfixToken.Add(ultimotokenlistaAux);

                                    listaAuxPostfix.Remove(ultimoperdadorlistaAux); listaAuxPostfixToken.Remove(ultimotokenlistaAux);

                                    listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken);

                                }
                                else
                                {
                                    listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken);
                                }
                                break;
                            case -15://   and &&
                                if (consultarPrioridad(ultimoperdadorlistaAux) <= consultarPrioridad(lexema) && ultimoperdadorlistaAux != "=")
                                {
                                    listaPostfix.Add(ultimoperdadorlistaAux); listaPostfixToken.Add(ultimotokenlistaAux);

                                    listaAuxPostfix.Remove(ultimoperdadorlistaAux); listaAuxPostfixToken.Remove(ultimotokenlistaAux);

                                    listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken);

                                }
                                else
                                {
                                    listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken);
                                }
                                break;
                            case -16://   or ||
                                if (consultarPrioridad(ultimoperdadorlistaAux) <= consultarPrioridad(lexema) && ultimoperdadorlistaAux != "=")
                                {
                                    listaPostfix.Add(ultimoperdadorlistaAux); listaPostfixToken.Add(ultimotokenlistaAux);

                                    listaAuxPostfix.Remove(ultimoperdadorlistaAux); listaAuxPostfixToken.Remove(ultimotokenlistaAux);

                                    listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken);

                                }
                                else
                                {
                                    listaAuxPostfix.Add(lexema); listaAuxPostfixToken.Add(valortoken);
                                }
                                break;
                            case -37:// =
                                listaAuxPostfix.Add(lexema);
                                break;

                        }
                    }
                    totalT++;

                }
                posicion++;
            }
            while (listaAuxPostfixToken.Count > 0)
            {
                int i = listaAuxPostfixToken.Count - 1;
                listaPostfixToken.Add(listaAuxPostfixToken.ElementAt(i));
                listaPostfix.Add(listaAuxPostfix.ElementAt(i));
                listaAuxPostfixToken.RemoveAt(i);
                listaAuxPostfix.RemoveAt(i);
            }

            verificarTipos(listaPostfixToken, listaPostfix, listatoken[posicion].Linea);
            /*string postfijo = "";
            for (int i = 0; i < listaPostfix.Count; i++)
            {
                
                
            }
            //MessageBox.Show(postfijo);*/

        }

        public int consultarPrioridad(string op1)
        {
            int prioridad = 6;
            if (op1 == "/" || op1 == "*") { prioridad = 0; } //  /,*
            else if (op1 == "+" || op1 == "-") { prioridad = 1; } // +/-
            else if (op1 == ">" || op1 == ">=" || op1 == "<" || op1 == "<=" || op1 == "==" || op1 == "!=") { prioridad = 2; } //>,>=,<,<=,==,!= 
            else if (op1 == "&&") { prioridad = 4; }//and
            else if (op1 == "||") { prioridad = 5; }//or
            return prioridad;
        }

        public int[,] sumaM = { 
         //      int  || char || string || double ||  bool
/*int*/     {    -2   ,  -512  ,   -512  ,   -3    ,   -512,},
/*char*/    {    -512 ,  -512  ,   -4    ,   -512  ,   -512,},
/*string*/  {    -512 ,  -512  ,   -4    ,   -512  ,   -512,},
/*double*/  {    -512 ,  -512  ,   -512  ,   -3    ,   -512,},
/*bool*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,}
        };

        public int[,] restaM = { 
         //      int  || char || string || double ||  bool
/*int*/     {    -2   ,  -512  ,   -512  ,   -3    ,   -512,},
/*char*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
/*string*/  {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
/*double*/  {    -512 ,  -512  ,   -512  ,   -3    ,   -512,},
/*bool*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,}
        };

        public int[,] multiplicacionM = { 
         //      int  || char || string || double ||  bool
/*int*/     {    -2   ,  -512  ,   -512  ,   -3    ,   -512,},
/*char*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
/*string*/  {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
/*double*/  {    -512 ,  -512  ,   -512  ,   -3    ,   -512,},
/*bool*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,}
        };

        public int[,] divisionM = { 
         //      int  || char || string || double ||  bool
/*int*/     {    -2   ,  -512  ,   -512  ,   -3    ,   -512,},
/*char*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
/*string*/  {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
/*double*/  {    -512 ,  -512  ,   -512  ,   -3    ,   -512,},
/*bool*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,}
        };

        public int[,] asignacionM = { 
         //      int  || char || string || double ||  bool
/*int*/     {    -2   ,  -512  ,   -512  ,   -3    ,   -512,},
/*char*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
/*string*/  {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
/*double*/  {    -512 ,  -512  ,   -512  ,   -3    ,   -512,},
/*bool*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,}
        };

        public int[,] diferenteM = { 
         //      int  || char || string || double ||  bool
/*int*/     {    -2   ,  -512  ,   -512  ,   -3    ,   -512,},
/*char*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
/*string*/  {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
/*double*/  {    -512 ,  -512  ,   -512  ,   -3    ,   -512,},
/*bool*/    {    -512 ,  -512  ,   -512  ,   -512  ,   -512,}
        };

        public int[,] mayorQueM = { 
         //      int  || char || string || real ||  bool
         //      int-2  || char-5 || string-4 || real-3 || bool-54
            {    -2   ,  -512  ,   -512  ,   -512  ,   -512,},
            {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
            {    -512 ,  -512  ,   -512  ,   -512  ,   -512,},
            {    -3   ,  -512  ,   -512  ,   -3    ,   -512,},
            {    -512 ,  -512  ,   -512  ,   -512  ,   -512}
        };

        public int[,] menorQueM = { 
         //      int  || char || string || real ||  bool
         //      int-2  || char-5 || string-4 || real-3 || bool-54

            {    -2    ,   -512  ,   -512  ,   -512  ,   -512,},
            {    -512  ,   -512  ,   -512  ,   -512  ,   -512,},
            {    -512  ,   -512  ,   -512  ,   -512  ,   -512,},
            {    -3    ,   -512  ,   -512  ,   -3    ,   -512,},
            {    -512  ,   -512  ,   -512  ,   -512  ,   -512}
        };

        public int[,] mayorIgualM = { 
         //      int  || char || string || real ||  bool
         //      int-2  || char-5 || string-4 || real-3 || bool-54
            {    -2    ,   -512  ,   -512  ,   -512  ,   -512,},
            {    -512  ,   -512  ,   -512  ,   -512  ,   -512,},
            {    -512  ,   -512  ,   -512  ,   -512  ,   -512,},
            {    -3    ,   -512  ,   -512  ,   -3    ,   -512,},
            {    -512  ,   -512  ,   -512  ,   -512  ,   -512}
        };

        public int[,] menorIgualM = { 
         //      int  || char || string || real ||  bool
         //      int-2  || char-5 || string-4 || real-3 || bool-54
            {    -2    ,   -512  ,   -512  ,   -512  ,   -512,},
            {    -512  ,   -512  ,   -512  ,   -512  ,   -512,},
            {    -512  ,   -512  ,   -512  ,   -512  ,   -512,},
            {    -3    ,   -512  ,   -512  ,   -3    ,   -512,},
            {    -512  ,   -512  ,   -512  ,   -512  ,   -512}
        };

        public int[,] igualM = { 
         //      int  || char || string || real ||  bool
         //      int-2  || char-5 || string-4 || real-3 || bool-54
            {    -2    ,   -512  ,   -512  ,   -512  ,   -512,},
            {    -512  ,   -5    ,   -512  ,   -512  ,   -512,},
            {    -512  ,   -4    ,   -4    ,   -512  ,   -512,},
            {    -3    ,   -512  ,   -512  ,   -3    ,   -512,},
            {    -512  ,   -512  ,   -512  ,   -512  ,   -54  }
        };

        public void verificarTipos(List<int> listaPostfijoT, List<string> listaPostfijoL, int linea)
        {
            int tokenMain = 0, tokenAux, operacion;
            List<int> listaTokensAux = new List<int>();
            for (int i = 0; i < listaPostfijoT.Count; i++)
            {
                if (tokenMain <= -500)
                {
                    listaError.Add(ManejoErroresSemantico(-512, linea));
                    break;
                }
                if (listaPostfijoT.ElementAt(i) == -1)
                {
                    for (int j = 0; j < listaVariables.Count; j++)
                    {
                        if (listaPostfijoL.ElementAt(i) == listaVariables[j].Lexema)
                        {
                            listaTokensAux.Add(listaVariables[j].TipoVariable);
                        }
                    }
                }
                else if (listaPostfijoT.ElementAt(i) <= -2 && listaPostfijoT.ElementAt(i) >= -5)
                {
                    listaTokensAux.Add(listaPostfijoT.ElementAt(i));
                }
                else if (listaPostfijoT.ElementAt(i) == -54)
                {
                    listaTokensAux.Add(listaPostfijoT.ElementAt(i));
                }
                else
                {
                    tokenMain = listaTokensAux.ElementAt(listaTokensAux.Count - 1);
                    listaTokensAux.RemoveAt(listaTokensAux.Count - 1);
                    tokenAux = listaTokensAux.ElementAt(listaTokensAux.Count - 1);
                    listaTokensAux.RemoveAt(listaTokensAux.Count - 1);
                    operacion = listaPostfijoT.ElementAt(i);

                    switch(operacion)
                    {
                        case -6:
                            tokenMain = sumaM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        case -7:
                            tokenMain = restaM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        case -8:
                            tokenMain = multiplicacionM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        case -9:
                            tokenMain = divisionM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        case -37:
                            tokenMain = asignacionM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        case -20:
                            tokenMain = diferenteM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        case -21:
                            tokenMain = menorQueM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        case -22:
                            tokenMain = mayorQueM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        case -23:
                            tokenMain = menorIgualM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        case -24:
                            tokenMain = mayorIgualM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        case -19:
                            tokenMain = igualM[obtenerPosicion(tokenMain), obtenerPosicion(tokenAux)];
                            break;
                        default:
                            listaError.Add(ManejoErroresSemantico(-500, linea));
                            break;
                    }

                    if (tokenMain <= -500)
                    {
                        listaError.Add(ManejoErroresSemantico(-512, linea));
                        break;
                    }
                    else{
                        listaTokensAux.Add(tokenMain);
                    }
                }
            }
        }

        public int obtenerPosicion(int op)
        {
            switch (op) {
                case -2:
                    return 0;
                case -55:
                    return 0;
                case -3:
                    return 1;
                case -57:
                    return 1;
                case -4: 
                    return 2;
                case -58:
                    return 2;
                case -5:
                    return 3;
                case -56:
                    return 3;
                case -54:
                    return 4;
                default:
                    return 0;
                    
            }
        }

        internal int buscarVariable(string lexema)
        {
            int tipoVariable = -1;
            if (listaVariables.Count > 0)
            {
                for (int i = 0; i < listaVariables.Count; i++)
                {
                    if (lexema == listaVariables[i].Lexema)
                    {
                        tipoVariable = listaVariables[i].TipoVariable;
                    }
                    else if (i + 1 == listaVariables.Count)
                    {
                        listaError.Add(ManejoErroresSemantico(-511, 0));
                        break;
                    }
                }
            }
            else
            {
                listaError.Add(ManejoErroresSemantico(-511, 0));
            }
            return tipoVariable;
        }

        //-----------------------------------POLISH-------------------------------------------
        public void MandaraPolish(List<Token> listaToken, int posicion)
        {
            nuevoPolish = new Polish();
            string lexemaActual = listaToken[posicion].Lexema;
            string lexemaAux;
            if (listaToken[posicion].ValorToken == -1)
            {
                while (listaPostfix.Count > 0)
                {
                    if (saltoPendiente == true){ 
                        nuevoPolish = new Polish() { Lexema = listaPostfix.ElementAt(0), Direccionamiento = null, Salto = saltoActual };

                        if (saltoActual.StartsWith("W"))
                        {
                            iteradorWhile--;
                            if (iteradorWhile > 1)
                            {
                                condicionWhile = false;
                            }
                        }
                        else if (saltoActual.StartsWith("E"))
                        {
                            iteradorIF--;
                            condicionElse = false;
                        }
                        else if (saltoActual.StartsWith("A"))
                        {
                            iteradorIF--;
                            condicionIf = false;
                        }
                        saltoPendiente = false; }
                    else { nuevoPolish = new Polish() { Lexema = listaPostfix.ElementAt(0), Direccionamiento = null, Salto = null }; }
                    
                    listaPostfix.RemoveAt(0);
                    listaPolish.Add(nuevoPolish);

                }

            }
            if (listaToken[posicion].ValorToken == -81)
            {
                if (saltoPendiente == true)
                {
                    nuevoPolish = new Polish() { Lexema = stringR, Direccionamiento = null, Salto = saltoActual };

                    if (saltoActual.StartsWith("W"))
                    {
                        iteradorWhile--;
                        if (iteradorWhile > 1)
                        {
                            condicionWhile = false;
                        }
                    }
                    else if (saltoActual.StartsWith("E"))
                    {
                        iteradorIF--;
                        condicionElse = false;
                    }
                    else if (saltoActual.StartsWith("A"))
                    {
                        iteradorIF--;
                        condicionIf = false;
                    }
                    saltoPendiente = false;
                }
                else { nuevoPolish = new Polish() { Lexema = stringR, Direccionamiento = null, Salto = null }; }
                
                listaPolish.Add(nuevoPolish);
                lexemaAux = listaToken[posicion].Lexema;
                nuevoPolish = new Polish() { Lexema = lexemaAux, Direccionamiento = null, Salto = null };
                listaPolish.Add(nuevoPolish);

            }
            if (listaToken[posicion].ValorToken == -82)
            {
                if (saltoPendiente == true)
                {
                    nuevoPolish = new Polish() { Lexema = stringW, Direccionamiento = null, Salto = saltoActual };

                    if (saltoActual.StartsWith("W"))
                    {
                        iteradorWhile--;
                        if (iteradorWhile > 1)
                        {
                            condicionWhile = false;
                        }
                    }
                    else if (saltoActual.StartsWith("E"))
                    {
                        iteradorIF--;
                        condicionElse = false;
                    }
                    else if (saltoActual.StartsWith("A"))
                    {
                        iteradorIF--;
                        condicionIf = false;
                    }
                    saltoPendiente = false;
                }
                else { nuevoPolish = new Polish() { Lexema = stringW, Direccionamiento = null, Salto = null }; }
                
                listaPolish.Add(nuevoPolish);
                lexemaAux = listaToken[posicion].Lexema;
                nuevoPolish = new Polish() { Lexema = lexemaAux, Direccionamiento = null, Salto = null };
                listaPolish.Add(nuevoPolish);

            }
        }

    }
}
