using MIPS_Simulator1;

public static class Parser
{
    public static (string category, string opcode, string rd, string rs, string rt, string shamt, string immediate, string target) ParseInstruction(string instruction)
    {
        string[] parts = instruction.Trim().Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
        {
            throw new Exception("Instruction cannot be empty");
        }

        string opcode = parts[0];
        string rd = null, rs = null, rt = null, shamt = null, immediate = null, target = null;

        if (Instructions.RTypeInstructions.ContainsKey(opcode))
        {
            string funct = Instructions.RTypeInstructions[opcode].Funct;
            if (new[] { "sll", "srl", "sra" }.Contains(opcode))
            {
                if (parts.Length < 4)
                {
                    throw new Exception($"Invalid instruction format for {opcode}: {instruction}");
                }
                rd = parts[1];
                rt = parts[2];
                shamt = parts[3];
                return ("Shift", opcode, rd, null, rt, shamt, null, null);
            }
            else if (new[] { "jr" }.Contains(opcode))
            {
                if (parts.Length < 2)
                {
                    throw new Exception($"Invalid instruction format for {opcode}: {instruction}");
                }
                rs = parts[1];
                return ("RJump", opcode, null, rs, null, null, null, null);
            }
            else if (new[] { "mfhi", "mflo" }.Contains(opcode))
            {
                if (parts.Length < 2)
                {
                    throw new Exception($"Invalid instruction format for {opcode}: {instruction}");
                }
                rd = parts[1];
                return ("MoveFrom", opcode, rd, null, null, null, null, null);
            }
            else if (new[] { "mult", "div" }.Contains(opcode))
            {
                if (parts.Length < 3)
                {
                    throw new Exception($"Invalid instruction format for {opcode}: {instruction}");
                }
                rs = parts[1];
                rt = parts[2];
                return ("MultDiv", opcode, null, rs, rt, null, null, null);
            }
            else
            {
                if (parts.Length < 4)
                {
                    throw new Exception($"Invalid instruction format for {opcode}: {instruction}");
                }
                rd = parts[1];
                rs = parts[2];
                rt = parts[3];
                return ("Register", opcode, rd, rs, rt, null, null, null);
            }
        }
        else if (Instructions.ITypeInstructions.ContainsKey(opcode))
        {
            if (opcode == "lui")
            {
                if (parts.Length < 3)
                {
                    throw new Exception($"Invalid instruction format for {opcode}: {instruction}");
                }
                rt = parts[1];
                immediate = parts[2];
                return ("LoadUpperImmediate", opcode, null, null, rt, null, immediate, null);
            }
            else if (new[] { "lw", "lb", "sw", "sb" }.Contains(opcode))
            {
                if (parts.Length < 3)
                {
                    throw new Exception($"Invalid instruction format for {opcode}: {instruction}");
                }
                rt = parts[1];
                var offsetAndBase = parts[2].Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                if (offsetAndBase.Length < 2)
                {
                    throw new Exception($"Invalid address format for {opcode}: {instruction}");
                }
                immediate = offsetAndBase[0];
                rs = offsetAndBase[1];
                return ("LoadStore", opcode, null, rs, rt, null, immediate, null);
            }
            else
            {
                if (parts.Length < 4)
                {
                    throw new Exception($"Invalid instruction format for {opcode}: {instruction}");
                }
                rt = parts[1];
                rs = parts[2];
                immediate = parts[3];
                return ("Immediate", opcode, null, rs, rt, null, immediate, null);
            }
        }
        else if (Instructions.JTypeInstructions.ContainsKey(opcode))
        {
            if (parts.Length < 2)
            {
                throw new Exception($"Invalid instruction format for {opcode}: {instruction}");
            }
            target = parts[1];
            return ("Jump", opcode, null, null, null, null, null, target);
        }

        throw new Exception($"Invalid instruction format: {instruction}");
    }
}
