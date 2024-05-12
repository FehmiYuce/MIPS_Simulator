using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MIPS_Simulator1
{
	public class Parser
	{
		public static (string category, string opcode, string rd, string rs, string rt, string shamt, string immediate, string target) ParseInstruction(string instruction)
		{
			instruction = instruction.Trim();
			string category = "Unknown";
			string opcode = "";
			string rd = "";
			string rs = "";
			string rt = "";
			string shamt = "";
			string immediate = "";
			string target = "";

			if (Regex.IsMatch(instruction, @"^\w+\s+\$\w+,\s*\$\w+,\s*\$\w+$", RegexOptions.IgnoreCase))
			{
				category = "Register";
				opcode = GetOpcode(instruction);
				string[] operands = GetOperands(instruction);
				rd = operands[0];
				rs = operands[1];
				rt = operands[2];
			}
			else if (Regex.IsMatch(instruction, @"^\w+\s+\$\w+,\s*\$\w+,\s*(-?\d+|0x[\da-fA-F]+|0b[01]+)$", RegexOptions.IgnoreCase))
			{
				category = "Shift";
				opcode = GetOpcode(instruction);
				string[] operands = GetOperands(instruction);
				rd = operands[0];
				rt = operands[1];
				shamt = operands[2];
			}
			else if (Regex.IsMatch(instruction, @"^\w+\s+\$\w+,\s*\$\w+$", RegexOptions.IgnoreCase))
			{
				category = "MultDiv";
				opcode = GetOpcode(instruction);
				string[] operands = GetOperands(instruction);
				rs = operands[0];
				rt = operands[1];
			}
			else if (Regex.IsMatch(instruction, @"^mf\w+\s+\$\w+$", RegexOptions.IgnoreCase))
			{
				category = "MoveFrom";
				opcode = "mf" + GetOpcode(instruction);
				string[] operands = GetOperands(instruction);
				rd = operands[0];
			}
			else if (Regex.IsMatch(instruction, @"^jr\s+\$\w+$", RegexOptions.IgnoreCase))
			{
				category = "RJump";
				opcode = "jr";
				string[] operands = GetOperands(instruction);
				rs = operands[0];
			}
			else if (Regex.IsMatch(instruction, @"^\w+\s+\$\w+,\s*(-?\d+|0x[\da-fA-F]+|0b[01]+)$", RegexOptions.IgnoreCase))
			{
				category = "Immediate";
				opcode = GetOpcode(instruction);
				string[] operands = GetOperands(instruction);
				rt = operands[0];
				rs = operands[1];
				immediate = operands[2];
			}
			else if (Regex.IsMatch(instruction, @"^\w+\s+\$\w+,\s*(-?\d+|0x[\da-fA-F]+|0b[01]+),\s*\(\$\w+\)$", RegexOptions.IgnoreCase))
			{
				category = "LoadStore";
				opcode = GetOpcode(instruction);
				string[] operands = GetOperands(instruction);
				rt = operands[0];
				immediate = operands[1];
				rs = operands[2];
			}
			else if (Regex.IsMatch(instruction, @"^lui\s+\$\w+,\s*(-?\d+|0x[\da-fA-F]+|0b[01]+)$", RegexOptions.IgnoreCase))
			{
				category = "LoadUpperImmediate";
				opcode = "lui";
				string[] operands = GetOperands(instruction);
				rt = operands[0];
				immediate = operands[1];
			}
			else if (Regex.IsMatch(instruction, @"^\w+\s+(\d+|0x[\da-fA-F]+|0b[01]+)$", RegexOptions.IgnoreCase))
			{
				category = "Jump";
				opcode = GetOpcode(instruction);
				string[] operands = GetOperands(instruction);
				target = operands[0];
			}

			return (category, opcode, rd, rs, rt, shamt, immediate, target);
		}

		private static string GetOpcode(string instruction)
		{
			return instruction.Split()[0];
		}

		private static string[] GetOperands(string instruction)
		{
			string[] parts = instruction.Split(',');
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i] = parts[i].Trim();
			}
			return parts[1..];
		}
	}
}
