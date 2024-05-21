using System.Collections.Generic;

namespace MIPS_Simulator1
{
	public static class Registers
	{
		public static readonly Dictionary<string, string> RegisterMap = new Dictionary<string, string>()
	{
		{ "$zero","00000" },
		{ "$at", "00001" },
		{ "$v0", "00010" },
		{ "$v1", "00011" },
		{ "$a0", "00100" },
		{ "$a1", "00101" },
		{ "$a2", "00110" },
		{ "$a3", "00111" },
		{ "$t0", "01000" },
		{ "$t1", "01001" },
		{ "$t2", "01010" },
		{ "$t3", "01011" },
		{ "$t4", "01100" },
		{ "$t5", "01101" },
		{ "$t6", "01110" },
		{ "$t7", "01111" },
		{ "$s0", "10000" },
		{ "$s1", "10001" },
		{ "$s2", "10010" },
		{ "$s3", "10011" },
		{ "$s4", "10100" },
		{ "$s5", "10101" },
		{ "$s6", "10110" },
		{ "$s7", "10111" },
		{ "$t8", "11000" },
		{ "$t9", "11001" },
		{ "$k0", "11010" },
		{ "$k1", "11011" },
		{ "$gp", "11100" },
		{ "$sp", "11101" },
		{ "$fp", "11110" },
		{ "$ra", "11111" }
	};
	}

	public class RTypeInstruction
    {
        public string Opcode { get; set; } = string.Empty;
        public string Funct { get; set; } = string.Empty;
    }

    public class ITypeInstruction
    {
        public string Opcode { get; set; } = string.Empty;
    }

    public class JTypeInstruction
    {
        public string Opcode { get; set; } = string.Empty;
    }

	public class Instructions
	{
		public static readonly Dictionary<string, RTypeInstruction> RTypeInstructions = new Dictionary<string, RTypeInstruction>()
	{
		{ "add", new RTypeInstruction { Opcode = "000000", Funct = "100000" } },
		{ "sub", new RTypeInstruction { Opcode = "000000", Funct = "100010" } },
		{ "and", new RTypeInstruction { Opcode = "000000", Funct = "100100" } },
		{ "or", new RTypeInstruction { Opcode = "000000", Funct = "100101" } },
		{ "xor", new RTypeInstruction { Opcode = "000000", Funct = "100110" } },
		{ "slt", new RTypeInstruction { Opcode = "000000", Funct = "101010" } },
		{ "jr", new RTypeInstruction { Opcode = "000000", Funct = "001000" } },
		{ "sll", new RTypeInstruction { Opcode = "000000", Funct = "000000" } },
		{ "srl", new RTypeInstruction { Opcode = "000000", Funct = "000010" } },
		{ "sra", new RTypeInstruction { Opcode = "000000", Funct = "000011" } },
		{ "mfhi", new RTypeInstruction { Opcode = "000000", Funct = "010000" } },
		{ "mflo", new RTypeInstruction { Opcode = "000000", Funct = "010010" } },
		{ "mult", new RTypeInstruction { Opcode = "000000", Funct = "011000" } },
		{ "div", new RTypeInstruction { Opcode = "000000", Funct = "011010" } }
	};

		public static readonly Dictionary<string, ITypeInstruction> ITypeInstructions = new Dictionary<string, ITypeInstruction>()
	{
		{ "beq", new ITypeInstruction { Opcode = "000100" } },
		{ "bne", new ITypeInstruction { Opcode = "000101" } },
		{ "addi", new ITypeInstruction { Opcode = "001000" } },
		{ "slti", new ITypeInstruction { Opcode = "001010" } },
		{ "andi", new ITypeInstruction { Opcode = "001100" } },
		{ "ori", new ITypeInstruction { Opcode = "001101" } },
		{ "lui", new ITypeInstruction { Opcode = "001111" } },
		{ "lw", new ITypeInstruction { Opcode = "100011" } },
		{ "lb", new ITypeInstruction { Opcode = "100000" } },
		{ "sw", new ITypeInstruction { Opcode = "101011" } },
		{ "sb", new ITypeInstruction { Opcode = "101000" } }
	};

		public static readonly Dictionary<string, JTypeInstruction> JTypeInstructions = new Dictionary<string, JTypeInstruction>()
	{
		{ "j", new JTypeInstruction { Opcode = "000010" } },
		{ "jal", new JTypeInstruction { Opcode = "000011" } }
	};
	}

}
