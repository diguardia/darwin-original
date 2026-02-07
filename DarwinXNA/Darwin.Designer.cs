namespace DarwinXNA
{
    partial class Darwin
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
            this.button1 = new System.Windows.Forms.Button();
            this.velocidad = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.velocidad)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(205, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // velocidad
            // 
            this.velocidad.Location = new System.Drawing.Point(12, 12);
            this.velocidad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.velocidad.Name = "velocidad";
            this.velocidad.Size = new System.Drawing.Size(120, 20);
            this.velocidad.TabIndex = 1;
            this.velocidad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.velocidad.ValueChanged += new System.EventHandler(this.velocidad_ValueChanged);
            // 
            // Darwin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.velocidad);
            this.Controls.Add(this.button1);
            this.Name = "Darwin";
            this.Text = "Darwin";
            this.Load += new System.EventHandler(this.Darwin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.velocidad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown velocidad;
    }
}