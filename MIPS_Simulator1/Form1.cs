using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace MIPS_Simulator1
{
    public partial class Form1 : Form
    {
        private MIPS mips;
        private int currentRowIndex = -1;
        private int currentLineIndex = 0;
        private string[] oldRegisterValues;

        public Form1()
        {
            InitializeComponent();
            mips = new MIPS();
            InitializeDMTable();
            InitializeIMTable();
            InitializeRegisterTable();
            
        }

        private void button1_Click(object sender, EventArgs e) // Load button
        {
            string[] assemblyCodeArray = richTextBox1.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            List<string> assemblyCode = assemblyCodeArray.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

            List<string> hexMachineCode = Compiler.CompileToHex(assemblyCode);
            List<string> binMachineCode = Compiler.CompileToBin(assemblyCode);

            int[] binMachineCodeInts = binMachineCode.Select(bin => unchecked((int)(uint)Convert.ToUInt32(bin, 2))).ToArray();

            mips.SetIM(assemblyCode.ToArray(), binMachineCodeInts);

            UpdateInstructionMemory(hexMachineCode, assemblyCode);
            UpdateDataMemoryTable(mips.DMToHex());
            UpdateRegisterTable();
        }

        private void button2_Click(object sender, EventArgs e) // Step button
        {
            oldRegisterValues = mips.RegToHex().ToArray();
            mips.Step();
            UpdateRegisterTable();
            UpdateDataMemoryTable(mips.DMToHex());
            ClearRowHighlight(currentLineIndex - 1);
            HighlightCurrentLine();
            HighlightChangedCellsInColumn3();
        }

        private void HighlightChangedCellsInColumn3()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string cellValue = row.Cells[2].Value?.ToString();
                string previousValue = row.Cells[2].Tag?.ToString();

                if (cellValue != previousValue)
                {
                    row.Cells[2].Style.BackColor = Color.LightPink;
                    row.Cells[2].Tag = cellValue;
                }
                else
                {
                    row.Cells[2].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                }
            }
        }

        private void HighlightCurrentLine()
        {
            if (currentLineIndex < richTextBox1.Lines.Length)
            {
                richTextBox1.Select(richTextBox1.GetFirstCharIndexFromLine(currentLineIndex), richTextBox1.Lines[currentLineIndex].Length);
                richTextBox1.SelectionBackColor = Color.Aqua;
            }
            else
            {
                currentLineIndex = 0;
            }
            currentLineIndex++;
        }

        private void ClearRowHighlight(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < richTextBox1.Lines.Length)
            {
                richTextBox1.Select(richTextBox1.GetFirstCharIndexFromLine(rowIndex), richTextBox1.Lines[rowIndex].Length);
                richTextBox1.SelectionBackColor = richTextBox1.BackColor;
            }
        }

        private void UpdateInstructionMemory(List<string> hexMachineCode, List<string> assemblyCode)
        {
            dataGridView2.Rows.Clear();

            for (int i = 0; i < hexMachineCode.Count; i++)
            {
                string address = "0x" + (i * 2).ToString("X4");
                string machineCode = hexMachineCode[i];
                string source = assemblyCode[i];

                dataGridView2.Rows.Add(address, "0x" + machineCode, source);
            }
        }

        private void InitializeIMTable()
        {
            dataGridView2.Rows.Clear();

            for (int i = 0; i < 256; i++)
            {
                dataGridView2.Rows.Add(new DataGridViewRow());
                int address = i * 2;
                dataGridView2.Rows[i].Cells[0].Value = "0x" + address.ToString("X4");
                dataGridView2.Rows[i].Cells[1].Value = "0x0000";
                dataGridView2.Rows[i].Cells[2].Value = "";
            }
        }

        private void InitializeDMTable()
        {
            dataGridView3.Rows.Clear();

            for (int i = 0; i < 256; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                dataGridView3.Rows.Add(row);

                int address = i;
                dataGridView3.Rows[i].Cells[0].Value = "0x" + address.ToString("X4");
                for (int j = 1; j <= 4; j++)
                {
                    dataGridView3.Rows[i].Cells[j].Value = "0x0000";
                }
            }
        }

        private void UpdateDataMemoryTable(string[] dataMemory)
        {
            for (int i = 0; i < dataMemory.Length; i++)
            {
                string value = dataMemory[i];
                dataGridView3.Rows[i].Cells[1].Value = value;
            }
        }

        private void InitializeRegisterTable()
        {
            dataGridView1.Rows.Clear();

            List<(string Name, string Number)> registerInfo = new List<(string, string)>
            {
                ("$zero", "000"), ("$t0", "001"), ("$t1", "010"), ("$t2", "011"),
                ("$s0", "100"), ("$s1", "101"), ("$s2", "110"), ("$ra", "111"),
                ("pc", "pc"), ("hi", "hi"), ("lo", "lo")
            };

            foreach (var reg in registerInfo)
            {
                dataGridView1.Rows.Add(reg.Name, reg.Number, "0x0000", 0);
            }
        }

        private void UpdateRegisterTable()
        {
            string[] registerValues = mips.RegToHex();
            string pcValue = mips.PCToHex();
            string hiValue = mips.HiToHex();
            string loValue = mips.LoToHex();

            registerValues = registerValues.Concat(new string[] { pcValue, hiValue, loValue }).ToArray();

            for (int i = 0; i < registerValues.Length; i++)
            {
                string value = registerValues[i];
                int decimalValue = Convert.ToInt32(value, 16);

                dataGridView1.Rows[i].Cells[2].Value = value;
                dataGridView1.Rows[i].Cells[3].Value = decimalValue;
            }
        }

        private void ClearChangedCellHighlights()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[2].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                row.Cells[2].Tag = row.Cells[2].Value;
            }
        }

        private void button3_Click(object sender, EventArgs e) // Reset button
        {
            richTextBox1.Text = "";
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();

            currentRowIndex = -1;
            currentLineIndex = 0;

            mips.Reset();
            InitializeRegisterTable();
            InitializeIMTable();
            InitializeDMTable();

            ClearChangedCellHighlights();
        }

        private void button4_Click(object sender, EventArgs e) // Run button
        {
            mips.RunUntilEnd();
            UpdateRegisterTable();
            UpdateDataMemoryTable(mips.DMToHex());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        // Event handlers (empty)
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
    }
}
