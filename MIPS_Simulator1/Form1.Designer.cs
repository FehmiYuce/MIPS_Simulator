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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            dataGridView3 = new DataGridView();
            Column7 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            Column11 = new DataGridViewTextBoxColumn();
            button2 = new Button();
            button3 = new Button();
            richTextBox1 = new RichTextBox();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            button4 = new Button();
            pictureBox5 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ButtonHighlight;
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.FlatStyle = FlatStyle.Popup;
            button1.ImageAlign = ContentAlignment.TopLeft;
            button1.Location = new Point(1139, 24);
            button1.Name = "button1";
            button1.Size = new Size(244, 28);
            button1.TabIndex = 1;
            button1.Text = "Assemble";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.MenuHighlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.GridColor = Color.Black;
            dataGridView1.Location = new Point(1430, 24);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView1.Size = new Size(344, 806);
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
            Column3.ReadOnly = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(1430, 1);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 3;
            label1.Text = "Registers";
            label1.Click += label1_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { Column4, Column5, Column6 });
            dataGridView2.Location = new Point(27, 430);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(1369, 174);
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
            label2.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(27, 407);
            label2.Name = "label2";
            label2.Size = new Size(140, 20);
            label2.TabIndex = 5;
            label2.Text = "Instrucion Memory";
            label2.Click += label2_Click;
            // 
            // dataGridView3
            // 
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { Column7, Column8, Column9, Column10, Column11 });
            dataGridView3.Location = new Point(27, 648);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.Size = new Size(1369, 182);
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
            button2.BackColor = SystemColors.ButtonHighlight;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Location = new Point(1139, 73);
            button2.Name = "button2";
            button2.Size = new Size(244, 28);
            button2.TabIndex = 8;
            button2.Text = "Next";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ButtonHighlight;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Location = new Point(1139, 350);
            button3.Name = "button3";
            button3.Size = new Size(244, 27);
            button3.TabIndex = 9;
            button3.Text = "Clear";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(27, 24);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(1054, 363);
            richTextBox1.TabIndex = 10;
            richTextBox1.Text = "";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(27, 616);
            label4.Name = "label4";
            label4.Size = new Size(104, 20);
            label4.TabIndex = 11;
            label4.Text = "Data Memory";
            label4.Click += label4_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(1087, 120);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(28, 28);
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImage = (Image)resources.GetObject("pictureBox2.BackgroundImage");
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(1087, 72);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(28, 29);
            pictureBox2.TabIndex = 13;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.BackgroundImage = (Image)resources.GetObject("pictureBox3.BackgroundImage");
            pictureBox3.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox3.Location = new Point(1087, 349);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(28, 28);
            pictureBox3.TabIndex = 14;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.BackgroundImage = (Image)resources.GetObject("pictureBox4.BackgroundImage");
            pictureBox4.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox4.Location = new Point(1139, 175);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(244, 146);
            pictureBox4.TabIndex = 15;
            pictureBox4.TabStop = false;
            // 
            // button4
            // 
            button4.BackColor = Color.White;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Location = new Point(1139, 120);
            button4.Name = "button4";
            button4.Size = new Size(244, 30);
            button4.TabIndex = 16;
            button4.Text = "Run All";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // pictureBox5
            // 
            pictureBox5.BackgroundImage = (Image)resources.GetObject("pictureBox5.BackgroundImage");
            pictureBox5.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox5.Location = new Point(1087, 24);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(28, 27);
            pictureBox5.TabIndex = 17;
            pictureBox5.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.MintCream;
            ClientSize = new Size(1904, 955);
            Controls.Add(pictureBox5);
            Controls.Add(button4);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(label4);
            Controls.Add(richTextBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(dataGridView3);
            Controls.Add(label2);
            Controls.Add(dataGridView2);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            ImeMode = ImeMode.On;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FAMEE SIMULATOR";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
		private DataGridView dataGridView1;
		private Label label1;
		private DataGridView dataGridView2;
		private DataGridViewTextBoxColumn Column4;
		private DataGridViewTextBoxColumn Column5;
		private DataGridViewTextBoxColumn Column6;
		private Label label2;
		private DataGridView dataGridView3;
		private DataGridViewTextBoxColumn Column7;
		private DataGridViewTextBoxColumn Column8;
		private DataGridViewTextBoxColumn Column9;
		private DataGridViewTextBoxColumn Column10;
		private DataGridViewTextBoxColumn Column11;
        private Button button2;
        private Button button3;
        private RichTextBox richTextBox1;
        private Label label4;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private Button button4;
        private PictureBox pictureBox5;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
    }
}
