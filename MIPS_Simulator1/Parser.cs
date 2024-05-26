using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MIPS_Simulator1
{
    public static class Parser
    {
        public static dynamic ParseInstruction(string instruction)
        {
            string[] parts = instruction.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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

        private static dynamic ParseRtype(string instruction)
        {
            Regex rTypeRegex = new Regex(@"^(\w+)\s+\$(\w+),\s*\$(\w+),\s*\$(\w+)$", RegexOptions.IgnoreCase);
            Regex shiftRegex = new Regex(@"^(\w+)\s+\$(\w+),\s*\$(\w+),\s*(\d+|0x[\da-fA-F]+)$", RegexOptions.IgnoreCase);
            Regex multDivRegex = new Regex(@"^(\w+)\s+\$(\w+),\s*\$(\w+)$", RegexOptions.IgnoreCase);
            Regex mfRegex = new Regex(@"^mf(\w+)\s+\$(\w+)$", RegexOptions.IgnoreCase);
            Regex jumpRegex = new Regex(@"^jr\s+\$(\w+)$", RegexOptions.IgnoreCase);
            

            Match rTypeMatches = rTypeRegex.Match(instruction);
            Match shiftMatches = shiftRegex.Match(instruction);
            Match multDivMatches = multDivRegex.Match(instruction);
            Match mfMatches = mfRegex.Match(instruction);
            Match jumpMatches = jumpRegex.Match(instruction);

            if (rTypeMatches.Success)
            {
                string opcode = rTypeMatches.Groups[1].Value;
                string rd = rTypeMatches.Groups[2].Value;
                string rs = rTypeMatches.Groups[3].Value;
                string rt = rTypeMatches.Groups[4].Value;
                return new { Category = "Register", Opcode = opcode, Rd = "$" + rd, Rs = "$" + rs, Rt = "$" + rt };
            }
            else if (shiftMatches.Success)
            {
                string opcode = shiftMatches.Groups[1].Value;
                string rd = shiftMatches.Groups[2].Value;
                string rt = shiftMatches.Groups[3].Value;
                string shamt = shiftMatches.Groups[4].Value;
                return new { Category = "Shift", Opcode = opcode, Rd = "$" + rd, Rt = "$" + rt, Shamt = shamt };
            }
            else if (multDivMatches.Success)
            {
                string opcode = multDivMatches.Groups[1].Value;
                string rs = multDivMatches.Groups[2].Value;
                string rt = multDivMatches.Groups[3].Value;
                return new { Category = "MultDiv", Opcode = opcode, Rs = "$" + rs, Rt = "$" + rt };
            }
            else if (mfMatches.Success)
            {
                string opcode = mfMatches.Groups[1].Value;
                string rd = mfMatches.Groups[2].Value;
                return new { Category = "MoveFrom", Opcode = "mf" + opcode, Rd = "$" + rd };
            }
            else if (jumpMatches.Success)
            {
                string rs = jumpMatches.Groups[1].Value;
                return new { Category = "RJump", Opcode = "jr", Rs = "$" + rs };
            }
            else
            {
                return null;
            }
        }

        private static dynamic ParseItype(string instruction)
        {
            Regex itypeRegex = new Regex(@"^(\w+)\s+\$(\w+),\s*\$(\w+),\s*(-?\d+|0x[\da-fA-F]+|0b[01]+|[\w]+)$", RegexOptions.IgnoreCase);
            Regex loadStoreRegex = new Regex(@"^(\w+)\s+\$(\w+),\s*(-?\d+|0x[\da-fA-F]+|0b[01]+)\((\$\w+)\)$", RegexOptions.IgnoreCase);
            Regex luiRegex = new Regex(@"^lui\s+\$(\w+),\s*(-?\d+|0x[\da-fA-F]+|0b[01]+)$", RegexOptions.IgnoreCase);

            Match itypeMatches = itypeRegex.Match(instruction);
            Match loadStoreMatches = loadStoreRegex.Match(instruction);
            Match luiMatches = luiRegex.Match(instruction);

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
                    return new { Category = "Immediate", Opcode = opcode, Rt = "$" + rt, Rs = "$" + rs, Immediate = immediateOrLabel };
                }
                else
                {
                    return new { Category = "Branch", Opcode = opcode, Rt = "$" + rt, Rs = "$" + rs, TargetLabel = immediateOrLabel };
                }
            }
            else if (loadStoreMatches.Success)
            {
                string opcode = loadStoreMatches.Groups[1].Value;
                string rt = loadStoreMatches.Groups[2].Value;
                string immediate = loadStoreMatches.Groups[3].Value;
                string rs = loadStoreMatches.Groups[4].Value;
                return new { Category = "LoadStore", Opcode = opcode, Rt = "$" + rt, Rs = rs, Immediate = immediate };
            }
            else if (luiMatches.Success)
            {
                string rt = luiMatches.Groups[1].Value;
                string immediate = luiMatches.Groups[2].Value;
                return new { Category = "LoadUpperImmediate", Opcode = "lui", Rt = "$" + rt, Immediate = immediate };
            }
            else
            {
                return null;
            }
        }


        private static dynamic ParseJtype(string instruction)
        {
            // Regex for J-type instruction with label
            Regex jTypeLabelRegex = new Regex(@"^(\w+)\s+(\w+)$", RegexOptions.IgnoreCase);
            // Regex for J-type instruction with numeric target
            Regex jTypeNumericRegex = new Regex(@"^(\w+)\s+(\d+|0x[\da-fA-F]+|0b[01]+)$", RegexOptions.IgnoreCase);

            Match jTypeLabelMatches = jTypeLabelRegex.Match(instruction);
            Match jTypeNumericMatches = jTypeNumericRegex.Match(instruction);

            if (jTypeLabelMatches.Success)
            {
                string opcode = jTypeLabelMatches.Groups[1].Value;
                string targetLabel = jTypeLabelMatches.Groups[2].Value;
                return new { Category = "Jump", Opcode = opcode, TargetLabel = targetLabel };
            }
            else if (jTypeNumericMatches.Success)
            {
                string opcode = jTypeNumericMatches.Groups[1].Value;
                string target = jTypeNumericMatches.Groups[2].Value;
                return new { Category = "Jump", Opcode = opcode, Target = target };
            }
            else
            {
                return null;
            }
        }

    }
}
