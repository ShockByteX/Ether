# Ether

Add some additional auto assemble functionality such as adding jump marks and defining data types.

## Example asm

```
float someValue 20000
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
	movss xmm2,[someValue]
	movss [rcx+0x000002C4],xmm2
	jmp return
```

## Usage
> var data = EtherAssembler.Assemble(mnemonics, returnOffset)
