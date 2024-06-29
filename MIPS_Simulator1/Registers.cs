using System.Collections.Generic;

namespace MIPS_Simulator1
{
    public class Instruction
    {
        public string Category { get; set; }
        public string Opcode { get; set; }
        public string Rs { get; set; }
        public string Rt { get; set; }
        public string Rd { get; set; }
        public string Immediate { get; set; }
        public string TargetLabel { get; set; }
    }

    public static class Registers
    {
        public static readonly Dictionary<string, string> RegisterMap = new Dictionary<string, string>()
        {
            { "$zero", "000" },
            { "$t0", "001" },
            { "$t1", "010" },
            { "$t2", "011" },
            { "$s0", "100" },
            { "$s1", "101" },
            { "$s2", "110" },
            { "$ra", "111" }
        };
    }

    public class SpecialRegisters
    {
        public const int PC = 0;   // Program Counter, 8 bits
        public const int HI = 1;   // HI register, 8 bits
        public const int LO = 2;   // LO register, 8 bits
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
            { "add", new RTypeInstruction { Opcode = "0000", Funct = "000" } },
            { "sub", new RTypeInstruction { Opcode = "0000", Funct = "001" } },
            { "and", new RTypeInstruction { Opcode = "0000", Funct = "010" } },
            { "or", new RTypeInstruction { Opcode = "0000", Funct = "011" } },
            { "xor", new RTypeInstruction { Opcode = "0000", Funct = "100" } },
            { "slt", new RTypeInstruction { Opcode = "0000", Funct = "101" } },
            { "jr", new RTypeInstruction { Opcode = "0001", Funct = "000" } },
            { "sll", new RTypeInstruction { Opcode = "0010", Funct = "000" } },
            { "srl", new RTypeInstruction { Opcode = "0010", Funct = "001" } },
            { "sra", new RTypeInstruction { Opcode = "0010", Funct = "010" } },
            { "mfhi", new RTypeInstruction { Opcode = "0011", Funct = "000" } },
            { "mflo", new RTypeInstruction { Opcode = "0011", Funct = "001" } },
            { "mult", new RTypeInstruction { Opcode = "0100", Funct = "000" } },
            { "div", new RTypeInstruction { Opcode = "0100", Funct = "001" } }
        };

        public static readonly Dictionary<string, ITypeInstruction> ITypeInstructions = new Dictionary<string, ITypeInstruction>()
        {
            { "beq", new ITypeInstruction { Opcode = "0101" } },
            { "bne", new ITypeInstruction { Opcode = "0110" } },
            { "addi", new ITypeInstruction { Opcode = "0111" } },
            { "slti", new ITypeInstruction { Opcode = "1000" } },
            { "andi", new ITypeInstruction { Opcode = "1001" } },
            { "ori", new ITypeInstruction { Opcode = "1010" } },
            { "lw", new ITypeInstruction { Opcode = "1011" } },
            { "sw", new ITypeInstruction { Opcode = "1100" } },
            { "muli", new ITypeInstruction { Opcode = "1101" } }
        };

        public static readonly Dictionary<string, JTypeInstruction> JTypeInstructions = new Dictionary<string, JTypeInstruction>()
        {
            { "j", new JTypeInstruction { Opcode = "1110" } },
            { "jal", new JTypeInstruction { Opcode = "1111" } }
        };
    }
}
