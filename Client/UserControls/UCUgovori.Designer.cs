namespace Client.UserControls
{
    partial class UCUgovori
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private DataGridView dgvUgovori;
        private TextBox txtKupac;
        private TextBox txtProdavac;
        private TextBox txtUgovor;
        private TextBox txtAutomobil;
        private Button btnPretrazi;
        private Button btnDetalji;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;

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
            dgvUgovori = new DataGridView();
            txtKupac = new TextBox();
            txtProdavac = new TextBox();
            txtUgovor = new TextBox();
            txtAutomobil = new TextBox();
            btnPretrazi = new Button();
            btnDetalji = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            btnKreiraj = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvUgovori).BeginInit();
            SuspendLayout();
            // 
            // dgvUgovori
            // 
            dgvUgovori.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUgovori.Location = new Point(20, 100);
            dgvUgovori.Name = "dgvUgovori";
            dgvUgovori.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUgovori.Size = new Size(740, 300);
            dgvUgovori.TabIndex = 0;
            // 
            // txtKupac
            // 
            txtKupac.Location = new Point(20, 30);
            txtKupac.Name = "txtKupac";
            txtKupac.Size = new Size(150, 23);
            txtKupac.TabIndex = 1;
            // 
            // txtProdavac
            // 
            txtProdavac.Location = new Point(190, 30);
            txtProdavac.Name = "txtProdavac";
            txtProdavac.Size = new Size(150, 23);
            txtProdavac.TabIndex = 2;
            // 
            // txtUgovor
            // 
            txtUgovor.Location = new Point(360, 30);
            txtUgovor.Name = "txtUgovor";
            txtUgovor.Size = new Size(150, 23);
            txtUgovor.TabIndex = 3;
            // 
            // txtAutomobil
            // 
            txtAutomobil.Location = new Point(530, 30);
            txtAutomobil.Name = "txtAutomobil";
            txtAutomobil.Size = new Size(150, 23);
            txtAutomobil.TabIndex = 4;
            // 
            // btnPretrazi
            // 
            btnPretrazi.Location = new Point(690, 30);
            btnPretrazi.Name = "btnPretrazi";
            btnPretrazi.Size = new Size(75, 23);
            btnPretrazi.TabIndex = 5;
            btnPretrazi.Text = "Pretraži";
            btnPretrazi.UseVisualStyleBackColor = true;
            // 
            // btnDetalji
            // 
            btnDetalji.Enabled = false;
            btnDetalji.Location = new Point(685, 420);
            btnDetalji.Name = "btnDetalji";
            btnDetalji.Size = new Size(75, 23);
            btnDetalji.TabIndex = 6;
            btnDetalji.Text = "Detalji";
            btnDetalji.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 10);
            label1.Name = "label1";
            label1.Size = new Size(74, 15);
            label1.TabIndex = 7;
            label1.Text = "Email kupca:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(190, 10);
            label2.Name = "label2";
            label2.Size = new Size(82, 15);
            label2.TabIndex = 8;
            label2.Text = "Ime prodavca:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(360, 10);
            label3.Name = "label3";
            label3.Size = new Size(152, 15);
            label3.TabIndex = 9;
            label3.Text = "Max. iznos ugovora sa PDV:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(530, 10);
            label4.Name = "label4";
            label4.Size = new Size(108, 15);
            label4.TabIndex = 10;
            label4.Text = "Model automobila:";
            // 
            // btnKreiraj
            // 
            btnKreiraj.Location = new Point(593, 420);
            btnKreiraj.Name = "btnKreiraj";
            btnKreiraj.Size = new Size(75, 23);
            btnKreiraj.TabIndex = 11;
            btnKreiraj.Text = "Kreiraj";
            btnKreiraj.UseVisualStyleBackColor = true;
            btnKreiraj.Click += btnKreiraj_Click;
            // 
            // UCUgovori
            // 
            Controls.Add(btnKreiraj);
            Controls.Add(dgvUgovori);
            Controls.Add(txtKupac);
            Controls.Add(txtProdavac);
            Controls.Add(txtUgovor);
            Controls.Add(txtAutomobil);
            Controls.Add(btnPretrazi);
            Controls.Add(btnDetalji);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(label4);
            Name = "UCUgovori";
            Size = new Size(780, 460);
            ((System.ComponentModel.ISupportInitialize)dgvUgovori).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnKreiraj;
    }
}
