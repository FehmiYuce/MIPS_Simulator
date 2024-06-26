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
        private Dictionary<string, string> registers;
        private int currentRowIndex = -1;
        int currentInstructionIndex = 0;
        private List<DataGridViewCell> changedCells = new List<DataGridViewCell>();
        private int currentLineIndex = 0;
        private string[] oldRegisterValues;

        public Form1()
        {
            InitializeComponent();
            mips = new MIPS();
            registers = Registers.RegisterMap;

            InitializeDMTable();
            InitializeIMTable();
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
            UpdateRegistersTable();
        }

        private void HighlightChangedCellsInColumn3() // Highlight changed cells in column 3
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

        private void button2_Click(object sender, EventArgs e) // Step button
        {
            oldRegisterValues = mips.RegToHex().ToArray();
            mips.Step();
            UpdateRegistersTable();
            UpdateDataMemoryTable(mips.DMToHex());

            ClearRowHighlight(currentLineIndex - 1);
            HighlightCurrentLine();
            UpdateInstructionPointer(currentLineIndex);
            //HighlightChangedRegisterCells(oldRegisterValues);
            HighlightChangedCellsInColumn3();
        }

        private void HighlightChangedRegisterCells(string[] oldValues)
        {
            var newRegisterValues = mips.RegToHex().ToArray();

            for (int i = 0; i < newRegisterValues.Length; i++)
            {
                if (oldValues[i] != newRegisterValues[i])
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[0].Value.ToString() == Registers.RegisterList[i])
                        {
                            row.Cells[2].Style.BackColor = Color.LightPink;
                            changedCells.Add(row.Cells[2]);
                        }
                    }
                }
            }
        }

        public static class Registers
        {
            public static Dictionary<string, string> RegisterMap { get; } = new Dictionary<string, string>
            {
                { "$zero", "000" }, { "$t0", "001" }, { "$t1", "010" }, { "$t2", "011" },
                { "$s0", "100" }, { "$s1", "101" }, { "$s2", "110" }, { "$ra", "111" },
                { "pc", "pc" }, { "hi", "hi" }, { "lo", "lo" }
            };

            public static List<string> RegisterList { get; } = new List<string>
            {
                "$zero", "$t0", "$t1", "$t2", "$s0", "$s1", "$s2", "$ra",
                "pc", "hi", "lo"
            };
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

        private void UpdateInstructionPointer(int currentInstructionIndex)
        {
            if (currentRowIndex != -1 && currentRowIndex < dataGridView2.Rows.Count)
            {
                dataGridView2.Rows[currentRowIndex].DefaultCellStyle.BackColor = dataGridView2.DefaultCellStyle.BackColor;
            }

            if (currentInstructionIndex >= 0 && currentInstructionIndex < dataGridView2.Rows.Count)
            {
                dataGridView2.Rows[currentInstructionIndex].DefaultCellStyle.BackColor = Color.White;
                currentRowIndex = currentInstructionIndex;
            }
        }

        private void InitializeIMTable()
        {
            dataGridView2.Rows.Clear();

            for (int i = 0; i < 256; i++)
            {
                dataGridView2.Rows.Add(new DataGridViewRow());
                int address = i * 4;
                dataGridView2.Rows[i].Cells[0].Value = "0x" + address.ToString("X8");
                dataGridView2.Rows[i].Cells[1].Value = "0x00000000";
                dataGridView2.Rows[i].Cells[2].Value = "";
            }
        }

        private void InitializeDMTable()
        {
            dataGridView3.Rows.Clear();

            for (int i = 0; i < 256; i += 4)
            {
                DataGridViewRow row = new DataGridViewRow();
                dataGridView3.Rows.Add(row);

                int address = i;
                dataGridView3.Rows[i / 4].Cells[0].Value = "0x" + address.ToString("X2");
                for (int j = 1; j <= 4; j++)
                {
                    dataGridView3.Rows[i / 4].Cells[j].Value = "0x00";
                }
            }
        }

        private void UpdateDataMemoryTable(string[] dataMemory)
        {
            for (int i = 0; i < dataMemory.Length; i++)
            {
                string value = dataMemory[i];
                dataGridView3.Rows[i / 4].Cells[i % 4 + 1].Value = value;
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

                dataGridView2.Rows.Add(address, "0x" + machineCode, source);
            }
        }

        private void UpdateRegistersTable()
        {
            string[] registerValues = mips.RegToHex();
            string pcValue = mips.PCToHex();
            string hiValue = mips.HiToHex();
            string loValue = mips.LoToHex();

            registerValues = registerValues.Concat(new string[] { pcValue, hiValue, loValue }).ToArray();
            dataGridView1.Rows.Clear();

            List<(string Name, string Number)> registerInfo = new List<(string, string)>
            {
                ("$zero", "000"), ("$t0", "001"), ("$t1", "010"), ("$t2", "011"),
                ("$s0", "100"), ("$s1", "101"), ("$s2", "110"), ("$ra", "111"),
                ("pc", "pc"), ("hi", "hi"), ("lo", "lo")
            };

            for (int i = 0; i < registerValues.Length; i++)
            {
                string name = registerInfo[i].Name;
                string number = registerInfo[i].Number;
                string value = registerValues[i];
                int decimalValue = Convert.ToInt32(value, 16);

                dataGridView1.Rows.Add(name, number, value, decimalValue);
            }
        }

        private void ClearChangedCellHighlights()
        {
            foreach (var cell in changedCells)
            {
                cell.Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            }
            changedCells.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();

            currentRowIndex = -1;
            currentInstructionIndex = 0;
            currentLineIndex = 0;

            mips.Reset();
            UpdateRegistersTable();
            InitializeIMTable();
            InitializeDMTable();

            ClearChangedCellHighlights();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mips.RunUntilEnd();
            UpdateRegistersTable();
            UpdateDataMemoryTable(mips.DMToHex());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateRegistersTable();
        }

        // Event handlers
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
    }
}
