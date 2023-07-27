;/StartHeader
include macros.asm
.MODEL SMALL
.STACK 100h
.DATA
		a DW '', '$' 
		b DW '', '$' 
		t1 DW '' ? 
		t2 DW '' ? 
		t3 DW '' ? 
.CODE

BEGIN:
		MOV AX, @DATA
		MOV DS, AX
CALL COMPI
		MOV AX, 4C00H
		INT 21H
COMPI PROC

			MOV AL, a
			MOV 1, AL
		LOCAL LABEL1 
		LOCAL SALIR 
			MOV AL, 1
			CMP AL, a
			JNE LABEL1
			MOV t2 1
			JMP SALIR
		LABEL1: 
			MOV t2, 0
		SALIR:

		JF t2, A1

			MOV AL, a
			MOV 1, AL
			MOV AL, b
			MOV 1, AL	A1:

	ret
END BEGIN
MOV AH, 4cH
INT 21H
END 
