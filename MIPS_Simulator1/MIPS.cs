using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIPS_Simulator1
{
	public class MIPS
	{
		private int[] reg;
		private int[] IM;
		private string[] IM_asm;
		private int IM_len;
		private int[] DM;
		private int pc;
		private int hi;
		private int lo;
		private string instr;
		private string instr_asm;
		private string opcode;
		private int rs;
		private int rt;
		private int rd;
		private int shamt;
		private string funct;
		private int imm;
		private int target;

		public MIPS()
		{
			reg = new int[32];
			Array.Fill(reg, 0);
			IM = new int[256];
			IM_asm = new string[256];
			IM_len = 0;
			DM = new int[256];
			Array.Fill(DM, 0);
			pc = 0;
			hi = 0;
			lo = 0;
			instr = null;
			instr_asm = null;
			opcode = null;
			rs = 0;
			rt = 0;
			rd = 0;
			shamt = 0;
			funct = null;
			imm = 0;
			target = 0;
		}

		public void SetIM(string[] assemblyCode, int[] binMachineCode)
		{
			IM_len = binMachineCode.Length;
			Array.Copy(assemblyCode, IM_asm, assemblyCode.Length);
			Array.Copy(binMachineCode, IM, binMachineCode.Length);
		}

		public void Fetch()
		{
			instr = Convert.ToString(IM[pc / 4], 2).PadLeft(32, '0');
			instr_asm = IM_asm[pc / 4];
			pc += 4;
		}

		public void Step()
		{
			Fetch();
			if (instr != null)
			{
				ParseMachineCode();
				Execute();
			}
		}

		public void RunUntilEnd()
		{
			while (pc < (IM_len * 4))
			{
				Step();
			}
		}

		public void ParseMachineCode()
		{
			var instructionTuple = Parser.ParseInstruction(instr_asm);
			opcode = instructionTuple.opcode;
			rs = Convert.ToInt32(instructionTuple.rs, 2);
			rt = Convert.ToInt32(instructionTuple.rt, 2);
			rd = Convert.ToInt32(instructionTuple.rd, 2);
			shamt = Convert.ToInt32(instructionTuple.shamt, 2);
			funct = instructionTuple.opcode;
			imm = Convert.ToInt32(instructionTuple.immediate, 2);
			target = Convert.ToInt32(instructionTuple.target, 2);
		}



		public void Execute()
		{
			switch (opcode)
			{
				case "000000": // R-type instruction
					switch (funct)
					{
						case "100000": // ADD
							Add();
							break;
						case "100010": // SUB
							Sub();
							break;
						case "100100": // AND
							And();
							break;
						case "100101": // OR
							Or();
							break;
						case "100110": // XOR
							Xor();
							break;
						case "101010": // SLT
							Slt();
							break;
						case "001000": // JR
							Jr();
							break;
						case "000000": // SLL
							Sll();
							break;
						case "000010": // SRL
							Srl();
							break;
						case "000011": // SRA
							Sra();
							break;
						case "010000": // MFHI
							Mfhi();
							break;
						case "010010": // MFLO
							Mflo();
							break;
						case "011000": // MULT
							Mult();
							break;
						case "011010": // DIV
							Div();
							break;
						default:
							throw new Exception($"Unsupported function code: {funct}");
					}
					break;
				case "000100": // BEQ
					Beq();
					break;
				case "000101": // BNE
					Bne();
					break;
				case "001000": // ADDI
					Addi();
					break;
				case "001010": // SLTI
					Slti();
					break;
				case "001100": // ANDI
					Andi();
					break;
				case "001101": // ORI
					Ori();
					break;
				case "001111": // LUI
					Lui();
					break;
				case "100011": // LW
					Lw();
					break;
				case "100000": // LB
					Lb();
					break;
				case "101011": // SW
					Sw();
					break;
				case "101000": // SB
					Sb();
					break;
				case "111000": // MULI
					Muli();
					break;
				case "000010": // J
					J();
					break;
				case "000011": // JAL
					Jal();
					break;
				default:
					throw new Exception($"Unsupported opcode: {opcode}");
			}
		}

		public void Add()
		{
			reg[rd] = reg[rs] + reg[rt];
		}

		public void Sub()
		{
			reg[rd] = reg[rs] - reg[rt];
		}

		public void And()
		{
			reg[rd] = reg[rs] & reg[rt];
		}

		public void Or()
		{
			reg[rd] = reg[rs] | reg[rt];
		}

		public void Xor()
		{
			reg[rd] = reg[rs] ^ reg[rt];
		}

		public void Slt()
		{
			reg[rd] = reg[rs] < reg[rt] ? 1 : 0;
		}

		public void Jr()
		{
			pc = reg[rs];
		}

		public void Sll()
		{
			reg[rd] = reg[rt] << shamt;
		}

		public void Srl()
		{
			reg[rd] = (int)((uint)reg[rt] >> shamt);
		}

		public void Sra()
		{
			reg[rd] = reg[rt] >> shamt;
		}

		public void Mfhi()
		{
			reg[rd] = hi;
		}

		public void Mflo()
		{
			reg[rd] = lo;
		}

		public void Mult()
		{
			long product = (long)reg[rs] * reg[rt];
			hi = (int)(product >> 32);
			lo = (int)(product & 0xFFFFFFFF);
		}

		public void Div()
		{
			if (reg[rt] == 0)
			{
				throw new Exception("Division by zero");
			}

			lo = reg[rs] / reg[rt];
			hi = reg[rs] % reg[rt];
		}


		public void Beq()
		{
			if (reg[rs] == reg[rt])
			{
				pc = imm;
			}
		}

		public void Bne()
		{
			if (reg[rs] != reg[rt])
			{
				pc = imm;
			}
		}

		public void Addi()
		{
			reg[rt] = reg[rs] + imm;
		}

		public void Slti()
		{
			reg[rt] = reg[rs] < imm ? 1 : 0;
		}

		public void Andi()
		{
			reg[rt] = reg[rs] & imm;
		}

		public void Ori()
		{
			reg[rt] = reg[rs] | imm;
		}

		public void Lui()
		{
			reg[rt] = imm << 16;
		}

		public void Lw()
		{
			reg[rt] = DM[reg[rs] + imm];
		}

		public void Lb()
		{
			int address = reg[rs] + imm;
			int wordAddr = address - (address % 4);
			int word = DM[wordAddr / 4];
			int start = 2 * (4 - (address % 4)) - 2;
			int end = 2 * (4 - (address % 4));
			int shift = (3 - (address % 4)) * 8;
			int mask = 0xFF << shift;
			int byteValue = (word & mask) >> shift;
			reg[rt] = (byteValue << 24) >> 24; // Sign extend to 32 bits
		}

		public void Sw()
		{
			DM[reg[rs] + imm] = reg[rt];
		}

		public void Sb()
		{
			int address = reg[rs] + imm;
			int wordAddr = address - (address % 4);
			int word = DM[wordAddr / 4];
			int start = 2 * (4 - (address % 4)) - 2;
			int end = 2 * (4 - (address % 4));
			int byteShift = (3 - (address % 4)) * 8;
			int mask = 0xFF << byteShift;
			int newData = (reg[rt] & 0xFF) << byteShift;
			DM[wordAddr / 4] = (word & ~mask) | newData;
		}

		public void Muli()
		{
			reg[rt] = reg[rs] * imm;
		}

		public void J()
		{
			pc = (int)((pc & 0xF0000000) | (target << 2));
		}

		public void Jal()
		{
			reg[31] = pc;
			pc = target;
		}

		private int signedInt(int unsigned)
		{
			byte[] uintBytes = BitConverter.GetBytes(unsigned);
			int signed = BitConverter.ToInt32(uintBytes, 0);
			return signed;
		}
		public string[] RegToHex()
		{
			List<string> hexArray = new List<string>();
			for (int i = 0; i < reg.Length; i++)
			{
				string hexString = "0x" + ToHexString(reg[i], 8);
				hexArray.Add(hexString);
			}
			return hexArray.ToArray();
		}

		public string[] DMToHex()
		{
			List<string> hexArray = new List<string>();
			for (int i = 0; i < DM.Length; i++)
			{
				string hexString = "0x" + ToHexString(DM[i], 8);
				hexArray.Add(hexString);
			}
			return hexArray.ToArray();
		}

		public string PCToHex()
		{
			return "0x" + ToHexString(pc, 8);
		}

		public string HiToHex()
		{
			return "0x" + ToHexString(hi, 8);
		}

		public string LoToHex()
		{
			return "0x" + ToHexString(lo, 8);
		}

		public int ParseInt32(string inputStr, int radix)
		{
			return signedInt(Convert.ToInt32(inputStr, radix));
		}

		public string SignExtend(string inputStr, int initialLen, int finalLen)
		{
			string outputStr = inputStr;
			char signBit = inputStr[0];
			string signExtension = new string(signBit, finalLen - initialLen);
			if (initialLen < finalLen)
			{
				outputStr = signExtension + inputStr;
			}
			else if (initialLen > finalLen)
			{
				outputStr = inputStr.Substring(initialLen - finalLen);
			}
			return outputStr;
		}

		public string ToHexString(int num, int hexLen)
		{
			// Get the binary string representation of the number in two's complement form
			string binaryStr = ToBinString(num, hexLen * 4);

			// Convert the binary string to a hexadecimal string
			string hexStr = Convert.ToInt32(binaryStr, 2).ToString("X");

			// Pad the hexadecimal string with zeros to the desired length
			return hexStr.PadLeft(hexLen, '0');
		}

		public string ToBinString(int num, int binLen)
		{
			// Convert num to binary string
			string binaryStr = Math.Abs(num).ToString("B");

			// If binaryStr is shorter than binLen, pad with zeros to the left
			binaryStr = binaryStr.PadLeft(binLen, '0');

			// If num is negative, take the two's complement
			if (num < 0)
			{
				binaryStr = TwosComplement(binaryStr, binLen);
			}

			// Return binary string
			return binaryStr;
		}

		public string TwosComplement(string binaryStr, int length)
		{
			// Pad the binary string with zeros on the left to the given length
			string paddedStr = binaryStr.PadLeft(length, '0');

			// Invert all bits
			string invertedStr = new string(paddedStr.Select(bit => bit == '0' ? '1' : '0').ToArray());

			// Add 1 to the inverted value
			int carry = 1;
			StringBuilder result = new StringBuilder();
			for (int i = invertedStr.Length - 1; i >= 0; i--)
			{
				int sum = (invertedStr[i] - '0') + carry;
				if (sum == 2)
				{
					result.Insert(0, '0');
					carry = 1;
				}
				else
				{
					result.Insert(0, sum);
					carry = 0;
				}
			}

			// Pad the result with zeros on the left to the given length
			return result.ToString().PadLeft(length, '0');
		}

	}
}
