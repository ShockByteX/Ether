# Ether

Add some additional auto assemble functionality such as adding jump marks and defining data types.

# Example asm

#change
	mov byte [rax],0
	jmp code
#main
	cmp r8,1000
	ja change 
	cmp r8,0
	je change
	nop
#code
	cmp [rax],cl
	mov eax,[rdx+10]
	jmp return
