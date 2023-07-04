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
        

        public void crearDocumentoEnsamblador(List<Variable> listaVariables, List<Polish> listaPolish, int totalT)
        {
            try
            {
                string filename = @"C:\Users\Jorge Barraza\Documents\compi\CompiladorFinal\blue.asm";
                using (StreamWriter mylogs = File.CreateText(filename))
                {
                    cadena_ensamblador = ";/StartHeader\nINCLUDE macros.mac\nDOSSEG\n.MODEL SMALL\nSTACK 100h\n.DATA\n";
                    for (int i = 0; i < listaVariables.Count; i++)
                    {
                        cadena_ensamblador += "\t\t\t" + listaVariables[i].Lexema + " DW '', '$' \n";
                    }
                    for (int i = 1; i < totalT; i++)
                    {
                        cadena_ensamblador += "\t\t\tt" + i + " DW '' ? \n";
                    }
                    cadena_ensamblador += ".CODE\n.386\nBEGIN:\n\t\t\tMOV AX, @DATA\n\t\t\tMOV DS, AX\nCALL COMPI\n\t\t\tMOV AX, 4C00H\n\t\t\tINT 21H\nCOMPI PROC\n";

                    for (int i = 0; i < listaPolish.Count; i++)
                    {
                        switch (listaPolish[i].Lexema)
                        {
                            case "+":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tSUMAR " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                break;
                            case "-":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop(); 
                                cadena_ensamblador += "\t\t\tRESTA " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                break;
                            case "*":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tMULTI " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                break;
                            case "/":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tDIVIDE " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                break;
                            case "=":
                                op1 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_ASIGNAR " + op1 + ", t" + tAct + "\n";
                                tAct++;
                                break;
                            case "read":
                                op1 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tREAD " + op1 + "\n";
                                break;
                            case "write":
                                op1 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tWRITE " + op1 + "\n";
                                break;
                            case ">":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_MAYOR " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                break;
                            case "<":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_MENOR " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                break;
                            case "<=":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_MENORIGUAL " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                break;
                            case ">=":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_MAYORIGUAL " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                break;
                            case "==":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_IGUAL " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                break;
                            case "!=":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\t\t\tI_DIFERENTES " + op1 + ", " + op2 + ", t" + tAct + "\n";
                                break;
                            case "BRF":
                                cadena_ensamblador += "\t\t\tJF t" + tAct + ", " + listaPolish[i].Direccionamiento + "\n";
                                tAct++;
                                break;
                            case "BRI":
                                cadena_ensamblador += "\t\t\tJMP " + listaPolish[i].Direccionamiento + "\n";
                                tAct++;
                                break;
                            default:
                                if (listaPolish[i].Salto != null)
                                {
                                    cadena_ensamblador += "\t\t" + listaPolish[i].Salto + ":\n";
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
