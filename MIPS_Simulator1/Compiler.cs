using MIPS_Simulator1;
using System;
using System.Collections.Generic;
using System.Linq;

public static class Compiler
{
    public static List<string> CompileToHex(List<string> assemblyCode)
    {
        var machineCode = new List<string>();
        var labels = new Dictionary<string, int>();
        int addressCounter = 0;

        // First pass: record label addresses
        foreach (var instruction in assemblyCode)
        {
            if (instruction.EndsWith(":"))
            {
                string label = instruction.TrimEnd(':');
                labels[label] = addressCounter;
            }
            else
            {
                addressCounter++;
            }
        }

        // Second pass: compile instructions, resolving labels
        addressCounter = 0;
        foreach (var instruction in assemblyCode)
        {
            if (!instruction.EndsWith(":"))
            {
                var compiledInstruction = CompileInstruction(instruction, labels, addressCounter);
                var hexCode = Convert.ToUInt32(compiledInstruction, 2).ToString("X").PadLeft(8, '0');
                machineCode.Add(hexCode);
                addressCounter++;
            }
        }

        return machineCode;
    }


    public static List<string> CompileToBin(List<string> assemblyCode)
    {
        var machineCode = new List<string>();
        var labels = new Dictionary<string, int>();
        int addressCounter = 0;

        // First pass: record label addresses
        foreach (var instruction in assemblyCode)
        {
            if (instruction.EndsWith(":"))
            {
                string label = instruction.TrimEnd(':');
                labels[label] = addressCounter;
            }
            else
            {
                addressCounter++;
            }
        }

        // Second pass: compile instructions, resolving labels
        addressCounter = 0;
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

    public static string CompileInstruction(string instruction, Dictionary<string, int> labels, int currentAddress)
    {
        var parsedInstruction = Parser.ParseInstruction(instruction);

        switch (parsedInstruction.Category)
        {
            case "Register":
                return CompileRTypeInstruction(parsedInstruction);
            case "Immediate":
            case "LoadStore":
            case "LoadUpperImmediate":
                return CompileITypeInstruction(parsedInstruction);
            case "Branch":
                return CompileBranchInstruction(parsedInstruction, labels, currentAddress);
            case "Jump":
                return CompileJTypeInstruction(parsedInstruction, labels, currentAddress);
            default:
                throw new Exception($"Unknown instruction category: {parsedInstruction.Category}");
        }
    }

    private static string CompileBranchInstruction(dynamic parsedInstruction, Dictionary<string, int> labels, int currentAddress)
    {
        var opcodeValue = Instructions.ITypeInstructions[parsedInstruction.Opcode].Opcode;

        if (!labels.ContainsKey(parsedInstruction.TargetLabel))
        {
            throw new Exception($"Undefined label: {parsedInstruction.TargetLabel}");
        }

        int targetAddress = labels[parsedInstruction.TargetLabel];
        int offset = targetAddress - (currentAddress + 1);

        var rsValue = Registers.RegisterMap[parsedInstruction.Rs];
        var rtValue = Registers.RegisterMap[parsedInstruction.Rt];

        // Convert offset to a 16-bit two's complement binary string
        string offsetBinary = Convert.ToString((short)offset, 2).PadLeft(16, '0');

        return opcodeValue + rsValue + rtValue + offsetBinary;
    }



    private static string CompileRTypeInstruction(dynamic parsedInstruction)
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
                       "00000" +
                       functValue;
            case "Shift":
                return opcodeValue +
                       "00000" +
                       Registers.RegisterMap[parsedInstruction.Rt] +
                       Registers.RegisterMap[parsedInstruction.Rd] +
                       ConvertImmediateToBinary(parsedInstruction.Shamt, 5) +
                       functValue;
            case "MultDiv":
                return opcodeValue +
                       Registers.RegisterMap[parsedInstruction.Rs] +
                       Registers.RegisterMap[parsedInstruction.Rt] +
                       "00000" +
                       "00000" +
                       functValue;
            case "MoveFrom":
                return opcodeValue +
                       "00000" +
                       "00000" +
                       Registers.RegisterMap[parsedInstruction.Rd] +
                       "00000" +
                       functValue;
            case "RJump":
                return opcodeValue +
                       Registers.RegisterMap[parsedInstruction.Rs] +
                       "00000" +
                       "00000" +
                       "00000" +
                       functValue;
            default:
                throw new Exception($"Invalid R-Type instruction: {parsedInstruction}");
        }
    }

    private static string CompileITypeInstruction(dynamic parsedInstruction)
    {
        var opcodeValue = Instructions.ITypeInstructions[parsedInstruction.Opcode].Opcode;

        if (parsedInstruction.Category == "LoadUpperImmediate")
        {
            return opcodeValue +
                   "00000" +
                   Registers.RegisterMap[parsedInstruction.Rt] +
                   ConvertImmediateToBinary(parsedInstruction.Immediate, 16);
        }
        else
        {
            return opcodeValue +
                   Registers.RegisterMap[parsedInstruction.Rs] +
                   Registers.RegisterMap[parsedInstruction.Rt] +
                   ConvertImmediateToBinary(parsedInstruction.Immediate, 16);
        }
    }

    private static string CompileJTypeInstruction(dynamic parsedInstruction, Dictionary<string, int> labels, int currentAddress)
    {
        var opcodeValue = Instructions.JTypeInstructions[parsedInstruction.Opcode].Opcode;

        if (parsedInstruction.TargetLabel != null)
        {
            if (!labels.ContainsKey(parsedInstruction.TargetLabel))
            {
                throw new Exception($"Undefined label: {parsedInstruction.TargetLabel}");
            }

            int targetAddress = labels[parsedInstruction.TargetLabel];
            int offset = targetAddress - (currentAddress + 1);
            return opcodeValue + Convert.ToString(offset, 2).PadLeft(26, '0');
        }
        else
        {
            int targetAddress = int.Parse(parsedInstruction.Target);
            return opcodeValue + Convert.ToString(targetAddress, 2).PadLeft(26, '0');
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
