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
        private int stepCount;

        // Registers dizisine erişim için 
        public int[] Registers
        {
            get { return reg; }
            set { reg = value; }
        }

        public int StepCount
        {
            get { return stepCount; }
            set { stepCount = value; }
        }

        // DataMemory 
        public int[] DataMemory
        {
            get { return DM; }
            set { DM = value; }
        }

        public MIPS()
        {
            reg = new int[8];  // 8 general-purpose registers
            Array.Fill(reg, 0);
            IM = new int[256];
            IM_asm = new string[256];
            IM_len = 0;
            DM = new int[256];
            Array.Fill(DM, 0);
            pc = 0;
            hi = 0;
            lo = 0;
            instr = string.Empty;
            instr_asm = string.Empty;
            opcode = string.Empty;
            rs = 0;
            rt = 0;
            rd = 0;
            shamt = 0;
            funct = string.Empty;
            imm = 0;
            target = 0;
            stepCount = 0;
        }

        public void SetIM(string[] assemblyCode, int[] binMachineCode)
        {
            IM_len = binMachineCode.Length;
            Array.Copy(assemblyCode, IM_asm, assemblyCode.Length);
            Array.Copy(binMachineCode, IM, binMachineCode.Length);
        }

        public void Fetch()
        {
            instr = Convert.ToString(IM[pc / 2], 2).PadLeft(16, '0');  // Fetch 16-bit instruction
            instr_asm = IM_asm[pc / 2];
            pc += 2;  // Increment program counter by 2
        }

        public void Step()
        {
            Fetch();
            if (instr != null && instr_asm != null)
            {
                ParseMachineCode();
                Execute();
                stepCount++; // Her adımda sayaç arttırılıyor.
            }
        }

        public void RunUntilEnd()
        {
            while (pc < (IM_len * 2))
            {
                Step();
            }
        }

        public void ParseMachineCode()
        {
            opcode = instr.Substring(0, 4);  // 4-bit opcode
            rs = Convert.ToInt32(instr.Substring(4, 3), 2);  // 3-bit rs
            rt = Convert.ToInt32(instr.Substring(7, 3), 2);  // 3-bit rt
            rd = Convert.ToInt32(instr.Substring(10, 3), 2);  // 3-bit rd
            funct = instr.Substring(13, 3);  // 3-bit function

            var parts = Parser.ParseInstruction(instr_asm);

            // Access the immediate field correctly based on the expected property name
            if (parts.Category == "Immediate" || parts.Category == "LoadStore" || parts.Category == "LoadUpperImmediate")
            {
                imm = signedInt(Convert.ToInt32(parts.Immediate));
            }

            target = Convert.ToInt32(instr.Substring(4, 12), 2);  // 12-bit target for J-type
        }

        public void Reset()
        {
            // Registerları sıfırla
            Array.Fill(reg, 0);

            // Instruction Memory'yi sıfırla
            Array.Fill(IM, 0);
            Array.Fill(IM_asm, "");

            // Data Memory'yi sıfırla
            Array.Fill(DM, 0);

            // Diğer değişkenleri sıfırla
            pc = 0;
            hi = 0;
            lo = 0;
            instr = string.Empty;
            instr_asm = string.Empty;
            opcode = string.Empty;
            rs = 0;
            rt = 0;
            rd = 0;
            shamt = 0;
            funct = string.Empty;
            imm = 0;
            target = 0;
            stepCount = 0;
        }

        public void Execute()
        {
            switch (opcode)
            {
                case "0000": // R-type instruction
                    switch (funct)
                    {
                        case "000":
                            Add();
                            break;
                        case "001":
                            Sub();
                            break;
                        case "010":
                            And();
                            break;
                        case "011":
                            Or();
                            break;
                        case "100":
                            Xor();
                            break;
                        case "101":
                            Slt();
                            break;
                        case "110":
                            Jr();
                            break;
                        case "111":
                            Mfhi();
                            break;
                        case "1101":
                            Mflo();
                            break;
                        default:
                            throw new Exception($"Unsupported function code: {funct}");
                    }
                    break;
                case "0100":
                    Beq();
                    break;
                case "0101":
                    Bne();
                    break;
                case "0110":
                    Addi();
                    break;
                case "0111":
                    Slti();
                    break;
                case "1000":
                    Andi();
                    break;
                case "1001":
                    Ori();
                    break;
                case "1010":
                    Lui();
                    break;
                case "1011":
                    Lw();
                    break;
                case "1100":
                    Sw();
                    break;
                case "1101":
                    Muli();
                    break;
                case "1110":
                    J();
                    break;
                case "1111":
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

        public void Mfhi()
        {
            reg[rd] = hi;
        }

        public void Mflo()
        {
            reg[rd] = lo;
        }

        public void Beq()
        {
            if (reg[rs] == reg[rt])
            {
                pc += imm << 1; // Address calculation correction for 16-bit instructions
            }
        }

        public void Bne()
        {
            if (reg[rs] != reg[rt])
            {
                pc += imm << 1; // Address calculation correction for 16-bit instructions
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
            reg[rt] = imm << 8; // Shift left to place immediate in the upper 8 bits
        }

        public void Lw()
        {
            reg[rt] = DM[(reg[rs] + imm)];
        }

        public void Sw()
        {
            DM[(reg[rs] + imm)] = reg[rt];
        }

        public void Muli()
        {
            reg[rt] = reg[rs] * imm;
        }

        public void J()
        {
            pc = target;
        }

        public void Jal()
        {
            reg[7] = pc;  // Save return address in $ra
            pc = target;
        }

        // Output functions
        public string[] RegToHex()
        {
            List<string> hexArray = new List<string>();
            for (int i = 0; i < reg.Length; i++)
            {
                string hexString = "0x" + ToHexString(reg[i], 2);
                hexArray.Add(hexString);
            }
            return hexArray.ToArray();
        }

        public string[] DMToHex()
        {
            List<string> hexArray = new List<string>();
            for (int i = 0; i < DM.Length; i++)
            {
                string hexString = "0x" + ToHexString(DM[i], 2);
                hexArray.Add(hexString);
            }
            return hexArray.ToArray();
        }

        public string PCToHex()
        {
            return "0x" + ToHexString(pc, 2);
        }

        public string HiToHex()
        {
            return "0x" + ToHexString(hi, 2);
        }

        public string LoToHex()
        {
            return "0x" + ToHexString(lo, 2);
        }

        // Conversion functions
        public int ParseInt32(string inputStr, int radix)
        {
            return signedInt(Convert.ToInt32(inputStr, radix));
        }

        private int signedInt(int unsigned)
        {
            byte[] uintBytes = BitConverter.GetBytes(unsigned);
            int signed = BitConverter.ToInt32(uintBytes, 0);
            return signed;
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
            string binaryStr = ToBinString(num, hexLen * 4);
            string hexStr = Convert.ToInt32(binaryStr, 2).ToString("X");
            return hexStr.PadLeft(hexLen, '0');
        }

        public string ToBinString(int num, int binLen)
        {
            string binaryStr = Math.Abs(num).ToString("B");
            binaryStr = binaryStr.PadLeft(binLen, '0');
            if (num < 0)
            {
                binaryStr = TwosComplement(binaryStr, binLen);
            }
            return binaryStr;
        }

        public string TwosComplement(string binaryStr, int length)
        {
            string paddedStr = binaryStr.PadLeft(length, '0');
            string invertedStr = new string(paddedStr.Select(bit => bit == '0' ? '1' : '0').ToArray());
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
            return result.ToString().PadLeft(length, '0');
        }
    }
}
