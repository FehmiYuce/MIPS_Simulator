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
        private int currentRowIndex = -1; // Baþlangýçta iþaretçi yok
        int currentInstructionIndex = 0; // Ýþlem sýrasýnýn baþlangýçta 0 olduðunu varsayalým

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
            string[] assemblyCodeArray = textBox1.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            List<string> assemblyCode = assemblyCodeArray.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

            // Etiketlerin (labels) iþlenmesi
            // assemblyCode = ProcessLabels(assemblyCode);

            List<string> hexMachineCode = Compiler.CompileToHex(assemblyCode);
            List<string> binMachineCode = Compiler.CompileToBin(assemblyCode);

            // Convert the binary machine code to an int array
            int[] binMachineCodeInts = binMachineCode.Select(bin =>
            {
                // Parse binary string to ulong first
                ulong parsedValue = Convert.ToUInt64(bin, 2);
                // Cast to uint and then to int with unchecked context
                return unchecked((int)(uint)parsedValue);
            }).ToArray();

            mips.SetIM(assemblyCode.ToArray(), binMachineCodeInts);

            UpdateInstructionMemory(hexMachineCode, assemblyCode);
            UpdateDataMemoryTable(mips.DMToHex());
            UpdateRegistersTable();
        }


        private void button2_Click(object sender, EventArgs e) // Step button
        {

            mips.Step();
            currentInstructionIndex++; // Bir sonraki iþlem sýrasýný belirle
            UpdateRegistersTable();
            UpdateDataMemoryTable(mips.DMToHex());
            //HighlightRow(currentRowIndex);
            UpdateInstructionPointer(currentInstructionIndex); // Satýrý iþaretle
        }


        //Satýrý belirtmek için bu metod kullanýlabilir
        private void HighlightRow(int rowIndex)
        {
            if (currentRowIndex != -1)
            {
                // Önceki iþaretçiyi kaldýr
                dataGridView1.Rows[currentRowIndex].DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            }

            // Yeni iþaretçiyi ayarla
            dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Yellow;
            currentRowIndex = rowIndex; // Yeni iþaretçiyi güncelle
        }

        // Ýþlem sýrasýný güncelleme
        private void UpdateInstructionPointer(int currentInstructionIndex) //Instruction Memoryi iþaretler.
        {
            // Önceki iþaretçiyi kaldýr
            if (currentRowIndex != -1 && currentRowIndex < dataGridView2.Rows.Count)
            {
                dataGridView2.Rows[currentRowIndex].DefaultCellStyle.BackColor = dataGridView2.DefaultCellStyle.BackColor;
            }

            // Yeni iþaretçiyi ayarla
            if (currentInstructionIndex >= 0 && currentInstructionIndex < dataGridView2.Rows.Count)
            {
                dataGridView2.Rows[currentInstructionIndex].DefaultCellStyle.BackColor = Color.Yellow;
                currentRowIndex = currentInstructionIndex; // Yeni iþaretçiyi güncelle
            }
        }



        // Etiketlerin (labels) iþlenmesi
        //private List<string> ProcessLabels(List<string> assemblyCode)
        //{
        //    Dictionary<string, int> labels = new Dictionary<string, int>();
        //    List<string> processedCode = new List<string>();
        //    int currentAddress = 0;

        //    // First pass: Collect labels and their addresses
        //    foreach (string line in assemblyCode)
        //    {
        //        string trimmedLine = line.Trim();
        //        if (!string.IsNullOrWhiteSpace(trimmedLine))
        //        {
        //            string[] parts = trimmedLine.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        //            string firstPart = parts[0];

        //            if (firstPart.EndsWith(":"))
        //            {
        //                string label = firstPart.Substring(0, firstPart.Length - 1);
        //                if (!labels.ContainsKey(label))
        //                {
        //                    labels[label] = currentAddress;
        //                }
        //            }
        //            else
        //            {
        //                processedCode.Add(trimmedLine);
        //                currentAddress += 4; // Assuming each instruction is 4 bytes
        //            }
        //        }
        //    }

        //    // Second pass: Replace labels with their addresses
        //    List<string> finalCode = new List<string>();
        //    foreach (string line in processedCode)
        //    {
        //        string replacedLine = line;
        //        foreach (var label in labels)
        //        {
        //            if (line.Contains(label.Key))
        //            {
        //                replacedLine = replacedLine.Replace(label.Key, label.Value.ToString());
        //            }
        //        }
        //        finalCode.Add(replacedLine);
        //    }

        //    // Update the Compiler's Labels dictionary
        //    Compiler.SetLabels(labels);

        //    return finalCode;
        //}

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
                    dataGridView3.Rows[i].Cells[j + 1].Value = "0x00000000";
                }
            }
        }

        private void UpdateDataMemoryTable(string[] dataMemory)
        {
            int rowCount = dataMemory.Length; // DataMemory'den gelen veri sayýsý

            // Veri belleði tablosunun satýr sayýsýný kontrol et
            if (dataGridView3.Rows.Count != rowCount)
            {
                // Eðer tablonun satýr sayýsý, veri sayýsýna eþit deðilse, satýr sayýsýný güncelle
                dataGridView3.Rows.Clear(); // Tabloyu temizle

                // Her bir veri için bir satýr oluþtur ve tabloya ekle
                for (int i = 0; i < rowCount; i++)
                {
                    string address = "0x" + (i * 4).ToString("X8"); // Adresi hesapla
                    dataGridView3.Rows.Add(address); // Adresi tabloya ekle
                }
            }

            // Verileri tabloya yerleþtir
            for (int i = 0; i < rowCount; i++)
            {
                string[] values = dataMemory[i].Split(' '); // DataMemory'den gelen veriyi bölelim
                for (int j = 0; j < values.Length; j++)
                {
                    dataGridView3.Rows[i].Cells[j + 1].Value = values[j]; // Veriyi hücrelere yerleþtir
                }

                // Eðer hücrelerin geri kalaný boþsa, onlarý da "0x00000000" ile doldur
                for (int j = values.Length; j < 4; j++)
                {
                    dataGridView3.Rows[i].Cells[j + 1].Value = "0x00000000";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // TextBox'ý temizle
            textBox1.Text = "";

            // DataGridView'larý temizle
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();

            // Ýþaretçileri ve indeksleri sýfýrla
            currentRowIndex = -1;
            currentInstructionIndex = 0;

            // MIPS nesnesini sýfýrla
            mips.Reset();

            // Register tablosunu güncelle
            UpdateRegistersTable();

            // Talimat belleði tablosunu yeniden baþlat
            InitializeIMTable();

            // Veri belleði tablosunu yeniden baþlat
            InitializeDMTable();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
