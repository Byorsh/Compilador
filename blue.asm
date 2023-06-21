;/StartHeader
INCLUDE macros.mac
DOSSEG
.MODEL SMALL
STACK 100h
.DATA
			a DW '', '$' 
			sum DW '', '$' 
			t1 DW '' ? 
			t2 DW '' ? 
			t3 DW '' ? 
			t4 DW '' ? 
			t5 DW '' ? 
			t6 DW '' ? 
			t7 DW '' ? 
			t8 DW '' ? 
			t9 DW '' ? 
			t10 DW '' ? 
			t11 DW '' ? 
.CODE
.386
BEGIN:
			MOV AX, @DATA
			MOV DS, AX
CALL COMPI
			MOV AX, 4C00H
			INT 21H
COMPI PROC
			READ a
			READ sum
		X1:
			I_MAYORIGUAL 9, a, t1
			JF t1, W1
			WRITE "1"
			I_MENOR 4, a, t2
			JF t2, A1
			WRITE "2"
			SUMAR 1, a, t3
			I_ASIGNAR a, t3
			JMP E1
		A1:
			WRITE "3"
		X2:
			I_MAYOR 5, a, t5
			JF t5, W2
			SUMAR a, sum, t6
			I_ASIGNAR sum, t6
			RESTA 2, a, t7
			I_ASIGNAR a, t7
			JMP X2
		W2:
			WRITE sum
			SUMAR 1, a, t9
			I_ASIGNAR a, t9
		E1:
			WRITE "4"
			JMP X1
		W1:
			I_ASIGNAR 9, t11

	ret
COMPI ENDP
END BEGIN
