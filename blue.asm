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
			MOV AL, x
			MOV 1, AL
			MOV AL, y
			MOV 2, AL
	X1:

		LOCAL LABEL1 
		LOCAL SALIR 
			MOV AL, 1
			CMP AL, x
			JNE LABEL1
			MOV t3 1
			JMP SALIR
		LABEL1: 
			MOV t3, 0
		SALIR:

		JF t3, W1
	X2:

		LOCAL LABEL1 
		LOCAL SALIR 
			MOV AL, 2
			CMP AL, y
			JNE LABEL1
			MOV t4 1
			JMP SALIR
		LABEL1: 
			MOV t4, 0
		SALIR:

		JF t4, W2

		LOCAL LABEL1 
		LOCAL SALIR 
			MOV AL, 1
			CMP AL, x
			JNE LABEL1
			MOV t5 1
			JMP SALIR
		LABEL1: 
			MOV t5, 0
		SALIR:

		JF t5, A1
			MOV AL, x
			MOV 2, AL
			MOV AL, y
			MOV 5, AL
	A1:

		LOCAL LABEL1 
		LOCAL SALIR 
			MOV AL, 1
			CMP AL, x
			JNE LABEL1
			MOV t8 1
			JMP SALIR
		LABEL1: 
			MOV t8, 0
		SALIR:

		JF t8, A2
			MOV AL, x
			MOV 2, AL
			MOV AL, y
			MOV 5, AL
	A2:


		JMP X2


		JMP X1
	W2:
	W1:

	ret
COMPI ENDP
END BEGIN
