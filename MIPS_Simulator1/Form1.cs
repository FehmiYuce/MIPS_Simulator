using Microsoft.Win32;

namespace MIPS_Simulator1
{
    public partial class Form1 : Form
    {
        //private Dictionary<string, string> registers = Registers.RegisterMap;
        private MIPS mips;
        private Dictionary<string, string> registers;
        private List<string> assemblyCode;

        public Form1()
        {
            InitializeComponent();
            mips = new MIPS();
            registers = Registers.RegisterMap;
            SaveRegistersToTable();
            UpdateInstructionMemoryTable(Array.Empty<string>(), new List<string>());
            UpdateDataMemoryTable(mips.DataMemory);
        }

        private void button1_Click(object sender, EventArgs e) // Run button
        {
            assemblyCode = textBox1.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
            CompileAssembly(assemblyCode);
            CompileNextAssemblyInstruction(); // Compile next instruction after running
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
        private void SaveRegistersToTable()
        {
            dataGridView1.Rows.Clear();

            // Register Map'ten kayýtlarý alýrken hi, lo ve pc'yi ekleyin
            foreach (var register in registers)
            {
                string name = register.Key;
                string binaryNumber = register.Value;
                int number = Convert.ToInt32(binaryNumber, 2);
                string value = "0x" + register.Value; // String olarak makinadaki de?eri alýr

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = name;
                row.Cells[1].Value = number.ToString();
                row.Cells[2].Value = value;
                dataGridView1.Rows.Add(row);
            }

            // hi, lo ve pc kayýtlarýný ekleyin
            dataGridView1.Rows.Add("hi", "", ""); // Boþ hücreler, çünkü hi'nin ve lo'nun deðeri 64 bitlik olabilir
            dataGridView1.Rows.Add("lo", "", "");
            dataGridView1.Rows.Add("pc", "", ""); // Program Sayacý
        }

        private void CompileAssembly(List<string> assemblyCode) // textbox
        {
            try
            {
                List<string> machineCode = Compiler.CompileToHex(assemblyCode);
                DisplayMachineCode(machineCode);
                UpdateDataMemoryTable(mips.DataMemory);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluþtu: " + ex.Message);
            }
        }
        private void CompileNextAssemblyInstruction()
        {
            if (mips.StepCount < assemblyCode.Count)
            {
                mips.Step();
                if (mips.StepCount <= assemblyCode.Count) // Burada '<=' yerine '<' kullanýlmalý
                {
                    string currentInstruction = assemblyCode[mips.StepCount - 1];
                    List<string> currentInstructionList = new List<string> { currentInstruction };
                    CompileAssembly(currentInstructionList);
                }
                else
                {
                    MessageBox.Show("Tüm talimatlar iþlendi.");
                }
            }
        }




        private void DisplayMachineCode(List<string> machineCode) // textbox
        {
            dataGridView2.Rows.Clear();

            for (int i = 0; i < machineCode.Count; i++)
            {
                string instruction = machineCode[i];
                string address = "0x" + (i * 4).ToString("X8");
                string source = assemblyCode[i];

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView2);
                row.Cells[0].Value = address;
                row.Cells[1].Value = instruction;
                row.Cells[2].Value = source;
                dataGridView2.Rows.Add(row);
            }
        }

        private void button2_Click(object sender, EventArgs e) // IM
        {
            string[] assemblyCode = textBox1.Lines;
            List<string> defaultMachineCode = new List<string>();

            for (int i = 0; i < assemblyCode.Length; i++)
            {
                defaultMachineCode.Add("0x00000000");
            }

            UpdateInstructionMemoryTable(assemblyCode, defaultMachineCode);
        }

        private void UpdateInstructionMemoryTable(string[] assemblyCode, List<string> machineCode)
        {
            dataGridView2.Rows.Clear();

            for (int i = 0; i < assemblyCode.Length; i++)
            {
                string instruction = assemblyCode[i];
                string address = "0x" + (i * 4).ToString("X8");
                string code = machineCode[i];
                string source = instruction;

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
            dataGridView3.Rows.Clear();

            for (int i = 0; i < dataMemory.Length; i += 4)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView3);
                row.Cells[0].Value = "0x" + (i * 4).ToString("X8");

                for (int j = 0; j < 4; j++)
                {
                    int index = i + j;
                    if (index < dataMemory.Length)
                    {
                        row.Cells[j + 1].Value = "0x" + dataMemory[index].ToString("X8");
                    }
                    else
                    {
                        row.Cells[j + 1].Value = "0x00000000";
                    }
                }

                dataGridView3.Rows.Add(row);
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            string[] dataMemory = textBox1.Lines;

            if (dataMemory.Length != mips.DataMemory.Length)
            {
                MessageBox.Show("Veri belleði giriþi, beklenen boyuta sahip deðil.");
                return;
            }

            if (!IsValidDataMemoryInput(dataMemory))
            {
                MessageBox.Show("Geçersiz veri belleði giriþi.");
                return;
            }

            mips.DataMemory = ConvertDataMemory(dataMemory);
            UpdateDataMemoryTable(mips.DataMemory);
            MessageBox.Show("Veri belleði güncellendi.");
        }
        private bool IsValidDataMemoryInput(string[] dataMemory)
        {
            foreach (string data in dataMemory)
            {
                if (!int.TryParse(data, System.Globalization.NumberStyles.HexNumber, null, out _))
                {
                    return false;
                }
            }
            return true;
        }

        private int[] ConvertDataMemory(string[] dataMemory)
        {
            int[] memory = new int[dataMemory.Length];

            for (int i = 0; i < dataMemory.Length; i++)
            {
                memory[i] = Convert.ToInt32(dataMemory[i], 16);
            }

            return memory;
        }
    }
}
