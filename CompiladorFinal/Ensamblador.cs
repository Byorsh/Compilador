﻿using System;
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
                string filename = @"C:\Users\Abeled\Desktop\Compilador\Compilador\blue.asm";
                using (StreamWriter mylogs = File.CreateText(filename))
                {
                    cadena_ensamblador = ";/StartHeader\nINCLUDE macros.mac\nDOSSEG\n.MODEL SMALL\nSTACK 100h\n.DATA\n";
                    for (int i = 0; i < listaVariables.Count; i++)
                    {
                        if (listaVariables[i].Usada == true)
                        {
                            cadena_ensamblador += "\t\t" + listaVariables[i].Lexema + " DW '', '$' \n";
                        }
                    }
                    for (int i = 1; i < totalT; i++)
                    {
                        cadena_ensamblador += "\t\tt" + i + " DW '' ? \n";
                    }
                    cadena_ensamblador += ".CODE\n.386\nBEGIN:\n\t\tMOV AX, @DATA\n\t\tMOV DS, AX\nCALL COMPI\n\t\tMOV AX, 4C00H\n\t\tINT 21H\nCOMPI PROC\n";

                    for (int i = 0; i < listaPolish.Count; i++)
                    {
                        switch (listaPolish[i].Lexema)
                        {
                            case "+":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\t\tMOV AL, " + op1 + "\n\t\t\tADD AL, " + op2 + "\n\t\t\tMOV t" + tAct + ", AL";
                                break;
                            case "-":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\t\tMOV AL, " + op1 + "\n\t\t\tSUB AL, " + op2 + "\n\t\t\tMOV t" + tAct + ", AL";
                                break;
                            case "*":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\t\tMOV AL, " + op1 + "\n\t\t\tMOV BL, " + op2 + "\n\t\t\tIMUL BL \n\t\t\tMOV t" + tAct + ", AL";
                                break;
                            case "/":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\t\tMOV DX, 0 \n\t\t\tMOV AL, " + op1 + "\n\t\t\tMOV BL, " + op2 + "\n\t\t\tIDIV BL \n\t\t\tMOV t" + tAct + ", AL";
                                break;
                            case "=":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\t\tMOV AL, " + op2 + "\n\t\t\tMOV " + op1 + ", AL";
                                tAct++;
                                break;
                            case "read":
                                op1 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\t\tMOV DX, OFFSET " + op1 + "\n\t\t\tMOV AH, 3FH" + "\n\t\t\tINT 21H";
                                break;
                            case "write":
                                op1 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\t\tMOV AH, 09H" + "\n\t\t\tLEA DL, " + op1 + "\n\t\t\tINT 21H";
                                break;
                            case ">":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\tLOCAL LABEL1 " + "\n\t\tLOCAL SALIR "
                                    + "\n\t\t\tMOV AL, " + op1 + "\n\t\t\tCMP AL, " + op2 + "\n\t\t\tJLE LABEL1" + "\n\t\t\tMOV t" + tAct + " 1" 
                                    + "\n\t\t\tJMP SALIR" + "\n\t\tLABEL1: " + "\n\t\t\tMOV t" + tAct + ", 0" + "\n\t\tSALIR:";
                                break;
                            case "<":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\tLOCAL LABEL1 " + "\n\t\tLOCAL SALIR "
                                    + "\n\t\t\tMOV AL, " + op1 + "\n\t\t\tCMP AL, " + op2 + "\n\t\t\tJGE LABEL1" + "\n\t\t\tMOV t" + tAct + " 1"
                                    + "\n\t\t\tJMP SALIR" + "\n\t\tLABEL1: " + "\n\t\t\tMOV t" + tAct + ", 0" + "\n\t\tSALIR:";
                                break;
                            case "<=":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\tLOCAL LABEL1 " + "\n\t\tLOCAL SALIR "
                                    + "\n\t\t\tMOV AL, " + op1 + "\n\t\t\tCMP AL, " + op2 + "\n\t\t\tJG LABEL1" + "\n\t\t\tMOV t" + tAct + " 1"
                                    + "\n\t\t\tJMP SALIR" + "\n\t\tLABEL1: " + "\n\t\t\tMOV t" + tAct + ", 0" + "\n\t\tSALIR:";
                                break;
                            case ">=":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\tLOCAL LABEL1 " + "\n\t\tLOCAL SALIR "
                                    + "\n\t\t\tMOV AL, " + op1 + "\n\t\t\tCMP AL, " + op2 + "\n\t\t\tJL LABEL1" + "\n\t\t\tMOV t" + tAct + " 1"
                                    + "\n\t\t\tJMP SALIR" + "\n\t\tLABEL1: " + "\n\t\t\tMOV t" + tAct + ", 0" + "\n\t\tSALIR:";
                                break;
                            case "==":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\tLOCAL LABEL1 " + "\n\t\tLOCAL SALIR "
                                    + "\n\t\t\tMOV AL, " + op1 + "\n\t\t\tCMP AL, " + op2 + "\n\t\t\tJNE LABEL1" + "\n\t\t\tMOV t" + tAct + " 1"
                                    + "\n\t\t\tJMP SALIR" + "\n\t\tLABEL1: " + "\n\t\t\tMOV t" + tAct + ", 0" + "\n\t\tSALIR:";
                                break;
                            case "!=":
                                op1 = listaAuxoperandos.Pop();
                                op2 = listaAuxoperandos.Pop();
                                cadena_ensamblador += "\n\t\tLOCAL LABEL1 " + "\n\t\tLOCAL SALIR "
                                    + "\n\t\t\tMOV AL, " + op1 + "\n\t\t\tCMP AL, " + op2 + "\n\t\t\tJE LABEL1" + "\n\t\t\tMOV t" + tAct + " 1"
                                    + "\n\t\t\tJMP SALIR" + "\n\t\tLABEL1: " + "\n\t\t\tMOV t" + tAct + ", 0" + "\n\t\tSALIR:";
                                break;
                            case "BRF":
                                cadena_ensamblador += "\n\n\t\tJF t" + tAct + ", " + listaPolish[i].Direccionamiento + "\n";
                                tAct++;
                                break;
                            case "BRI":
                                cadena_ensamblador += "\n\n\t\tJMP " + listaPolish[i].Direccionamiento + "\n";
                                tAct++;
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
                    cadena_ensamblador += "\n\tret\nMOV AH, 4cH\nINT 21H\nEND BEGIN";
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
