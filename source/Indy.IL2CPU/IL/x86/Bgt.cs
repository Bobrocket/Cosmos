using System;
using System.IO;


using CPU = Indy.IL2CPU.Assembler;
using CPUx86 = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Bgt)]
	public class Bgt: Op {
		public readonly string TargetLabel;
		public readonly string CurInstructionLabel;
		public Bgt(ILReader aReader, MethodInformation aMethodInfo)
			: base(aReader, aMethodInfo) {
			TargetLabel = GetInstructionLabel(aReader.OperandValueBranchPosition);
			CurInstructionLabel = GetInstructionLabel(aReader);
		}
		public override void DoAssemble() {
			string BaseLabel = CurInstructionLabel + "__";
			string LabelTrue = BaseLabel + "True";
			string LabelFalse = BaseLabel + "False";
			var xStackContent = Assembler.StackContents.Pop();
			if (xStackContent.IsFloat) {
				throw new Exception("Floats not yet supported!");
			}
			Assembler.StackContents.Pop();
			if (xStackContent.Size > 8) {
				throw new Exception("StackSize>8 not supported");
			}
			if (xStackContent.Size > 4)
			{
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.EAX };
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.EDX };
				//value2: EDX:EAX
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.EBX };
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.ECX };
                //value1: ECX:EBX
				new CPUx86.Sub("ebx", "eax");
				new CPUx86.SubWithCarry("ecx", "edx");
				//result = value1 - value2
				new CPUx86.JumpOnGreater(TargetLabel);
			}else
			{
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.EAX };
				new CPUx86.Compare{DestinationReg=CPUx86.Registers.EAX, SourceReg=CPUx86.Registers.ESP, SourceIsIndirect=true};
				new CPUx86.JumpIfGreater(LabelTrue);
				new CPUx86.Jump(LabelFalse);
				new CPU.Label(LabelTrue);
                new CPUx86.Add { DestinationReg = CPUx86.Registers.ESP, SourceValue = 4 };
				new CPUx86.Jump(TargetLabel);
				new CPU.Label(LabelFalse);
                new CPUx86.Add { DestinationReg = CPUx86.Registers.ESP, SourceValue = 4 };
			}
		}
	}
}