using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MIPS_Simulator1
{
    public partial class Form1 : Form
    {
        private MIPS mips;
        private Dictionary<string, string> registers;

        public Form1()
        {
            InitializeComponent();
            mips = new MIPS();
            registers = Registers.RegisterMap;

            InitializeDMTable();
            InitializeIMTable();
        }

        private void button1_Click(object sender, EventArgs e) // Run button
        {
            string[] assemblyCodeArray = textBox1.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            List<string> assemblyCode = assemblyCodeArray.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

            List<string> hexMachineCode = Compiler.CompileToHex(assemblyCode);
            List<string> binMachineCode = Compiler.CompileToBin(assemblyCode);

            // Convert the binary machine code to an int array
            int[] binMachineCodeInts = binMachineCode.Select(bin => Convert.ToInt32(bin, 2)).ToArray();

            mips.SetIM(assemblyCode.ToArray(), binMachineCodeInts);
            mips.RunUntilEnd();

            UpdateInstructionMemory(hexMachineCode, assemblyCode);
            UpdateDataMemoryTable(mips.DMToHex());
            UpdateRegistersTable();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateRegistersTable();
        }

        private void UpdateRegistersTable()
        {
            string[] registerValues = mips.RegToHex();
            string pcValue = mips.PCToHex().ToString();
            string hiValue = mips.HiToHex().ToString();
            string loValue = mips.LoToHex().ToString();

            // Deðerleri registerValues dizisine ekleyin
            registerValues = registerValues.Concat(new string[] { pcValue, hiValue, loValue }).ToArray();

            dataGridView1.Rows.Clear();

            List<(string Name, int Number)> registerInfo = new List<(string, int)>
        {
            ("$zero", 0), ("$at", 1), ("$v0", 2), ("$v1", 3), ("$a0", 4), ("$a1", 5), ("$a2", 6), ("$a3", 7),
            ("$t0", 8), ("$t1", 9), ("$t2", 10), ("$t3", 11), ("$t4", 12), ("$t5", 13), ("$t6", 14), ("$t7", 15),
            ("$s0", 16), ("$s1", 17), ("$s2", 18), ("$s3", 19), ("$s4", 20), ("$s5", 21), ("$s6", 22), ("$s7", 23),
            ("$t8", 24), ("$t9", 25), ("$k0", 26), ("$k1", 27), ("$gp", 28), ("$sp", 29), ("$fp", 30), ("$ra", 31),
            ("pc", -1), ("hi", -1), ("lo", -1)
        };

            for (int i = 0; i < registerValues.Length; i++)
            {
                string name = registerInfo[i].Name;
                int number = registerInfo[i].Number;
                string value = registerValues[i];

                dataGridView1.Rows.Add(name, number >= 0 ? number.ToString() : "", value);
            }
        }


        private void UpdateInstructionMemory(List<string> hexMachineCode, List<string> assemblyCode) 
        {
            dataGridView2.Rows.Clear();

            for (int i = 0; i < hexMachineCode.Count; i++)
            {
                string address = "0x" + (i * 4).ToString("X8");
                string machineCode = hexMachineCode[i];
                string source = assemblyCode[i];

                dataGridView2.Rows.Add(address, machineCode, source);
            }
        }

        private void InitializeIMTable()
        {
            dataGridView2.Rows.Clear();

            for (int i = 0; i < 256; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                dataGridView2.Rows.Add(row);

                int address = i * 4;
                dataGridView2.Rows[i].Cells[0].Value = "0x" + address.ToString("X8");
                dataGridView2.Rows[i].Cells[1].Value = "0x00000000";
                dataGridView2.Rows[i].Cells[2].Value = "";
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void InitializeDMTable()
        {
            dataGridView3.Rows.Clear();

            for (int i = 0; i < 64; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                dataGridView3.Rows.Add(row);

                int decimalAddress = i * 16;
                dataGridView3.Rows[i].Cells[0].Value = "0x" + decimalAddress.ToString("X8");

                for (int j = 0; j < 4; j++)
                {
                    int decimalId = (i * 16 + j * 4) / 4;
                    dataGridView3.Rows[i].Cells[j + 1].Value = "0x00000000";
                }
            }
        }

        private void UpdateDataMemoryTable(string[] dataMemory)
        {
            dataGridView3.Rows.Clear();

            for (int i = 0; i < dataMemory.Length; i++)
            {
                string address = "0x" + (i * 4).ToString("X8");
                string value = dataMemory[i];

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView3);
                row.Cells[0].Value = address;
                row.Cells[1].Value = value;
                dataGridView3.Rows.Add(row);
            }
        }

    }
}
