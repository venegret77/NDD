namespace NetworkDesign.Main
{
    partial class MapExportForm
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.NW = new System.Windows.Forms.CheckBox();
            this.NE = new System.Windows.Forms.CheckBox();
            this._Text = new System.Windows.Forms.CheckBox();
            this.IW = new System.Windows.Forms.CheckBox();
            this.Ent = new System.Windows.Forms.CheckBox();
            this.Build = new System.Windows.Forms.CheckBox();
            this.Circ = new System.Windows.Forms.CheckBox();
            this.Poly = new System.Windows.Forms.CheckBox();
            this.Rect = new System.Windows.Forms.CheckBox();
            this.Line = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(257, 28);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(138, 109);
            this.checkedListBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(254, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Выберите нужные здания:";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(269, 146);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(120, 17);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Список элементов";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(269, 168);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(95, 17);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.Text = "Изображение";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(279, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Выйти";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(279, 234);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Экспортировать";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.NW);
            this.groupBox1.Controls.Add(this.NE);
            this.groupBox1.Controls.Add(this._Text);
            this.groupBox1.Controls.Add(this.IW);
            this.groupBox1.Controls.Add(this.Ent);
            this.groupBox1.Controls.Add(this.Build);
            this.groupBox1.Controls.Add(this.Circ);
            this.groupBox1.Controls.Add(this.Poly);
            this.groupBox1.Controls.Add(this.Rect);
            this.groupBox1.Controls.Add(this.Line);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 251);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Фильтры";
            // 
            // NW
            // 
            this.NW.AutoSize = true;
            this.NW.Checked = true;
            this.NW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NW.Location = new System.Drawing.Point(6, 226);
            this.NW.Name = "NW";
            this.NW.Size = new System.Drawing.Size(134, 17);
            this.NW.TabIndex = 19;
            this.NW.Text = "Показывать провода";
            this.NW.UseVisualStyleBackColor = true;
            // 
            // NE
            // 
            this.NE.AutoSize = true;
            this.NE.Checked = true;
            this.NE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NE.Location = new System.Drawing.Point(6, 203);
            this.NE.Name = "NE";
            this.NE.Size = new System.Drawing.Size(189, 17);
            this.NE.TabIndex = 18;
            this.NE.Text = "Показывать сетевые эелменты";
            this.NE.UseVisualStyleBackColor = true;
            // 
            // _Text
            // 
            this._Text.AutoSize = true;
            this._Text.Checked = true;
            this._Text.CheckState = System.Windows.Forms.CheckState.Checked;
            this._Text.Location = new System.Drawing.Point(6, 180);
            this._Text.Name = "_Text";
            this._Text.Size = new System.Drawing.Size(134, 17);
            this._Text.TabIndex = 17;
            this._Text.Text = "Показывать надписи";
            this._Text.UseVisualStyleBackColor = true;
            // 
            // IW
            // 
            this.IW.AutoSize = true;
            this.IW.Checked = true;
            this.IW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IW.Location = new System.Drawing.Point(6, 157);
            this.IW.Name = "IW";
            this.IW.Size = new System.Drawing.Size(222, 17);
            this.IW.TabIndex = 16;
            this.IW.Text = "Показывать входы проводов в здания";
            this.IW.UseVisualStyleBackColor = true;
            // 
            // Ent
            // 
            this.Ent.AutoSize = true;
            this.Ent.Checked = true;
            this.Ent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Ent.Location = new System.Drawing.Point(6, 134);
            this.Ent.Name = "Ent";
            this.Ent.Size = new System.Drawing.Size(171, 17);
            this.Ent.TabIndex = 15;
            this.Ent.Text = "Показывать входы в здания";
            this.Ent.UseVisualStyleBackColor = true;
            // 
            // Build
            // 
            this.Build.AutoSize = true;
            this.Build.Checked = true;
            this.Build.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Build.Location = new System.Drawing.Point(6, 111);
            this.Build.Name = "Build";
            this.Build.Size = new System.Drawing.Size(128, 17);
            this.Build.TabIndex = 14;
            this.Build.Text = "Показывать здания";
            this.Build.UseVisualStyleBackColor = true;
            // 
            // Circ
            // 
            this.Circ.AutoSize = true;
            this.Circ.Checked = true;
            this.Circ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Circ.Location = new System.Drawing.Point(6, 88);
            this.Circ.Name = "Circ";
            this.Circ.Size = new System.Drawing.Size(120, 17);
            this.Circ.TabIndex = 13;
            this.Circ.Text = "Показывать круги";
            this.Circ.UseVisualStyleBackColor = true;
            // 
            // Poly
            // 
            this.Poly.AutoSize = true;
            this.Poly.Checked = true;
            this.Poly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Poly.Location = new System.Drawing.Point(6, 65);
            this.Poly.Name = "Poly";
            this.Poly.Size = new System.Drawing.Size(175, 17);
            this.Poly.TabIndex = 12;
            this.Poly.Text = "Показывать многоугольники";
            this.Poly.UseVisualStyleBackColor = true;
            // 
            // Rect
            // 
            this.Rect.AutoSize = true;
            this.Rect.Checked = true;
            this.Rect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Rect.Location = new System.Drawing.Point(6, 42);
            this.Rect.Name = "Rect";
            this.Rect.Size = new System.Drawing.Size(176, 17);
            this.Rect.TabIndex = 11;
            this.Rect.Text = "Показывать прямоугольники";
            this.Rect.UseVisualStyleBackColor = true;
            // 
            // Line
            // 
            this.Line.AutoSize = true;
            this.Line.Checked = true;
            this.Line.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Line.Location = new System.Drawing.Point(6, 19);
            this.Line.Name = "Line";
            this.Line.Size = new System.Drawing.Size(122, 17);
            this.Line.TabIndex = 10;
            this.Line.Text = "Показывать линии";
            this.Line.UseVisualStyleBackColor = true;
            // 
            // MapExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 272);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "MapExportForm";
            this.Text = "Выберите параметры экспорта";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox NW;
        private System.Windows.Forms.CheckBox NE;
        private System.Windows.Forms.CheckBox _Text;
        private System.Windows.Forms.CheckBox IW;
        private System.Windows.Forms.CheckBox Ent;
        private System.Windows.Forms.CheckBox Build;
        private System.Windows.Forms.CheckBox Circ;
        private System.Windows.Forms.CheckBox Poly;
        private System.Windows.Forms.CheckBox Rect;
        private System.Windows.Forms.CheckBox Line;
    }
}