namespace NetworkDesign.Main
{
    partial class FiltersForm
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
            this.Line = new System.Windows.Forms.CheckBox();
            this.Rect = new System.Windows.Forms.CheckBox();
            this.Poly = new System.Windows.Forms.CheckBox();
            this.Circ = new System.Windows.Forms.CheckBox();
            this.Build = new System.Windows.Forms.CheckBox();
            this.Ent = new System.Windows.Forms.CheckBox();
            this.IW = new System.Windows.Forms.CheckBox();
            this._Text = new System.Windows.Forms.CheckBox();
            this.NE = new System.Windows.Forms.CheckBox();
            this.NW = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Line
            // 
            this.Line.AutoSize = true;
            this.Line.Location = new System.Drawing.Point(12, 12);
            this.Line.Name = "Line";
            this.Line.Size = new System.Drawing.Size(122, 17);
            this.Line.TabIndex = 0;
            this.Line.Text = "Показывать линии";
            this.Line.UseVisualStyleBackColor = true;
            this.Line.CheckedChanged += new System.EventHandler(this.Line_CheckedChanged);
            // 
            // Rect
            // 
            this.Rect.AutoSize = true;
            this.Rect.Location = new System.Drawing.Point(12, 35);
            this.Rect.Name = "Rect";
            this.Rect.Size = new System.Drawing.Size(176, 17);
            this.Rect.TabIndex = 1;
            this.Rect.Text = "Показывать прямоугольники";
            this.Rect.UseVisualStyleBackColor = true;
            this.Rect.CheckedChanged += new System.EventHandler(this.Rect_CheckedChanged);
            // 
            // Poly
            // 
            this.Poly.AutoSize = true;
            this.Poly.Location = new System.Drawing.Point(12, 58);
            this.Poly.Name = "Poly";
            this.Poly.Size = new System.Drawing.Size(175, 17);
            this.Poly.TabIndex = 2;
            this.Poly.Text = "Показывать многоугольники";
            this.Poly.UseVisualStyleBackColor = true;
            this.Poly.CheckedChanged += new System.EventHandler(this.Poly_CheckedChanged);
            // 
            // Circ
            // 
            this.Circ.AutoSize = true;
            this.Circ.Location = new System.Drawing.Point(12, 81);
            this.Circ.Name = "Circ";
            this.Circ.Size = new System.Drawing.Size(120, 17);
            this.Circ.TabIndex = 3;
            this.Circ.Text = "Показывать круги";
            this.Circ.UseVisualStyleBackColor = true;
            this.Circ.CheckedChanged += new System.EventHandler(this.Circ_CheckedChanged);
            // 
            // Build
            // 
            this.Build.AutoSize = true;
            this.Build.Location = new System.Drawing.Point(12, 104);
            this.Build.Name = "Build";
            this.Build.Size = new System.Drawing.Size(128, 17);
            this.Build.TabIndex = 4;
            this.Build.Text = "Показывать здания";
            this.Build.UseVisualStyleBackColor = true;
            this.Build.CheckedChanged += new System.EventHandler(this.Build_CheckedChanged);
            // 
            // Ent
            // 
            this.Ent.AutoSize = true;
            this.Ent.Location = new System.Drawing.Point(12, 127);
            this.Ent.Name = "Ent";
            this.Ent.Size = new System.Drawing.Size(171, 17);
            this.Ent.TabIndex = 5;
            this.Ent.Text = "Показывать входы в здания";
            this.Ent.UseVisualStyleBackColor = true;
            this.Ent.CheckedChanged += new System.EventHandler(this.Ent_CheckedChanged);
            // 
            // IW
            // 
            this.IW.AutoSize = true;
            this.IW.Location = new System.Drawing.Point(12, 150);
            this.IW.Name = "IW";
            this.IW.Size = new System.Drawing.Size(222, 17);
            this.IW.TabIndex = 6;
            this.IW.Text = "Показывать входы проводов в здания";
            this.IW.UseVisualStyleBackColor = true;
            this.IW.CheckedChanged += new System.EventHandler(this.IW_CheckedChanged);
            // 
            // _Text
            // 
            this._Text.AutoSize = true;
            this._Text.Location = new System.Drawing.Point(12, 173);
            this._Text.Name = "_Text";
            this._Text.Size = new System.Drawing.Size(134, 17);
            this._Text.TabIndex = 7;
            this._Text.Text = "Показывать надписи";
            this._Text.UseVisualStyleBackColor = true;
            this._Text.CheckedChanged += new System.EventHandler(this.Text_CheckedChanged);
            // 
            // NE
            // 
            this.NE.AutoSize = true;
            this.NE.Location = new System.Drawing.Point(12, 196);
            this.NE.Name = "NE";
            this.NE.Size = new System.Drawing.Size(189, 17);
            this.NE.TabIndex = 8;
            this.NE.Text = "Показывать сетевые эелменты";
            this.NE.UseVisualStyleBackColor = true;
            this.NE.CheckedChanged += new System.EventHandler(this.NE_CheckedChanged);
            // 
            // NW
            // 
            this.NW.AutoSize = true;
            this.NW.Location = new System.Drawing.Point(12, 219);
            this.NW.Name = "NW";
            this.NW.Size = new System.Drawing.Size(134, 17);
            this.NW.TabIndex = 9;
            this.NW.Text = "Показывать провода";
            this.NW.UseVisualStyleBackColor = true;
            this.NW.CheckedChanged += new System.EventHandler(this.NW_CheckedChanged);
            // 
            // FiltersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 250);
            this.Controls.Add(this.NW);
            this.Controls.Add(this.NE);
            this.Controls.Add(this._Text);
            this.Controls.Add(this.IW);
            this.Controls.Add(this.Ent);
            this.Controls.Add(this.Build);
            this.Controls.Add(this.Circ);
            this.Controls.Add(this.Poly);
            this.Controls.Add(this.Rect);
            this.Controls.Add(this.Line);
            this.Name = "FiltersForm";
            this.Text = "Фильтры";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FiltersForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox Line;
        private System.Windows.Forms.CheckBox Rect;
        private System.Windows.Forms.CheckBox Poly;
        private System.Windows.Forms.CheckBox Circ;
        private System.Windows.Forms.CheckBox Build;
        private System.Windows.Forms.CheckBox Ent;
        private System.Windows.Forms.CheckBox IW;
        private System.Windows.Forms.CheckBox _Text;
        private System.Windows.Forms.CheckBox NE;
        private System.Windows.Forms.CheckBox NW;
    }
}