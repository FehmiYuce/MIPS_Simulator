using System;
using System.Collections.Generic;
using MIPS_Simulator1;

public static class Compiler
{
    public static List<string> CompileToHex(List<string> assemblyCode)
{
    var machineCode = new List<string>();
    var labels = CollectLabels(assemblyCode);
    int addressCounter = 0;

    foreach (var instruction in assemblyCode)
    {
        if (!instruction.EndsWith(":"))
        {
            var compiledInstruction = CompileInstruction(instruction, labels, addressCounter);
            if (!string.IsNullOrEmpty(compiledInstruction))
            {
                uint parsedValue = unchecked((uint)Convert.ToUInt32(compiledInstruction, 2));
                var hexCode = parsedValue.ToString("X4"); // 16-bit instruction
                machineCode.Add(hexCode);
                addressCounter++;
            }
        }
    }

    return machineCode;
}


    public static List<string> CompileToBin(List<string> assemblyCode)
    {
        var machineCode = new List<string>();
        var labels = CollectLabels(assemblyCode);

        int addressCounter = 0;
        foreach (var instruction in assemblyCode)
        {
            if (!instruction.EndsWith(":"))
            {
                machineCode.Add(CompileInstruction(instruction, labels, addressCounter));
                addressCounter++;
            }
        }

        return machineCode;
    }

    private static Dictionary<string, int> CollectLabels(List<string> assemblyCode)
    {
        var labels = new Dictionary<string, int>();
        int addressCounter = 0; // Initialize counter for addresses

        foreach (var line in assemblyCode)
        {
            string trimmedLine = line.Trim();
            if (trimmedLine.EndsWith(":"))
            {
                // Remove the colon and any trailing whitespace, then add to labels dictionary
                string label = trimmedLine.TrimEnd(':').Trim();
                if (!labels.ContainsKey(label))
                {
                    labels[label] = addressCounter;
                }
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                // Only increment address counter if the line is a valid instruction (not empty or just a label)
                addressCounter += 2; // Assuming each instruction is 2 bytes
            }
        }
        return labels;
    }


    public static string CompileInstruction(string instruction, Dictionary<string, int> labels, int currentAddress)
    {
        var parsedInstruction = Parser.ParseInstruction(instruction);

        switch (parsedInstruction.Category)
        {
            case "Register":
            case "MultDiv":
            case "RJump":
                return CompileRTypeInstruction(parsedInstruction);
            case "Immediate":
            case "LoadStore":
                return CompileITypeInstruction(parsedInstruction);
            case "Branch":
                return CompileBranchInstruction(parsedInstruction, labels, currentAddress);
            case "Jump":
                return CompileJTypeInstruction(parsedInstruction, labels, currentAddress);
            case "MoveFrom":
                return CompileMoveFromInstruction(parsedInstruction);
            case "Label":
                // For labels, you might want to record the label's position or simply skip compilation
                return null;  // Return null or an empty string if no machine code is necessary
            default:
                throw new Exception($"Unknown instruction category: {parsedInstruction.Category}");
        }
    }


    private static string CompileBranchInstruction(Instruction parsedInstruction, Dictionary<string, int> labels, int currentAddress)
    {
        var opcodeValue = Instructions.ITypeInstructions[parsedInstruction.Opcode].Opcode;
        if (!labels.ContainsKey(parsedInstruction.TargetLabel))
        {
            throw new Exception($"Undefined label: {parsedInstruction.TargetLabel}");
        }

        int targetAddress = labels[parsedInstruction.TargetLabel];
        // Offset must be from the next instruction, and each instruction is 2 bytes in this setup
        int offset = (targetAddress - (currentAddress + 1)) >> 1;

        var rsValue = Registers.RegisterMap[parsedInstruction.Rs];
        var rtValue = Registers.RegisterMap[parsedInstruction.Rt];
        string offsetBinary = Convert.ToString((short)offset, 2).PadLeft(6, '0'); // Ensure it's a 16-bit offset

        return opcodeValue + rsValue + rtValue + offsetBinary;
    }



    private static string CompileRTypeInstruction(Instruction parsedInstruction)
    {
        var opcodeValue = Instructions.RTypeInstructions[parsedInstruction.Opcode].Opcode;
        var functValue = Instructions.RTypeInstructions[parsedInstruction.Opcode].Funct;

        switch (parsedInstruction.Category)
        {
            case "Register":
                return opcodeValue +
                       Registers.RegisterMap[parsedInstruction.Rs] +
                       Registers.RegisterMap[parsedInstruction.Rt] +
                       Registers.RegisterMap[parsedInstruction.Rd] +
                       functValue;
            case "MultDiv":
                return opcodeValue +
                       Registers.RegisterMap[parsedInstruction.Rs] +
                       Registers.RegisterMap[parsedInstruction.Rt] +
                       "000" +
                       functValue;
            case "RJump":
                return opcodeValue +
                       Registers.RegisterMap[parsedInstruction.Rs] +
                       "000" +
                       "000" +
                       functValue;
            default:
                throw new Exception($"Invalid R-Type instruction: {parsedInstruction}");
        }
    }

    private static string CompileMoveFromInstruction(Instruction parsedInstruction)
    {
        var opcodeValue = Instructions.RTypeInstructions[parsedInstruction.Opcode].Opcode;
        var functValue = Instructions.RTypeInstructions[parsedInstruction.Opcode].Funct;

        return opcodeValue +
               "000" +
               "000" +
               Registers.RegisterMap[parsedInstruction.Rd] +
               functValue;
    }

    private static string CompileITypeInstruction(Instruction parsedInstruction)
    {
        var opcodeValue = Instructions.ITypeInstructions[parsedInstruction.Opcode].Opcode;

        return opcodeValue +
               Registers.RegisterMap[parsedInstruction.Rs] +
               Registers.RegisterMap[parsedInstruction.Rt] +
               ConvertImmediateToBinary(parsedInstruction.Immediate, 6);
    }

    private static string CompileJTypeInstruction(Instruction parsedInstruction, Dictionary<string, int> labels, int currentAddress)
    {
        var opcodeValue = Instructions.JTypeInstructions[parsedInstruction.Opcode].Opcode;

        if (parsedInstruction.TargetLabel != null)
        {
            if (!labels.ContainsKey(parsedInstruction.TargetLabel))
            {
                throw new Exception($"Undefined label: {parsedInstruction.TargetLabel}");
            }

            int targetAddress = labels[parsedInstruction.TargetLabel];
            return opcodeValue + Convert.ToString(targetAddress, 2).PadLeft(12, '0');
        }

        else
        {
            int targetAddress = int.Parse(parsedInstruction.Immediate);
            return opcodeValue + Convert.ToString(targetAddress, 2).PadLeft(12, '0');
        }
    }

    public static string ConvertImmediateToBinary(string immediate, int length)
    {
        if (immediate.StartsWith('-'))
        {
            return Convert.ToString((int)Math.Pow(2, length) + int.Parse(immediate), 2).Substring(1).PadLeft(length, '0');
        }
        else if (immediate.StartsWith("0x"))
        {
            return Convert.ToString(int.Parse(immediate.Substring(2), System.Globalization.NumberStyles.HexNumber), 2).PadLeft(length, '0');
        }
        else
        {
            return Convert.ToString(int.Parse(immediate), 2).PadLeft(length, '0');
        }
    }
}
