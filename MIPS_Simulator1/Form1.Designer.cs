namespace MIPS_Simulator1
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			textBox1 = new TextBox();
			button1 = new Button();
			dataGridView1 = new DataGridView();
			Column1 = new DataGridViewTextBoxColumn();
			Column2 = new DataGridViewTextBoxColumn();
			Column3 = new DataGridViewTextBoxColumn();
			label1 = new Label();
			dataGridView2 = new DataGridView();
			Column4 = new DataGridViewTextBoxColumn();
			Column5 = new DataGridViewTextBoxColumn();
			Column6 = new DataGridViewTextBoxColumn();
			label2 = new Label();
			label3 = new Label();
			dataGridView3 = new DataGridView();
			Column7 = new DataGridViewTextBoxColumn();
			Column8 = new DataGridViewTextBoxColumn();
			Column9 = new DataGridViewTextBoxColumn();
			Column10 = new DataGridViewTextBoxColumn();
			Column11 = new DataGridViewTextBoxColumn();
			button2 = new Button();
			((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
			((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
			((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
			SuspendLayout();
			// 
			// textBox1
			// 
			textBox1.Location = new Point(27, 52);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(560, 219);
			textBox1.TabIndex = 0;
			textBox1.TextChanged += textBox1_TextChanged;
			// 
			// button1
			// 
			button1.Location = new Point(27, 23);
			button1.Name = "button1";
			button1.Size = new Size(75, 23);
			button1.TabIndex = 1;
			button1.Text = "Run";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// dataGridView1
			// 
			dataGridView1.BackgroundColor = Color.White;
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
			dataGridView1.GridColor = Color.Black;
			dataGridView1.Location = new Point(593, 49);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.Size = new Size(333, 219);
			dataGridView1.TabIndex = 2;
			dataGridView1.CellContentClick += dataGridView1_CellContentClick;
			// 
			// Column1
			// 
			Column1.HeaderText = "Name";
			Column1.Name = "Column1";
			// 
			// Column2
			// 
			Column2.HeaderText = "Number";
			Column2.Name = "Column2";
			// 
			// Column3
			// 
			Column3.HeaderText = "Value";
			Column3.Name = "Column3";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(593, 31);
			label1.Name = "label1";
			label1.Size = new Size(54, 15);
			label1.TabIndex = 3;
			label1.Text = "Registers";
			label1.Click += label1_Click;
			// 
			// dataGridView2
			// 
			dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView2.Columns.AddRange(new DataGridViewColumn[] { Column4, Column5, Column6 });
			dataGridView2.Location = new Point(27, 304);
			dataGridView2.Name = "dataGridView2";
			dataGridView2.Size = new Size(899, 143);
			dataGridView2.TabIndex = 4;
			dataGridView2.CellContentClick += dataGridView2_CellContentClick;
			// 
			// Column4
			// 
			Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column4.HeaderText = "Address";
			Column4.Name = "Column4";
			// 
			// Column5
			// 
			Column5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column5.HeaderText = "Code";
			Column5.Name = "Column5";
			// 
			// Column6
			// 
			Column6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column6.HeaderText = "Source";
			Column6.Name = "Column6";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(27, 286);
			label2.Name = "label2";
			label2.Size = new Size(108, 15);
			label2.TabIndex = 5;
			label2.Text = "Instrucion Memory";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(30, 452);
			label3.Name = "label3";
			label3.Size = new Size(79, 15);
			label3.TabIndex = 6;
			label3.Text = "Data Memory";
			label3.Click += label3_Click;
			// 
			// dataGridView3
			// 
			dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView3.Columns.AddRange(new DataGridViewColumn[] { Column7, Column8, Column9, Column10, Column11 });
			dataGridView3.Location = new Point(27, 470);
			dataGridView3.Name = "dataGridView3";
			dataGridView3.Size = new Size(899, 133);
			dataGridView3.TabIndex = 7;
			dataGridView3.CellContentClick += dataGridView3_CellContentClick;
			// 
			// Column7
			// 
			Column7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column7.HeaderText = "Address";
			Column7.Name = "Column7";
			// 
			// Column8
			// 
			Column8.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column8.HeaderText = "Value(+0)";
			Column8.Name = "Column8";
			// 
			// Column9
			// 
			Column9.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column9.HeaderText = "Value(+4)";
			Column9.Name = "Column9";
			// 
			// Column10
			// 
			Column10.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column10.HeaderText = "Value(+8)";
			Column10.Name = "Column10";
			// 
			// Column11
			// 
			Column11.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Column11.HeaderText = "Value(+c)";
			Column11.Name = "Column11";
			// 
			// button2
			// 
			button2.Location = new Point(141, 278);
			button2.Name = "button2";
			button2.Size = new Size(75, 23);
			button2.TabIndex = 8;
			button2.Text = "IM";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(938, 615);
			Controls.Add(button2);
			Controls.Add(dataGridView3);
			Controls.Add(label3);
			Controls.Add(label2);
			Controls.Add(dataGridView2);
			Controls.Add(label1);
			Controls.Add(dataGridView1);
			Controls.Add(button1);
			Controls.Add(textBox1);
			ImeMode = ImeMode.On;
			Name = "Form1";
			Text = "Form1";
			Load += Form1_Load;
			((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
			((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
			((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox textBox1;
		private Button button1;
		private DataGridView dataGridView1;
		private DataGridViewTextBoxColumn Column1;
		private DataGridViewTextBoxColumn Column2;
		private DataGridViewTextBoxColumn Column3;
		private Label label1;
		private DataGridView dataGridView2;
		private DataGridViewTextBoxColumn Column4;
		private DataGridViewTextBoxColumn Column5;
		private DataGridViewTextBoxColumn Column6;
		private Label label2;
		private Label label3;
		private DataGridView dataGridView3;
		private DataGridViewTextBoxColumn Column7;
		private DataGridViewTextBoxColumn Column8;
		private DataGridViewTextBoxColumn Column9;
		private DataGridViewTextBoxColumn Column10;
		private DataGridViewTextBoxColumn Column11;
		private Button button2;
	}
}
