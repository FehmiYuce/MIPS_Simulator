using Microsoft.Win32;

namespace MIPS_Simulator1
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> registers = Registers.RegisterMap;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)//run
        {
            // TextBox'tan assembly kodunu oku
            string assemblyCode = textBox1.Text;

            // Derleme i�lemini ger�ekle�tir
            string[] assemblyCodeArray = assemblyCode.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            CompileAssembly(assemblyCodeArray);
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
            SaveRegistersToTable();
            UpdateInstructionMemoryTable(Array.Empty<string>(), new List<string>());
            
        }
        private void SaveRegistersToTable()//register table
        {
            // Temizleme i�lemi, e�er tabloda varsa mevcut kay�tlar� kald�r�r
            dataGridView1.Rows.Clear();

            // Registerlar� tabloya ekler
            foreach (var register in registers)
            {
                string name = register.Key;
                string number = name.Substring(1); // "r" veya "s" karakterini kald�r�r
                string value = "0*00000000"; // Ba�lang�� de�erleri iste�e ba�l� olarak atanabilir

                // Yeni bir sat�r olu�tur ve kay�tlar� tabloya ekle
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = name;
                row.Cells[1].Value = number;
                row.Cells[2].Value = value;
                dataGridView1.Rows.Add(row);
            }
        }
        private void CompileAssembly(string[] assemblyCode)// textbox
        {
            try
            {
                List<string> machineCode = new List<string>();

                foreach (string instruction in assemblyCode)
                {
                    string compiledInstruction = Compiler.CompileInstruction(instruction);
                    string hexCode = Convert.ToInt32(compiledInstruction, 2).ToString("X").PadLeft(8, '0');
                    machineCode.Add(hexCode);
                }

                // Derlenmi� makine kodunu g�ster
                DisplayMachineCode(machineCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata olu�tu: " + ex.Message);
            }
        }

        private void DisplayMachineCode(List<string> machineCode)// textbox
        {
            // Temizleme i�lemi, e�er tabloda varsa mevcut kay�tlar� kald�r�r
            dataGridView1.Rows.Clear();

            // Derlenmi� makine kodunu tabloya ekle
            foreach (string code in machineCode)
            {
                // Yeni bir sat�r olu�tur ve makine kodunu tabloya ekle
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = code;
                dataGridView1.Rows.Add(row);
            }
        }

        private void button2_Click(object sender, EventArgs e)//IM
        {
            // TextBox'tan assembly kodunu oku
            string[] assemblyCode = textBox1.Lines;

            // Varsay�lan makine kodu olu�tur
            List<string> defaultMachineCode = new List<string>();
            for (int i = 0; i < assemblyCode.Length; i++)
            {
                defaultMachineCode.Add("0x00000000");
            }

            // Instruction Memory tablosunu g�ncelle (varsay�lan de�erlerle)
            UpdateInstructionMemoryTable(assemblyCode, defaultMachineCode);
        }

        private void UpdateInstructionMemoryTable(string[] assemblyCode, List<string> machineCode)
        {
            // Temizleme i�lemi, e�er tabloda varsa mevcut kay�tlar� kald�r�r
            dataGridView2.Rows.Clear();

            // Assembly kodunu ve makine kodunu tabloya ekle
            for (int i = 0; i < assemblyCode.Length; i++)
            {
                string instruction = assemblyCode[i];
                string address = "0x" + (i * 4).ToString("X8"); // Adres
                string code = machineCode[i]; // Makine kodu
                string source = instruction; // Kaynak

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView2);
                row.Cells[0].Value = address;
                row.Cells[1].Value = code;
                row.Cells[2].Value = source;
                dataGridView2.Rows.Add(row);
            }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void UpdateDataMemoryTable(int[] dataMemory)
        {
            // Temizleme i�lemi, e�er tabloda varsa mevcut kay�tlar� kald�r�r
            dataGridView3.Rows.Clear();

            // Adres ve de�er ba�l�klar�n� ekle
            dataGridView3.Columns.Add("Address", "Address");
            for (int i = 0; i < 8; i++)
            {
                dataGridView3.Columns.Add("Value+" + (i * 4).ToString("X2"), "Value+" + (i * 4).ToString("X2"));
            }

            // Data Memory tablosunu g�ncelle
            for (int i = 0; i < dataMemory.Length; i += 8)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView3);
                row.Cells[0].Value = "0x" + (i * 4).ToString("X8"); // Adresi ekle
                for (int j = 0; j < 8; j++)
                {
                    row.Cells[j + 1].Value = "0x" + dataMemory[i + j].ToString("X8"); // De�erleri ekle
                }
                dataGridView3.Rows.Add(row);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // TextBox'tan veri belle�i i�eri�ini oku
            string[] dataMemory = textBox1.Lines;

            // Veri belle�ini g�ncelle
            //UpdateDataMemoryTable(dataMemory);
        }
    }
}
