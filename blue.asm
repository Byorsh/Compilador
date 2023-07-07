;/StartHeader
INCLUDE macros.mac
DOSSEG
.MODEL SMALL
STACK 100h
.DATA
			x DW '', '$' 
			y DW '', '$' 
			t1 DW '' ? 
			t2 DW '' ? 
			t3 DW '' ? 
			t4 DW '' ? 
			t5 DW '' ? 
			t6 DW '' ? 
			t7 DW '' ? 
			t8 DW '' ? 
			t9 DW '' ? 
.CODE
.386
BEGIN:
			MOV AX, @DATA
			MOV DS, AX
CALL COMPI
			MOV AX, 4C00H
			INT 21H
COMPI PROC
			I_ASIGNAR 1, t1
			I_ASIGNAR 2, t2
		X1:
			I_IGUAL 1, x, t3
			JF t3, W1
		X2:
			I_IGUAL 2, y, t4
			JF t4, W2
			I_IGUAL 1, x, t5
			JF t5, A1
			I_ASIGNAR 2, t6
			I_ASIGNAR 5, t7
			I_IGUAL 1, x, t8
			JF t8, A2
		A1:
			I_ASIGNAR 2, t9
			I_ASIGNAR 5, t10
			JMP X2
			JMP X1
		W2:
		W1:

	ret
COMPI ENDP
END BEGIN
