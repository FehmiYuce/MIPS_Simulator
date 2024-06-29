using System;
using System.Text.RegularExpressions;

namespace MIPS_Simulator1
{
    public static class Parser
    {
        public static Instruction ParseInstruction(string instruction)
        {
            instruction = instruction.Trim();
            if (instruction.EndsWith(":"))
            {
                string labelName = instruction.TrimEnd(':').Trim();
                if (labelName.Equals("exit"))
                {
                    return new Instruction { Category = "Label", Opcode = "exit" };
                }
                return new Instruction { Category = "Label", Opcode = labelName };
            }

            string[] parts = instruction.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string opcode = parts[0];

            var rTypeInstruction = ParseRtype(instruction);
            var iTypeInstruction = ParseItype(instruction);
            var jTypeInstruction = ParseJtype(instruction);

            if (Instructions.RTypeInstructions.ContainsKey(opcode) && rTypeInstruction != null)
            {
                return rTypeInstruction;
            }
            else if (Instructions.ITypeInstructions.ContainsKey(opcode) && iTypeInstruction != null)
            {
                return iTypeInstruction;
            }
            else if (Instructions.JTypeInstructions.ContainsKey(opcode) && jTypeInstruction != null)
            {
                return jTypeInstruction;
            }
            else
            {
                throw new Exception($"Invalid instruction: {instruction}");
            }
        }
        private static Instruction ParseRtype(string instruction)
        {
            Regex rTypeRegex = new Regex(@"^(\w+)\s+\$(\w+),\s*\$(\w+),\s*\$(\w+)$", RegexOptions.IgnoreCase);
            Regex multDivRegex = new Regex(@"^(\w+)\s+\$(\w+),\s*\$(\w+)$", RegexOptions.IgnoreCase);
            Regex mfRegex = new Regex(@"^mf(\w+)\s+\$(\w+)$", RegexOptions.IgnoreCase);
            Regex jumpRegex = new Regex(@"^jr\s+\$(\w+)$", RegexOptions.IgnoreCase);

            Match rTypeMatches = rTypeRegex.Match(instruction);
            Match multDivMatches = multDivRegex.Match(instruction);
            Match mfMatches = mfRegex.Match(instruction);
            Match jumpMatches = jumpRegex.Match(instruction);

            if (rTypeMatches.Success)
            {
                return new Instruction
                {
                    Category = "Register",
                    Opcode = rTypeMatches.Groups[1].Value,
                    Rd = "$" + rTypeMatches.Groups[2].Value,
                    Rs = "$" + rTypeMatches.Groups[3].Value,
                    Rt = "$" + rTypeMatches.Groups[4].Value
                };
            }
            else if (multDivMatches.Success)
            {
                return new Instruction
                {
                    Category = "MultDiv",
                    Opcode = multDivMatches.Groups[1].Value,
                    Rs = "$" + multDivMatches.Groups[2].Value,
                    Rt = "$" + multDivMatches.Groups[3].Value
                };
            }
            else if (mfMatches.Success)
            {
                return new Instruction
                {
                    Category = "MoveFrom",
                    Opcode = "mf" + mfMatches.Groups[1].Value,
                    Rd = "$" + mfMatches.Groups[2].Value
                };
            }
            else if (jumpMatches.Success)
            {
                return new Instruction
                {
                    Category = "RJump",
                    Opcode = "jr",
                    Rs = "$" + jumpMatches.Groups[1].Value
                };
            }
            else
            {
                return null;
            }
        }

        private static Instruction ParseItype(string instruction)
        {
            Regex itypeRegex = new Regex(@"^(\w+)\s+\$(\w+),\s*\$(\w+),\s*(-?\d+|0x[\da-fA-F]+|0b[01]+|[\w]+)$", RegexOptions.IgnoreCase);
            Regex loadStoreRegex = new Regex(@"^(\w+)\s+\$(\w+),\s*(-?\d+|0x[\da-fA-F]+|0b[01]+)\((\$\w+)\)$", RegexOptions.IgnoreCase);

            Match itypeMatches = itypeRegex.Match(instruction);
            Match loadStoreMatches = loadStoreRegex.Match(instruction);

            if (itypeMatches.Success)
            {
                string opcode = itypeMatches.Groups[1].Value;
                string rt = itypeMatches.Groups[2].Value;
                string rs = itypeMatches.Groups[3].Value;
                string immediateOrLabel = itypeMatches.Groups[4].Value;
                int immediate;
                bool isImmediate = int.TryParse(immediateOrLabel, out immediate);

                if (isImmediate)
                {
                    return new Instruction
                    {
                        Category = "Immediate",
                        Opcode = opcode,
                        Rt = "$" + rt,
                        Rs = "$" + rs,
                        Immediate = immediateOrLabel
                    };
                }
                else
                {
                    return new Instruction
                    {
                        Category = "Branch",
                        Opcode = opcode,
                        Rt = "$" + rt,
                        Rs = "$" + rs,
                        TargetLabel = immediateOrLabel
                    };
                }
            }
            else if (loadStoreMatches.Success)
            {
                string opcode = loadStoreMatches.Groups[1].Value;
                string rt = loadStoreMatches.Groups[2].Value;
                string immediate = loadStoreMatches.Groups[3].Value;
                string rs = loadStoreMatches.Groups[4].Value;
                return new Instruction
                {
                    Category = "LoadStore",
                    Opcode = opcode,
                    Rt = "$" + rt,
                    Rs = rs,
                    Immediate = immediate
                };
            }
            else
            {
                return null;
            }
        }

        private static Instruction ParseJtype(string instruction)
        {
            Regex jTypeLabelRegex = new Regex(@"^(\w+)\s+(\w+)$", RegexOptions.IgnoreCase);
            Regex jTypeNumericRegex = new Regex(@"^(\w+)\s+(\d+|0x[\da-fA-F]+|0b[01]+)$", RegexOptions.IgnoreCase);

            Match jTypeLabelMatches = jTypeLabelRegex.Match(instruction);
            Match jTypeNumericMatches = jTypeNumericRegex.Match(instruction);

            if (jTypeLabelMatches.Success)
            {
                return new Instruction
                {
                    Category = "Jump",
                    Opcode = jTypeLabelMatches.Groups[1].Value,
                    TargetLabel = jTypeLabelMatches.Groups[2].Value
                };
            }
            else if (jTypeNumericMatches.Success)
            {
                return new Instruction
                {
                    Category = "Jump",
                    Opcode = jTypeNumericMatches.Groups[1].Value,
                    Immediate = jTypeNumericMatches.Groups[2].Value
                };
            }
            else
            {
                return null;
            }
        }
    }
}
