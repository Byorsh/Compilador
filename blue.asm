;/StartHeader
INCLUDE macros.asm
.MODEL SMALL
.STACK 100h
.DATA
		x DW '', '$' 
		y DW '', '$' 
		a DW , '', '$' 
		t1 DW '' ? 
		t2 DW '' ? 
		t3 DW '' ? 
		t4 DW '' ? 
		t5 DW '' ? 
		t6 DW '' ? 
.CODE
BEGIN:
		MOV AX, @DATA
		MOV DS, AX
CALL COMPI
		MOV AX, 4C00H
		INT 21H
COMPI PROC

			MULTI 3, 2, t1

			SUMAR 1, t1, t2

			I_ASIGNAR x, t2

			I_ASIGNAR y, 2

			I_ASIGNAR a, "No"

	X1:
			I_IGUAL x, 1, t3


		JF t3, W1

	X2:
			I_IGUAL y, 2, t4


		JF t4, W2
			I_IGUAL x, 1, t5


		JF t5, A1

			I_ASIGNAR x, 2

			I_ASIGNAR y, 5

	A1:
			I_IGUAL x, 1, t6


		JF t6, A2

			I_ASIGNAR x, 2

			I_ASIGNAR y, 5

			I_ASIGNAR a, "Si"

	A2:


		JMP X2


		JMP X1

	W2:

	W1:

	ret
COMPI ENDP
END BEGIN
