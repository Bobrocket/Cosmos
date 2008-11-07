﻿using System;

namespace Indy.IL2CPU.Assembler.X86 {
    public class Registers_Old {
		public const string EAX = "eax";
		public const string AX = "ax";
		public const string AL = "al";
		public const string AH = "ah";
		public const string EBX = "ebx";
		public const string BX = "bx";
		public const string BL = "bl";
		public const string BH = "bh";
		public const string ECX = "ecx";
		public const string CX = "cx";
		public const string CL = "cl";
		public const string CH = "ch";
		public const string EDX = "edx";
		public const string DX = "dx";
		public const string DL = "dl";
		public const string DH = "dh";
		public const string AtEAX = "[eax]";
		public const string AtEBX = "[ebx]";
		public const string AtECX = "[ecx]";
		public const string AtEDX = "[edx]";
		public const string ESP = "esp";
		public const string AtESP = "[esp]";
		public const string EBP = "ebp";
	    public const string AtEBP = "[EBP]";
		public const string EDI = "edi";
		public const string AtEDI = "[edi]";
		public const string ESI = "esi";
		public const string AtESI = "[esi]";
	    public const string CR0 = "CR0";
        public const string CR1 = "CR1";
        public const string CR2 = "CR2";
        public const string CR3 = "CR3";
        public const string CR4 = "CR4";
	}
}
namespace Indy.IL2CPU.Assembler.x86 {
    public class Registers_Old:X86.Registers_Old{}
}