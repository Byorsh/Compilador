using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompiladorFinal
{
    internal class Ensamblador
    {
        public string cadena_ensamblador = "";
        Stack<string> listaAuxoperandos = new Stack<string>();
        string op1, op2;
        int tAct = 1;
        bool opActiva = false;
        

        public void crearDocumentoEnsamblador(List<Variable> listaVariables, List<Polish> listaPolish, int totalT)
        {
            try
            {
                string filename = @"C:\Users\Jorge Barraza\Documents\compi\CompiladorFinal\blue.asm";
                using (StreamWriter mylogs = File.CreateText(filename))
                {
                    cadena_ensamblador = ";/StartHeader\nINCLUDE macros.asm\n.MODEL SMALL\n.STACK 100h\n.DATA\n";
                    for (int i = 0; i < listaVariables.Count; i++)
                    {
                        if (listaVariables[i].Usada == true)
                        {
                            switch (listaVariables[i].TipoVariable)
                            {
                                case -55:
                                    cadena_ensamblador += "\t\t" + listaVariables[i].Lexema + " DW '', '$' \n";
                                    break;
                                case -58:
                                    cadena_ensamblador += "\t\t" + listaVariables[i].Lexema + " DW , '', '$' \n";
                                    break;
                                case -56:
                                    cadena_ensamblador += "\t\t" + listaVariables[i].Lexema + " DB '0' \n";
                                    break;
                                case -57:
                                    cadena_ensamblador += "\t\t" + listaVariables[i].Lexema + " DD \n";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    for (int i = 1; i < totalT; i++)
                    {
                        cadena_ensamblador += "\t\tt" + i + " DW '' ? \n";
                    }
                    cadena_ensamblador += ".CODE\nBEGIN:\n\t\tMOV AX, @DATA\n\t\tMOV DS, AX\nCALL COMPI\n\t\tMOV AX, 4C00H\n\t\tINT 21H\nCOMPI PROC\n";

                    for (int i = 0; i < listaPolish.Count; i++)
                    {
                        switch (listaPolish[i].Lexema)
                        {
                            case "+":
                                if (opActiva == false)
                                {
                                    op1 = listaAuxoperandos.Pop();
                                    op2 = listaAuxoperandos.Pop();
                                    cadena_ensamblador += "\n\t\t\tSUMAR " + op2 + ", " + op1 + ", t" + tAct + "\n";
                                    tAct++;
                                    opActiva = true;
                                }
                                else
                                {
                                    op1 = listaAuxoperandos.Pop();
                                    cadena_ensamblador += "\n\t\t\tSUMAR " + op1 + ", t" + (tAct - 1) + ", t" + tAct + "\n";
                                    tAct++;
                                }
                                break;
                            case "-":
                                if (opActiva == false)
                                {
                                    op1 = listaAuxoperandos.Pop();
                                    op2 = listaAuxoperandos.Pop();
                                    cadena_ensamblador += "\n\t\t\tRESTA " + op2 + ", " + op1 + ", t" + tAct + "\n";
                                    tAct++;
                                    opActiva = true;
                                }
                                else
                                {
                                    op1 = listaAuxoperandos.Pop();
                                    cadena_ensamblador += "\n\t\t\tRESTA " + op1 + ", t" + (tAct - 1) + ", t" + tAct + "\n";
                                    tAct++;
                                }
                                break;
                            case "*":
                                if (opActiva == false)
                                {
                                    op1 = listaAuxoperandos.Pop();
                                    op2 = listaAuxoperandos.Pop();
                                    cadena_ensamblador += "\n\t\t\tMULTI " + op2 + ", " + op1 + ", t" + tAct + "\n";
                                    tAct++;
                                    opActiva = true;
                                }
                                else
                                {
                                    op1 = listaAuxoperandos.Pop();
                                    cadena_ensamblador += "\n\t\t\tMULTI " + op1 + ", t" + (tAct - 1) + ", t" + tAct + "\n";
                                    tAct++;
                                }
                                break;
                            case "/":
                                if (opActiva == false)
                                {
                                    op1 = listaAuxoperandos.Pop();
                                    op2 = listaAuxoperandos.Pop();
                                    cadena_ensamblador += "\n\t\t\tDIVIDE " + op2 + ", " + op1 + ", t" + tAct + "\n";
                                    tAct++;
                                    opActiva = true;
                                }
                                else
                                {
                                    op1 = listaAuxoperandos.Pop();
                                    cadena_ensamblador += "\n\t\t\tDIVIDE " + op1 + ", t" + (tAct - 1) + ", t" + tAct + "\n";
                                    tAct++;
                                }
                                break;
                            case "=":
                                if (opActiva == false)
                                {
                                    op1 = listaAuxoperandos.Pop();
                                    op2 = listaAuxoperandos.Pop();
                                    cadena_ensamblador += "\n\t\t\tI_ASIGNAR " + op2 + ", " + op1 + "\n";
                                }
                                else
                                {
                                    op1 = listaAuxoperandos.Pop();
                                    cadena_ensamblador += "\n\t\t\tI_ASIGNAR " + op1 + ", t" + (tAct - 1) + "\n";
                                }
                                
                                opActiva = false;
                                break;
                            case "read":
                                op1 = listaAuxoperandos.Pop();
                                for (int j = 0; j < listaVariables.Count; j++)
                                {
                                    if (op1 == listaVariables[j].Lexema)
                                    {
                                        switch (listaVariables[j].TipoVariable)
                                        {
                                            case -55:
                                            case -57:
                                                cadena_ensamblador += "\t\t\treadinteger@ " + op1 + "\n";
                                                break;
                                            case -58:
                                            case -56:
                                                cadena_ensamblador += "\t\t\treadstring@ " + op1 + "\n";
                                                break;
                                        }
                                    }
                                }
                                break;
                            case "write":
                                op1 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tWRITE " + op1 + "\n";
                                break;
                            case ">":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_MAYOR " + op2 + ", " + op1 + ", t" + tAct + "\n";
                                tAct++;
                                break;
                            case "<":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_MENOR " + op2 + ", " + op1 + ", t" + tAct + "\n";
                                tAct++;
                                break;
                            case "<=":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_MENORIGUAL " + op2 + ", " + op1 + ", t" + tAct + "\n";
                                tAct++;
                                break;
                            case ">=":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_MAYORIGUAL " + op2 + ", " + op1 + ", t" + tAct + "\n";
                                tAct++;
                                break;
                            case "==":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_IGUAL " + op2 + ", " + op1 + ", t" + tAct + "\n";
                                tAct++;
                                break;
                            case "!=":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_DIFERENTES " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                tAct++;
                                break;
                            case "BRF":
                                cadena_ensamblador += "\n\n\t\tJF t" + (tAct - 1) + ", " + listaPolish[i].Direccionamiento + "\n";
                                
                                break;
                            case "BRI":
                                cadena_ensamblador += "\n\n\t\tJMP " + listaPolish[i].Direccionamiento + "\n";
                                
                                break;
                            default:
                                if (listaPolish[i].Salto != null)
                                {
                                    cadena_ensamblador += "\n\t" + listaPolish[i].Salto + ":\n";
                                }
                                listaAuxoperandos.Push(listaPolish[i].Lexema);
                                break;
                        }
                    }
                    cadena_ensamblador += "\n\tret\nCOMPI ENDP\nEND BEGIN";
                    mylogs.WriteLine(cadena_ensamblador);
                    mylogs.Close();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
