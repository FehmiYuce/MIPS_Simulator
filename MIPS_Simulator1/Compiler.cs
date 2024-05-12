﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIPS_Simulator1
{
	public class Compiler
	{
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

			return opcodeValue +
				ConvertImmediateToBinary(target, 26);
		}


		private static string ConvertImmediateToBinary(string immediate, int length)
		{
			if (immediate.StartsWith('-'))
			{
				return (Math.Pow(2, length) + int.Parse(immediate)).ToString("B").Substring(1);
			}
			else if (immediate.StartsWith("0x"))
			{
				return Convert.ToInt32(immediate.Substring(2), 16).ToString("B").PadLeft(length, '0');
			}
			else if (immediate.StartsWith("0b"))
			{
				return immediate.Substring(2).PadLeft(length, '0');
			}
			else
			{
				return Convert.ToInt32(immediate).ToString("B").PadLeft(length, '0');
			}
		}

	}
}
