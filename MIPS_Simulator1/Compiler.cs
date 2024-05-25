using MIPS_Simulator1;
using System;
using System.Collections.Generic;
using System.Linq;

public static class Compiler
{
    public static List<string> CompileToHex(List<string> assemblyCode)
    {
        var machineCode = new List<string>();
        foreach (var instruction in assemblyCode)
        {
            var compiledInstruction = CompileInstruction(instruction);
            var hexCode = Convert.ToInt32(compiledInstruction, 2).ToString("X").PadLeft(8, '0');
            machineCode.Add(hexCode);
        }
        return machineCode;
    }

    public static List<string> CompileToBin(List<string> assemblyCode)
    {
        var machineCode = new List<string>();
        foreach (var instruction in assemblyCode)
        {
            machineCode.Add(CompileInstruction(instruction));
        }
        return machineCode;
    }

    public static string CompileInstruction(string instruction)
    {
        var parsedInstruction = Parser.ParseInstruction(instruction);
        var opcode = parsedInstruction.Opcode;

        if (Instructions.RTypeInstructions.ContainsKey(opcode))
        {
            return CompileRTypeInstruction(parsedInstruction);
        }
        else if (Instructions.ITypeInstructions.ContainsKey(opcode))
        {
            return CompileITypeInstruction(parsedInstruction);
        }
        else if (Instructions.JTypeInstructions.ContainsKey(opcode))
        {
            return CompileJTypeInstruction(parsedInstruction);
        }
        else
        {
            throw new Exception($"Unknown instruction: {instruction}");
        }
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

    private static string CompileJTypeInstruction(dynamic parsedInstruction)
    {
        var opcodeValue = Instructions.JTypeInstructions[parsedInstruction.Opcode].Opcode;
        return opcodeValue + ConvertImmediateToBinary(parsedInstruction.Target, 26);
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
