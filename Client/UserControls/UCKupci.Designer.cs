namespace Client.UserControls
{
    partial class UCKupci
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnKreiraj = new Button();
            dgvKupci = new DataGridView();
            txtKupac = new TextBox();
            btnPretrazi = new Button();
            btnDetalji = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvKupci).BeginInit();
            SuspendLayout();
            // 
            // btnKreiraj
            // 
            btnKreiraj.Location = new Point(591, 424);
            btnKreiraj.Name = "btnKreiraj";
            btnKreiraj.Size = new Size(75, 23);
            btnKreiraj.TabIndex = 23;
            btnKreiraj.Text = "Kreiraj";
            btnKreiraj.UseVisualStyleBackColor = true;
            btnKreiraj.Click += btnKreiraj_Click;
            // 
            // dgvKupci
            // 
            dgvKupci.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKupci.Location = new Point(18, 104);
            dgvKupci.Name = "dgvKupci";
            dgvKupci.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKupci.Size = new Size(740, 300);
            dgvKupci.TabIndex = 12;
            // 
            // txtKupac
            // 
            txtKupac.Location = new Point(18, 34);
            txtKupac.Name = "txtKupac";
            txtKupac.Size = new Size(150, 23);
            txtKupac.TabIndex = 13;
            // 
            // btnPretrazi
            // 
            btnPretrazi.Location = new Point(683, 34);
            btnPretrazi.Name = "btnPretrazi";
            btnPretrazi.Size = new Size(75, 23);
            btnPretrazi.TabIndex = 17;
            btnPretrazi.Text = "Pretraži";
            btnPretrazi.UseVisualStyleBackColor = true;
            btnPretrazi.Click += btnPretrazi_Click;
            // 
            // btnDetalji
            // 
            btnDetalji.Enabled = false;
            btnDetalji.Location = new Point(683, 424);
            btnDetalji.Name = "btnDetalji";
            btnDetalji.Size = new Size(75, 23);
            btnDetalji.TabIndex = 18;
            btnDetalji.Text = "Detalji";
            btnDetalji.UseVisualStyleBackColor = true;
            btnDetalji.Click += btnDetalji_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 14);
            label1.Name = "label1";
            label1.Size = new Size(74, 15);
            label1.TabIndex = 19;
            label1.Text = "Email kupca:";
            // 
            // UCKupci
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnKreiraj);
            Controls.Add(dgvKupci);
            Controls.Add(txtKupac);
            Controls.Add(btnPretrazi);
            Controls.Add(btnDetalji);
            Controls.Add(label1);
            Name = "UCKupci";
            Size = new Size(780, 460);
            ((System.ComponentModel.ISupportInitialize)dgvKupci).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnKreiraj;
        private DataGridView dgvKupci;
        private TextBox txtKupac;
        private Button btnPretrazi;
        private Button btnDetalji;
        private Label label1;
    }
}
