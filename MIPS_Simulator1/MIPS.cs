using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        public int pc;
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
        //public int[] Registers
        //{
        //    get { return reg; }
        //    set { reg = value; }
        //}

        //public int StepCount
        //{
        //    get { return stepCount; }
        //    set { stepCount = value; }
        //}

        //// DataMemory 
        //public int[] DataMemory
        //{
        //    get { return DM; }
        //    set { DM = value; }
        //}

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
            if (pc / 2 >= IM.Length)
            {
                return; // Ensure not going outside IM boundaries
            }
            instr = Convert.ToString(IM[pc / 2], 2).PadLeft(16, '0');  // Fetch 16-bit instruction
            instr_asm = IM_asm[pc / 2];
        }

        //public void Step()
        //{
        //    Fetch();
        //    if (string.IsNullOrEmpty(instr) || string.IsNullOrEmpty(instr_asm))
        //    {
        //        return; // Exit if no instruction
        //    }
        //    var parts = Parser.ParseInstruction(instr_asm);
        //    if (parts != null && parts.Category != "Label")
        //    {
        //        ParseMachineCode(parts);
        //        Execute();
        //    }
        //    pc += 2;  // Increment program counter if not jump/beq
        //}

        public void Step()
        {
            Fetch();
            if (string.IsNullOrEmpty(instr) || string.IsNullOrEmpty(instr_asm))
            {
                return; // No instruction to process
            }
            var parts = Parser.ParseInstruction(instr_asm);
            if (parts != null)
            {
                if (parts.Category == "Label" && parts.Opcode == "exit")
                {
                    shouldContinue = false; // Set the flag to stop after this cycle
                    return;
                }
                ParseMachineCode(parts);
                Execute();
                if (!(parts.Category == "Jump" || parts.Category == "Branch" || parts.Category == "RJump"))
                {
                    pc += 2; // Only increment PC if not a jump/branch
                }
            }
            else
            {
                pc += 2; // Default increment if instruction parsing fails
            }
        }




        private bool shouldContinue = true;

        public void RunUntilEnd()
        {
            while (pc < (IM_len * 2) && shouldContinue)
            {
                Step();
            }
        }

        public void ParseMachineCode(Instruction parts)
        {
            opcode = instr.Substring(0, 4);  // 4-bit opcode
            rs = Convert.ToInt32(instr.Substring(4, 3), 2);  // 3-bit rs
            rt = Convert.ToInt32(instr.Substring(7, 3), 2);  // 3-bit rt
            rd = Convert.ToInt32(instr.Substring(10, 3), 2);  // 3-bit rd
            funct = instr.Substring(13, 3);  // 3-bit function

            if (parts.Category == "Immediate" || parts.Category == "LoadStore" || parts.Category == "Branch")
            {
                if (parts.Immediate != null)
                {
                    imm = signedInt(Convert.ToInt32(parts.Immediate));
                }
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
                        default:
                            throw new Exception($"Unsupported function code: {funct}");
                    }
                    break;
                case "0001": // jr instruction
                    Jr();
                    break;
                case "0010": // Shift instructions
                    switch (funct)
                    {
                        case "000":
                            Sll();
                            break;
                        case "001":
                            Srl();
                            break;
                        case "010":
                            Sra();
                            break;
                        default:
                            throw new Exception($"Unsupported function code: {funct}");
                    }
                    break;
                case "0011": // mfhi, mflo instructions
                    switch (funct)
                    {
                        case "000":
                            Mfhi();
                            break;
                        case "001":
                            Mflo();
                            break;
                        default:
                            throw new Exception($"Unsupported function code: {funct}");
                    }
                    break;
                case "0100": // mult, div instructions
                    switch (funct)
                    {
                        case "000":
                            Mult();
                            break;
                        case "001":
                            Div();
                            break;
                        default:
                            throw new Exception($"Unsupported function code: {funct}");
                    }
                    break;
                case "0101":
                    Beq();
                    break;
                case "0110": // bne instruction
                    Bne();
                    break;
                case "0111": // addi instruction
                    Addi();
                    break;
                case "1000": // slti instruction
                    Slti();
                    break;
                case "1001": // andi instruction
                    Andi();
                    break;
                case "1010": // ori instruction
                    Ori();
                    break;
                case "1011": // lw instruction
                    Lw();
                    break;
                case "1100": // sw instruction
                    Sw();
                    break;
                case "1101": // muli instruction
                    Muli();
                    break;
                case "1110": // j instruction
                    J();
                    break;
                case "1111": // jal instruction
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
            Console.WriteLine($"SLT: reg[{rd}] = {reg[rd]} (reg[{rs}] = {reg[rs]} < reg[{rt}] = {reg[rt]})");
        }


        public void Jr()
        {
            pc = reg[rs];
        }

        public void Sll()
        {
            MessageBox.Show($"SLT: reg[{rd}] = {reg[rd]} (reg[{rs}] = {reg[rs]} < reg[{rt}] = {reg[rt]})");

            int shiftAmount = reg[rt];
            MessageBox.Show($"Shift Amount: {shiftAmount}, Value Before Shift: {reg[rt]}");
            reg[rd] = reg[rs] << shiftAmount;
            MessageBox.Show($"Value After Shift: {reg[rd]}");
        }


        public void Srl()
        {
            //reg[rd] = (int)((uint)reg[rt] >> (reg[rs] & 0x07)); // Kaydırma miktarını 3 bit ile sınırlandırarak alıyoruz.
            MessageBox.Show($"SLT: reg[{rd}] = {reg[rd]} (reg[{rs}] = {reg[rs]} < reg[{rt}] = {reg[rt]})");
            int shiftAmount = reg[rt];
            MessageBox.Show($"Shift Amount: {shiftAmount}, Value Before Shift: {reg[rt]}");
            reg[rd] = reg[rs] >> shiftAmount;
            MessageBox.Show($"Value After Shift: {reg[rd]}");
        }

        public void Sra()
        {
            reg[rd] = reg[rt] >> (reg[rs] & 0x07); // Kaydırma miktarını 3 bit ile sınırlandırarak alıyoruz.
        }

        //public void Sll()
        //{
        //    int shiftAmount = reg[rs] & 0x1F;  // Mask to use lower 5 bits
        //    reg[rd] = reg[rt] << shiftAmount;
        //    reg[rd] &= 0xFFFF; // Ensure the result is within 16 bits
        //}

        //public void Srl()
        //{
        //    int shiftAmount = reg[rs] & 0x1F;
        //    reg[rd] = (int)((uint)reg[rt] >> shiftAmount) & 0xFFFF;
        //}

        //public void Sra()
        //{
        //    int shiftAmount = reg[rs] & 0x1F;
        //    reg[rd] = (reg[rt] >> shiftAmount) & 0xFFFF;
        //}

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
            long result = (long)reg[rs] * reg[rt];
            lo = (int)(result & 0xFFFFFFFF); // Lower 32 bits
            hi = (int)(result >> 32);        // Upper 32 bits
        }

        public void Div()
        {
            if (reg[rt] != 0)
            {
                lo = reg[rs] / reg[rt];
                hi = reg[rs] % reg[rt];
            }
            else
            {
                throw new DivideByZeroException("Division by zero.");
            }
        }

        public void Beq()
        {
            MessageBox.Show($"rs: {reg[rs]}, rt: {reg[rt]}, imm: {imm}, pc: {pc}");

            if (reg[rs] == reg[rt])
            {
                //MessageBox.Show($"Branch taken: new pc: {pc}");
                pc = (imm << 1); // Address calculation correction for 16-bit instructions
                MessageBox.Show($"Branch taken: new pc: {pc}");

            }
            else//new
            {
                pc = pc + 2;
            }
        }

        public void Bne()
        {
            if (reg[rs] != reg[rt])
            {
                pc = (imm << 1); // Address calculation correction for 16-bit instructions
                MessageBox.Show($"Branch taken: new pc: {pc}");
            }
            else//new
            {
                pc = pc + 2;
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

        //public void Lui()
        //{
        //    reg[rt] = imm << 8; // Shift left to place immediate in the upper 8 bits
        //}

        public void Lw()
        {
            reg[rt] = DM[reg[rs] + imm];
        }

        public void Sw()
        {
            DM[reg[rs] + imm] = reg[rt];
        }

        public void Muli()
        {
            reg[rt] = reg[rs] * imm;
        }

        public void J()
        {
            //MessageBox.Show("Jumping to address: " + (target));
            pc = target + 2; // Correction for 16-bit instruction //+2 new
        }

        public void Jal()
        {
            // Assume Registers.RegisterMap["$ra"] correctly maps to the index for $ra in 'reg' array
            int raIndex = Convert.ToInt32(Registers.RegisterMap["$ra"], 2);  // Convert binary index
            reg[raIndex] = pc + 2;  // Save the return address (address following this instruction)

            // Jump to the target address (this needs to be set during the parsing phase and correctly aligned with the instruction set format)
            pc = target;  // Ensure 'target' is calculated correctly in the parsing phase
        }


        // Output functions
        public string[] RegToHex()
        {
            List<string> hexArray = new List<string>();
            for (int i = 0; i < reg.Length; i++)
            {
                string hexString = "0x" + ToHexString(reg[i], 4);
                hexArray.Add(hexString);
            }
            return hexArray.ToArray();
        }

        public string[] DMToHex()
        {
            List<string> hexArray = new List<string>();
            for (int i = 0; i < DM.Length; i++)
            {
                string hexString = "0x" + ToHexString(DM[i], 4);
                hexArray.Add(hexString);
            }
            return hexArray.ToArray();
        }

        public string PCToHex()
        {
            return "0x" + ToHexString(pc, 4);
        }

        public string HiToHex()
        {
            return "0x" + ToHexString(hi, 4);
        }

        public string LoToHex()
        {
            return "0x" + ToHexString(lo, 4);
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