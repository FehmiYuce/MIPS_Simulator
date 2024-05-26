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
            instr = Convert.ToString(IM[pc / 4], 2).PadLeft(32, '0');
            instr_asm = IM_asm[pc / 4];
            pc += 4;
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
            while (pc < (IM_len * 4))
            {
                Step();
            }
        }

        public void ParseMachineCode()
        {
            opcode = instr.Substring(0, 6);
            rs = Convert.ToInt32(instr.Substring(6, 5), 2);
            rt = Convert.ToInt32(instr.Substring(11, 5), 2);
            rd = Convert.ToInt32(instr.Substring(16, 5), 2);
            shamt = Convert.ToInt32(instr.Substring(21, 5));
            funct = instr.Substring(26, 6);

            var parts = Parser.ParseInstruction(instr_asm);

            // Access the immediate field correctly based on the expected property name
            if (parts.Category == "Immediate" || parts.Category == "LoadStore" || parts.Category == "LoadUpperImmediate")
            {
                imm = signedInt(Convert.ToInt32(parts.Immediate));
            }

            target = Convert.ToInt32(instr.Substring(6, 26), 2);
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
                case "000000": // R-type instruction
                    switch (funct)
                    {
                        case "100000":
                            Add();
                            break;
                        case "100010":
                            Sub();
                            break;
                        case "100100":
                            And();
                            break;
                        case "100101":
                            Or();
                            break;
                        case "100110":
                            Xor();
                            break;
                        case "101010":
                            Slt();
                            break;
                        case "001000":
                            Jr();
                            break;
                        case "000000":
                            Sll();
                            break;
                        case "000010":
                            Srl();
                            break;
                        case "000011":
                            Sra();
                            break;
                        case "010000":
                            Mfhi();
                            break;
                        case "010010":
                            Mflo();
                            break;
                        case "011000":
                            Mult();
                            break;
                        case "011010":
                            Div();
                            break;
                        default:
                            throw new Exception($"Unsupported function code: {funct}");
                    }
                    break;
                case "000100":
                    Beq();
                    break;
                case "000101":
                    Bne();
                    break;
                case "001000":
                    Addi();
                    break;
                case "001010":
                    Slti();
                    break;
                case "001100":
                    Andi();
                    break;
                case "001101":
                    Ori();
                    break;
                case "001111":
                    Lui();
                    break;
                case "100011":
                    Lw();
                    break;
                case "100000":
                    Lb();
                    break;
                case "101011":
                    Sw();
                    break;
                case "101000":
                    Sb();
                    break;
                case "111000":
                    Muli();
                    break;
                case "000010":
                    J();
                    break;
                case "000011":
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
            long product = (long)reg[rs] * (long)reg[rt];
            string binary = Convert.ToString(product, 2).PadLeft(64, '0');
            lo = Convert.ToInt32(binary.Substring(32, 32), 2);
            hi = Convert.ToInt32(binary.Substring(0, 32), 2);
        }

        public void Div()
        {
            int dividend = reg[rs];
            int divisor = reg[rt];

            if (divisor == 0)
            {
                throw new DivideByZeroException("Division by zero");
            }

            int quotient = dividend / divisor;
            int remainder = dividend % divisor;

            lo = quotient;
            hi = remainder;
        }

        //public void Mult()
        //{
        //    long product = (long)reg[rs] * (long)reg[rt];
        //    lo = (int)(product & 0xFFFFFFFF); // Lower 32 bits
        //    hi = (int)(product >> 32); // Upper 32 bits
        //}

        //public void Div()
        //{
        //    int dividend = reg[rs];
        //    int divisor = reg[rt];

        //    if (divisor == 0)
        //    {
        //        throw new DivideByZeroException("Division by zero");
        //    }

        //    lo = dividend / divisor; // Quotient
        //    hi = dividend % divisor; // Remainder
        //}


        public void Beq()
        {
            if (reg[rs] == reg[rt])
            {
                pc += imm << 2; // Address calculation correction
            }
        }

        public void Bne()
        {
            if (reg[rs] != reg[rt])
            {
                pc += imm << 2; // Address calculation correction
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
            reg[rt] = DM[(reg[rs] + imm) / 4];
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
            DM[(reg[rs] + imm) / 4] = reg[rt];
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

        //output functions
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

        //bu kodda da 
        public int ParseInt32(string inputStr, int radix)
        {
            return signedInt(Convert.ToInt32(inputStr, radix));
        }

        //bu kodda hata olabilir
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