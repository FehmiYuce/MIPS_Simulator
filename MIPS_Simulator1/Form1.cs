using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing; // Bu sat�r� ekleyin


namespace MIPS_Simulator1
{
    public partial class Form1 : Form
    {
        private MIPS mips;
        private Dictionary<string, int> registers;
        private int currentRowIndex = -1; // Ba�lang��ta i�aret�i yok
        int currentInstructionIndex = 0; // ��lem s�ras�n�n ba�lang��ta 0 oldu�unu varsayal�m

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

            // Etiketlerin (labels) i�lenmesi
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

        // De�i�en register h�crelerini takip etmek i�in bir liste ekleyin
        private List<DataGridViewCell> changedCells = new List<DataGridViewCell>();

        private void HighlightChangedCellsInColumn3()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string cellValue = row.Cells[2].Value?.ToString(); // 3. s�tundaki de�eri al�n

                // �nceki de�eri depolamak i�in her sat�r i�in bir etiket olu�turun (Tag)
                if (cellValue != null)
                    row.Cells[2].Tag = cellValue;

                // De�i�en h�creleri vurgulay�n
                if (row.Cells[2].Tag != null && cellValue != row.Cells[2].Tag.ToString())
                {
                    row.Cells[2].Style.BackColor = Color.Green; // De�i�en h�creleri vurgula
                }
                else
                {
                    row.Cells[2].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor; // De�i�meyen h�creleri varsay�lan renge geri d�n
                }
            }
        }


        private int currentLineIndex = 0; // Ba�lang��ta 0. sat�rdan ba�layal�m

        private string[] oldRegisterValues;
        private void button2_Click(object sender, EventArgs e) // Step button
        {
            mips.Step();
            UpdateRegistersTable();
            UpdateDataMemoryTable(mips.DMToHex());

            oldRegisterValues = mips.RegToHex().ToArray();

            // �nceki sat�r�n vurgusunu temizle
            ClearRowHighlight(currentLineIndex - 1);

            // Yeni sat�r�n vurgusunu yap
            HighlightCurrentLine();

            // Sat�r� i�aretle
            UpdateInstructionPointer(currentLineIndex);

            // De�i�en register h�crelerini vurgula
            HighlightChangedRegisterCells(oldRegisterValues);

            HighlightChangedCellsInColumn3();
        }

        

        // De�i�en register h�crelerini vurgulayan metot
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
                            row.Cells[2].Style.BackColor = Color.LightPink; // Vurgulamak i�in sar� rengi kullan
                            changedCells.Add(row.Cells[2]); // De�i�en h�creyi listeye ekle
                        }
                    }
                }
            }
        }

        public static class Registers
        {
            public static Dictionary<string, int> RegisterMap { get; } = new Dictionary<string, int>
            {
                { "$zero", 0 }, { "$at", 1 }, { "$v0", 2 }, { "$v1", 3 },
                { "$a0", 4 }, { "$a1", 5 }, { "$a2", 6 }, { "$a3", 7 },
                { "$t0", 8 }, { "$t1", 9 }, { "$t2", 10 }, { "$t3", 11 },
                { "$t4", 12 }, { "$t5", 13 }, { "$t6", 14 }, { "$t7", 15 },
                { "$s0", 16 }, { "$s1", 17 }, { "$s2", 18 }, { "$s3", 19 },
                { "$s4", 20 }, { "$s5", 21 }, { "$s6", 22 }, { "$s7", 23 },
                { "$t8", 24 }, { "$t9", 25 }, { "$k0", 26 }, { "$k1", 27 },
                { "$gp", 28 }, { "$sp", 29 }, { "$fp", 30 }, { "$ra", 31 },
                { "pc", -1 }, { "hi", -1 }, { "lo", -1 }
            };



            public static List<string> RegisterList { get; } = new List<string>
    {
        "$zero", "$at", "$v0", "$v1", "$a0", "$a1", "$a2", "$a3",
        "$t0", "$t1", "$t2", "$t3", "$t4", "$t5", "$t6", "$t7",
        "$s0", "$s1", "$s2", "$s3", "$s4", "$s5", "$s6", "$s7",
        "$t8", "$t9", "$k0", "$k1", "$gp", "$sp", "$fp", "$ra",
        "pc", "hi", "lo"
    };
        }



        private void HighlightRowByResult()
        {
            // Her bir sat�r i�in sonucu kontrol et
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string result = GetResultFromRow(row);
                if (result == null)
                {
                    // Sat�r�n sonucu yoksa varsay�lan arka plan rengini kullan
                    row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                }
                else
                {
                    // Sat�r�n sonucuna g�re arka plan rengini belirle
                    if (result == "DesiredResult") // Burada "DesiredResult" sonucunu kontrol ediyoruz
                    {
                        row.DefaultCellStyle.BackColor = Color.BlueViolet; // �rnek olarak mavi-mor renk kulland�m
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                    }
                }
            }
        }

        // DataGridView'deki bir sat�rdan sonucu almak i�in bu metodu kullanabiliriz
        private string GetResultFromRow(DataGridViewRow row)
        {
            // �rne�in, 3. h�credeki de�eri sonu� olarak alal�m
            if (row.Cells.Count > 2 && row.Cells[2].Value != null)
            {
                return row.Cells[2].Value.ToString();
            }
            return null; // E�er sonu� yoksa null d�nd�r
        }


        private string GetCurrentLineText()
        {
            if (currentLineIndex >= 0 && currentLineIndex < richTextBox1.Lines.Length)
            {
                return richTextBox1.Lines[currentLineIndex];
            }
            else
            {
                return string.Empty;
            }
        }

        private string ProcessLineAndGetResult(string line)
        {
            // Burada sat�r�n i�lenmesi ve sonucun elde edilmesi i�lemlerini yap�n
            // �rne�in:
            // string result = ProcessLine(line);
            // return result;

            // Burada �rnek bir sonu� d�nd�r�yorum, siz ger�ek i�lemlerinizi burada yapmal�s�n�z
            return "ResultValue";
        }


        private void HighlightCurrentLine()
        {
            // Aktif sat�r� vurgula
            if (currentLineIndex < richTextBox1.Lines.Length)
            {
                richTextBox1.Select(richTextBox1.GetFirstCharIndexFromLine(currentLineIndex), richTextBox1.Lines[currentLineIndex].Length);
                richTextBox1.SelectionBackColor = Color.Aqua;
            }
            else
            {
                // E�er t�m sat�rlar vurgulanm��sa, ba�a d�n
                currentLineIndex = 0;
            }

            // Bir sonraki sat�ra ge�
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






        // ��lem s�ras�n� g�ncelleme
        private void UpdateInstructionPointer(int currentInstructionIndex) //Instruction Memoryi i�aretler.
        {
            // �nceki i�aret�iyi kald�r
            if (currentRowIndex != -1 && currentRowIndex < dataGridView2.Rows.Count)
            {
                dataGridView2.Rows[currentRowIndex].DefaultCellStyle.BackColor = dataGridView2.DefaultCellStyle.BackColor;
            }

            // Yeni i�aret�iyi ayarla
            if (currentInstructionIndex >= 0 && currentInstructionIndex < dataGridView2.Rows.Count)
            {
                dataGridView2.Rows[currentInstructionIndex].DefaultCellStyle.BackColor = Color.White;
                currentRowIndex = currentInstructionIndex; // Yeni i�aret�iyi g�ncelle
            }
        }

        private void UpdateRegisterPointer(string registerName)
        {
            // �nceki i�aret�iyi kald�r
            if (currentRowIndex != -1 && currentRowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[currentRowIndex].DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            }

            // Yeni i�aret�iyi ayarla
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == registerName)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    currentRowIndex = row.Index; // Yeni i�aret�iyi g�ncelle
                    break;
                }
            }
        }




        // Etiketlerin (labels) i�lenmesi
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

        public void UpdateRegistersTable()
        {
            string[] registerValues = mips.RegToHex();
            string pcValue = mips.PCToHex().ToString();
            string hiValue = mips.HiToHex().ToString();
            string loValue = mips.LoToHex().ToString();

            // De�erleri registerValues dizisine ekleyin
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
            int rowCount = dataMemory.Length; // DataMemory'den gelen veri say�s�

            // Veri belle�i tablosunun sat�r say�s�n� kontrol et
            if (dataGridView3.Rows.Count != rowCount)
            {
                // E�er tablonun sat�r say�s�, veri say�s�na e�it de�ilse, sat�r say�s�n� g�ncelle
                dataGridView3.Rows.Clear(); // Tabloyu temizle

                // Her bir veri i�in bir sat�r olu�tur ve tabloya ekle
                for (int i = 0; i < rowCount; i++)
                {
                    string address = "0x" + (i * 4).ToString("X8"); // Adresi hesapla
                    dataGridView3.Rows.Add(address); // Adresi tabloya ekle
                }
            }

            // Verileri tabloya yerle�tir
            for (int i = 0; i < rowCount; i++)
            {
                string[] values = dataMemory[i].Split(' '); // DataMemory'den gelen veriyi b�lelim
                for (int j = 0; j < values.Length; j++)
                {
                    dataGridView3.Rows[i].Cells[j + 1].Value = values[j]; // Veriyi h�crelere yerle�tir
                }

                // E�er h�crelerin geri kalan� bo�sa, onlar� da "0x00000000" ile doldur
                for (int j = values.Length; j < 4; j++)
                {
                    dataGridView3.Rows[i].Cells[j + 1].Value = "0x00000000";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // TextBox'� temizle
            richTextBox1.Text = "";

            // DataGridView'lar� temizle
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();

            // ��aret�ileri ve indeksleri s�f�rla
            currentRowIndex = -1;
            currentInstructionIndex = 0;

            // MIPS nesnesini s�f�rla
            mips.Reset();

            // Register tablosunu g�ncelle
            UpdateRegistersTable();

            // Talimat belle�i tablosunu yeniden ba�lat
            InitializeIMTable();

            // Veri belle�i tablosunu yeniden ba�lat
            InitializeDMTable();

           

            ClearChangedCellHighlights();


        }

        // De�i�en h�crelerin vurgusunu temizleyen metot
        private void ClearChangedCellHighlights()
        {
            foreach (var cell in changedCells)
            {
                cell.Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            }
            changedCells.Clear();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // �ncelikle i�lemi yap
            mips.RunUntilEnd();
            UpdateRegistersTable();
            // Sonucu DataGridView'e yazd�r
            string[] sonuc = mips.RegToHex();
            string pcValue = mips.PCToHex().ToString();
            string hiValue = mips.HiToHex().ToString();
            string loValue = mips.LoToHex().ToString();
            sonuc = sonuc.Concat(new string[] { pcValue, hiValue, loValue }).ToArray();
            dataGridView1.Rows.Clear();

            List<(string Name, int Number)> registerInfo = new List<(string, int)>
        {
            ("$zero", 0), ("$at", 1), ("$v0", 2), ("$v1", 3), ("$a0", 4), ("$a1", 5), ("$a2", 6), ("$a3", 7),
            ("$t0", 8), ("$t1", 9), ("$t2", 10), ("$t3", 11), ("$t4", 12), ("$t5", 13), ("$t6", 14), ("$t7", 15),
            ("$s0", 16), ("$s1", 17), ("$s2", 18), ("$s3", 19), ("$s4", 20), ("$s5", 21), ("$s6", 22), ("$s7", 23),
            ("$t8", 24), ("$t9", 25), ("$k0", 26), ("$k1", 27), ("$gp", 28), ("$sp", 29), ("$fp", 30), ("$ra", 31),
            ("pc", -1), ("hi", -1), ("lo", -1)
        };

            for (int i = 0; i < sonuc.Length; i++)
            {
                string name = registerInfo[i].Name;
                int number = registerInfo[i].Number;
                dataGridView1.Rows.Add(name, number, sonuc[i]); // Bo� name ve number, sonu� s�tununa yaz�l�r
            }

            HighlightChangedCellsInColumn3();
        }

    }
}