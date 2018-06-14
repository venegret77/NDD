namespace NetworkDesign
{
    partial class ColorDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.linesbtn = new System.Windows.Forms.Button();
            this.polygonbtn = new System.Windows.Forms.Button();
            this.rectbtn = new System.Windows.Forms.Button();
            this.activeelembtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button5 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.buildbtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.entrancebtn = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.iwbtn = new System.Windows.Forms.Button();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.circlebtn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // linesbtn
            // 
            this.linesbtn.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.linesbtn.Location = new System.Drawing.Point(163, 4);
            this.linesbtn.Name = "linesbtn";
            this.linesbtn.Size = new System.Drawing.Size(96, 23);
            this.linesbtn.TabIndex = 0;
            this.linesbtn.Text = "  ";
            this.linesbtn.UseVisualStyleBackColor = false;
            this.linesbtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // polygonbtn
            // 
            this.polygonbtn.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.polygonbtn.Location = new System.Drawing.Point(163, 33);
            this.polygonbtn.Name = "polygonbtn";
            this.polygonbtn.Size = new System.Drawing.Size(96, 23);
            this.polygonbtn.TabIndex = 1;
            this.polygonbtn.Text = "  ";
            this.polygonbtn.UseVisualStyleBackColor = false;
            this.polygonbtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // rectbtn
            // 
            this.rectbtn.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.rectbtn.Location = new System.Drawing.Point(163, 61);
            this.rectbtn.Name = "rectbtn";
            this.rectbtn.Size = new System.Drawing.Size(96, 23);
            this.rectbtn.TabIndex = 2;
            this.rectbtn.Text = "  ";
            this.rectbtn.UseVisualStyleBackColor = false;
            this.rectbtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // activeelembtn
            // 
            this.activeelembtn.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.activeelembtn.Location = new System.Drawing.Point(163, 211);
            this.activeelembtn.Name = "activeelembtn";
            this.activeelembtn.Size = new System.Drawing.Size(96, 23);
            this.activeelembtn.TabIndex = 3;
            this.activeelembtn.Text = "  ";
            this.activeelembtn.UseVisualStyleBackColor = false;
            this.activeelembtn.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Линии";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Многоугольники";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Прямоугольники";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 216);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Цвет активных элементов";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 282);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Толщина линий";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(104, 280);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(49, 20);
            this.numericUpDown1.TabIndex = 9;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(163, 277);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(96, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "Закрыть";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Здания";
            // 
            // buildbtn
            // 
            this.buildbtn.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.buildbtn.Location = new System.Drawing.Point(163, 121);
            this.buildbtn.Name = "buildbtn";
            this.buildbtn.Size = new System.Drawing.Size(96, 23);
            this.buildbtn.TabIndex = 11;
            this.buildbtn.Text = "  ";
            this.buildbtn.UseVisualStyleBackColor = false;
            this.buildbtn.Click += new System.EventHandler(this.button6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Входы";
            // 
            // entrancebtn
            // 
            this.entrancebtn.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.entrancebtn.Location = new System.Drawing.Point(163, 150);
            this.entrancebtn.Name = "entrancebtn";
            this.entrancebtn.Size = new System.Drawing.Size(96, 23);
            this.entrancebtn.TabIndex = 13;
            this.entrancebtn.Text = "  ";
            this.entrancebtn.UseVisualStyleBackColor = false;
            this.entrancebtn.Click += new System.EventHandler(this.button7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 184);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Вход провода";
            // 
            // iwbtn
            // 
            this.iwbtn.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.iwbtn.Location = new System.Drawing.Point(163, 179);
            this.iwbtn.Name = "iwbtn";
            this.iwbtn.Size = new System.Drawing.Size(96, 23);
            this.iwbtn.TabIndex = 15;
            this.iwbtn.Text = "  ";
            this.iwbtn.UseVisualStyleBackColor = false;
            this.iwbtn.Click += new System.EventHandler(this.button8_Click);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(104, 153);
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(49, 20);
            this.numericUpDown2.TabIndex = 17;
            this.numericUpDown2.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(104, 182);
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(49, 20);
            this.numericUpDown3.TabIndex = 18;
            this.numericUpDown3.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Круги";
            // 
            // circlebtn
            // 
            this.circlebtn.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.circlebtn.Location = new System.Drawing.Point(163, 90);
            this.circlebtn.Name = "circlebtn";
            this.circlebtn.Size = new System.Drawing.Size(96, 23);
            this.circlebtn.TabIndex = 19;
            this.circlebtn.Text = "  ";
            this.circlebtn.UseVisualStyleBackColor = false;
            this.circlebtn.Click += new System.EventHandler(this.button9_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 250);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(149, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Сетевые провода (min - max)";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.button1.Location = new System.Drawing.Point(163, 245);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(49, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "  ";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.button2.Location = new System.Drawing.Point(210, 245);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(49, 23);
            this.button2.TabIndex = 23;
            this.button2.Text = "  ";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(205, 314);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(54, 50);
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(103, 327);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 23);
            this.button3.TabIndex = 25;
            this.button3.Text = "Выбрать фон";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "PNG|*.png";
            // 
            // ColorDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 376);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.circlebtn);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.iwbtn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.entrancebtn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buildbtn);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.activeelembtn);
            this.Controls.Add(this.rectbtn);
            this.Controls.Add(this.polygonbtn);
            this.Controls.Add(this.linesbtn);
            this.Name = "ColorDialogForm";
            this.Text = "Выберите цвета";
            this.Load += new System.EventHandler(this.ColorDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button linesbtn;
        private System.Windows.Forms.Button polygonbtn;
        private System.Windows.Forms.Button rectbtn;
        private System.Windows.Forms.Button activeelembtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buildbtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button entrancebtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button iwbtn;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button circlebtn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}