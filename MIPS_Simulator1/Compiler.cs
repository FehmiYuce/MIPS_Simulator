using MIPS_Simulator1;

public class Compiler
{
    private static Dictionary<string, int> Labels = new Dictionary<string, int>();

    public static void SetLabels(Dictionary<string, int> labels)
    {
        Labels = labels;
    }

    public static List<string> CompileToHex(List<string> assemblyCode)
    {
        List<string> machineCode = new List<string>();
        foreach (string instruction in assemblyCode)
        {
            string compiledInstruction = CompileInstruction(instruction);
            string hexCode = Convert.ToInt32(compiledInstruction, 2).ToString("X").PadLeft(8, '0');
            machineCode.Add(hexCode);
        }
        return machineCode;
    }

    public static List<string> CompileToBin(List<string> assemblyCode)
    {
        List<string> machineCode = new List<string>();
        foreach (string instruction in assemblyCode)
        {
            machineCode.Add(CompileInstruction(instruction));
        }
        return machineCode;
    }

    public static string CompileInstruction(string instruction)
    {
        string[] parts = instruction.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string opcode = parts[0];
        string[] args = parts.Skip(1).ToArray();

        if (Instructions.RTypeInstructions.ContainsKey(opcode))
        {
            return CompileRTypeInstruction(instruction);
        }
        else if (Instructions.ITypeInstructions.ContainsKey(opcode))
        {
            return CompileITypeInstruction(instruction);
        }
        else if (Instructions.JTypeInstructions.ContainsKey(opcode))
        {
            return CompileJTypeInstruction(instruction);
        }
        else
        {
            throw new Exception($"Unknown instruction: {instruction}");
        }
    }

    private static string CompileRTypeInstruction(string instruction)
    {
        var (category, opcode, rd, rs, rt, shamt, immediate, target) = Parser.ParseInstruction(instruction);
        string opcodeValue = Instructions.RTypeInstructions[opcode].Opcode;
        string functValue = Instructions.RTypeInstructions[opcode].Funct;

        switch (category)
        {
            case "Register":
                return opcodeValue +
                    Registers.RegisterMap[rs] +
                    Registers.RegisterMap[rt] +
                    Registers.RegisterMap[rd] +
                    "00000" +
                    functValue;
            case "Shift":
                return opcodeValue +
                    "00000" +
                    Registers.RegisterMap[rt] +
                    Registers.RegisterMap[rd] +
                    ConvertImmediateToBinary(shamt, 5) +
                    functValue;
            case "MultDiv":
                return opcodeValue +
                    Registers.RegisterMap[rs] +
                    Registers.RegisterMap[rt] +
                    "00000" +
                    "00000" +
                    functValue;
            case "MoveFrom":
                return opcodeValue +
                    "00000" +
                    "00000" +
                    Registers.RegisterMap[rd] +
                    "00000" +
                    functValue;
            case "RJump":
                return opcodeValue +
                    Registers.RegisterMap[rs] +
                    "00000" +
                    "00000" +
                    "00000" +
                    functValue;
            default:
                throw new Exception($"Invalid R-Type instruction: {instruction}");
        }
    }

    private static string CompileITypeInstruction(string instruction)
    {
        var (category, opcode, rd, rs, rt, shamt, immediate, target) = Parser.ParseInstruction(instruction);
        string opcodeValue = Instructions.ITypeInstructions[opcode].Opcode;

        if (category == "LoadUpperImmediate")
        {
            return opcodeValue +
                "00000" +
                Registers.RegisterMap[rt] +
                ConvertImmediateToBinary(immediate, 16);
        }
        else
        {
            return opcodeValue +
                Registers.RegisterMap[rs] +
                Registers.RegisterMap[rt] +
                ConvertImmediateToBinary(immediate, 16);
        }
    }

    private static string CompileJTypeInstruction(string instruction)
    {
        var (category, opcode, rd, rs, rt, shamt, immediate, target) = Parser.ParseInstruction(instruction);
        string opcodeValue = Instructions.JTypeInstructions[opcode].Opcode;

        // Check if the target is a label
        if (Labels.TryGetValue(target, out int address))
        {
            return opcodeValue + ConvertImmediateToBinary(address.ToString(), 26);
        }

        // If it's not a label, treat it as an immediate
        return opcodeValue + ConvertImmediateToBinary(target, 26);
    }

    private static string ConvertImmediateToBinary(string immediate, int length)
    {
        // If the immediate is a label, it should be converted to its address
        if (Labels.TryGetValue(immediate, out int address))
        {
            immediate = address.ToString();
        }

        // Check for valid integer formats and handle them
        if (immediate.StartsWith('-'))
        {
            return Convert.ToString((int)Math.Pow(2, length) + int.Parse(immediate), 2).Substring(1).PadLeft(length, '0');
        }
        else if (immediate.StartsWith("0x"))
        {
            return Convert.ToString(Convert.ToInt32(immediate.Substring(2), 16), 2).PadLeft(length, '0');
        }
        else if (immediate.StartsWith("0b"))
        {
            return immediate.Substring(2).PadLeft(length, '0');
        }
        else
        {
            return Convert.ToString(int.Parse(immediate), 2).PadLeft(length, '0');
        }
    }
}
