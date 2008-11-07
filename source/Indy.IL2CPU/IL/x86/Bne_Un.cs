using System;
using System.IO;


using CPU = Indy.IL2CPU.Assembler;
using CPUx86 = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Bne_Un)]
	public class Bne_Un: Op {
		public readonly string TargetLabel;
		public readonly string CurInstructionLabel;
		public Bne_Un(ILReader aReader, MethodInformation aMethodInfo)
			: base(aReader, aMethodInfo) {
			TargetLabel = GetInstructionLabel(aReader.OperandValueBranchPosition);
			CurInstructionLabel = GetInstructionLabel(aReader);
		}
		public override void DoAssemble() {
			if (Assembler.StackContents.Peek().IsFloat) {
				throw new Exception("Floats not yet supported!");
			}
			int xSize = Math.Max(Assembler.StackContents.Pop().Size, Assembler.StackContents.Pop().Size);
			if (xSize > 8) {
				throw new Exception("StackSize>8 not supported");
			}
			string BaseLabel = CurInstructionLabel + "__";
			string LabelTrue = BaseLabel + "True";
			string LabelFalse = BaseLabel + "False";
			if (xSize > 4)
			{
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.EAX };
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.EBX };
				//value2 = EBX:EAX
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.ECX };
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.EDX };
				//value1 = EDX:ECX
				new CPUx86.Xor("eax", "ecx");
				new CPUx86.JumpIfNotZero(TargetLabel);
				new CPUx86.Xor("ebx", "edx");
				new CPUx86.JumpIfNotZero(TargetLabel);
			} else
			{
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.EAX };
                new CPUx86.Compare { DestinationReg = CPUx86.Registers.EAX, SourceReg=CPUx86.Registers.ESP, SourceIsIndirect=true};
				new CPUx86.JumpIfEqual(LabelTrue);
				new CPUx86.Jump(LabelFalse);
				new CPU.Label(LabelFalse);
                new CPUx86.Add { DestinationReg = CPUx86.Registers.ESP, SourceValue = 4 };
				new CPUx86.Jump(TargetLabel);
				new CPU.Label(LabelTrue);
				new CPUx86.Add{DestinationReg = CPUx86.Registers.ESP, SourceValue=4};
			}
		}
	}
}