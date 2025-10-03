using Common.Domain;

namespace Client.Forms
{
    partial class FrmDetaljiUgovora
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private ComboBox cmbKupac;
        private ComboBox cmbProdavac;
        private DataGridView dgvStavke;
        private Button btnDodajStavku;
        private Button btnObrisiStavku;
        private NumericUpDown numPDV;
        private Button btnSacuvaj;
        private Button btnOdustani;
        private Label lblKupac;
        private Label lblProdavac;
        private Label lblPDV;
        private Label lblStavke;
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
            cmbKupac = new ComboBox();
            cmbProdavac = new ComboBox();
            dgvStavke = new DataGridView();
            btnDodajStavku = new Button();
            btnObrisiStavku = new Button();
            numPDV = new NumericUpDown();
            btnSacuvaj = new Button();
            btnOdustani = new Button();
            lblKupac = new Label();
            lblProdavac = new Label();
            lblPDV = new Label();
            lblStavke = new Label();
            lblDate = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvStavke).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPDV).BeginInit();
            SuspendLayout();
            // 
            // cmbKupac
            // 
            cmbKupac.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKupac.Location = new Point(97, 40);
            cmbKupac.Name = "cmbKupac";
            cmbKupac.Size = new Size(200, 23);
            cmbKupac.TabIndex = 0;
            // 
            // cmbProdavac
            // 
            cmbProdavac.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProdavac.Location = new Point(303, 40);
            cmbProdavac.Name = "cmbProdavac";
            cmbProdavac.Size = new Size(137, 23);
            cmbProdavac.TabIndex = 1;
            // 
            // dgvStavke
            // 
            dgvStavke.AllowUserToAddRows = false;
            dgvStavke.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStavke.Location = new Point(20, 100);
            dgvStavke.Name = "dgvStavke";
            dgvStavke.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStavke.Size = new Size(500, 200);
            dgvStavke.TabIndex = 2;
            // 
            // btnDodajStavku
            // 
            btnDodajStavku.Location = new Point(20, 320);
            btnDodajStavku.Name = "btnDodajStavku";
            btnDodajStavku.Size = new Size(100, 30);
            btnDodajStavku.TabIndex = 3;
            btnDodajStavku.Text = "Dodaj Stavku";
            // 
            // btnObrisiStavku
            // 
            btnObrisiStavku.Location = new Point(140, 320);
            btnObrisiStavku.Name = "btnObrisiStavku";
            btnObrisiStavku.Size = new Size(100, 30);
            btnObrisiStavku.TabIndex = 4;
            btnObrisiStavku.Text = "Obriši Stavku";
            // 
            // numPDV
            // 
            numPDV.DecimalPlaces = 2;
            numPDV.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numPDV.Location = new Point(460, 40);
            numPDV.Name = "numPDV";
            numPDV.Size = new Size(60, 23);
            numPDV.TabIndex = 5;
            // 
            // btnSacuvaj
            // 
            btnSacuvaj.Location = new Point(360, 320);
            btnSacuvaj.Name = "btnSacuvaj";
            btnSacuvaj.Size = new Size(75, 30);
            btnSacuvaj.TabIndex = 6;
            btnSacuvaj.Text = "Sačuvaj";
            // 
            // btnOdustani
            // 
            btnOdustani.Location = new Point(445, 320);
            btnOdustani.Name = "btnOdustani";
            btnOdustani.Size = new Size(75, 30);
            btnOdustani.TabIndex = 7;
            btnOdustani.Text = "Odustani";
            // 
            // lblKupac
            // 
            lblKupac.AutoSize = true;
            lblKupac.Location = new Point(97, 20);
            lblKupac.Name = "lblKupac";
            lblKupac.Size = new Size(43, 15);
            lblKupac.TabIndex = 8;
            lblKupac.Text = "Kupac:";
            // 
            // lblProdavac
            // 
            lblProdavac.AutoSize = true;
            lblProdavac.Location = new Point(303, 20);
            lblProdavac.Name = "lblProdavac";
            lblProdavac.Size = new Size(59, 15);
            lblProdavac.TabIndex = 9;
            lblProdavac.Text = "Prodavac:";
            // 
            // lblPDV
            // 
            lblPDV.AutoSize = true;
            lblPDV.Location = new Point(460, 20);
            lblPDV.Name = "lblPDV";
            lblPDV.Size = new Size(32, 15);
            lblPDV.TabIndex = 10;
            lblPDV.Text = "PDV:";
            // 
            // lblStavke
            // 
            lblStavke.AutoSize = true;
            lblStavke.Location = new Point(20, 80);
            lblStavke.Name = "lblStavke";
            lblStavke.Size = new Size(91, 15);
            lblStavke.TabIndex = 11;
            lblStavke.Text = "Stavke ugovora:";
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Location = new Point(20, 42);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(0, 15);
            lblDate.TabIndex = 12;
            // 
            // FrmDetaljiUgovora
            // 
            ClientSize = new Size(550, 370);
            Controls.Add(lblDate);
            Controls.Add(cmbKupac);
            Controls.Add(cmbProdavac);
            Controls.Add(dgvStavke);
            Controls.Add(btnDodajStavku);
            Controls.Add(btnObrisiStavku);
            Controls.Add(numPDV);
            Controls.Add(btnSacuvaj);
            Controls.Add(btnOdustani);
            Controls.Add(lblKupac);
            Controls.Add(lblProdavac);
            Controls.Add(lblPDV);
            Controls.Add(lblStavke);
            Name = "FrmDetaljiUgovora";
            Text = "Detalji";
            ((System.ComponentModel.ISupportInitialize)dgvStavke).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPDV).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDate;
    }
}