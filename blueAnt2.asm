;/StartHeader
INCLUDE macros.mac
DOSSEG
.MODEL SMALL
STACK 100h
.DATA
		a DW '', '$' 
		b DW '', '$' 
		t1 DW '' ? 
		t2 DW '' ? 
		t3 DW '' ? 
.CODE
.386
BEGIN:
		MOV AX, @DATA
		MOV DS, AX
CALL COMPI
		MOV AX, 4C00H
		INT 21H
COMPI PROC

<<<<<<< HEAD:blueAnt.asm
			MOV AL, x
=======
			MOV AL, a
>>>>>>> 5b69c09a67278c7458ae1e885680423fa7f374fd:blue.asm
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

<<<<<<< HEAD:blueAnt.asm
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
=======
		JF t2, A1

			MOV AL, a
			MOV 1, AL
			MOV AL, b
			MOV 1, AL	A1:
>>>>>>> 5b69c09a67278c7458ae1e885680423fa7f374fd:blue.asm

	ret
MOV AH, 4cH
INT 21H
END BEGIN
