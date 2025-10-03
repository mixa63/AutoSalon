namespace Client.UserControls
{
    partial class UCKvalifikacije
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
            dgvKvalifikacije = new DataGridView();
            txtNaziv = new TextBox();
            cmbStepen = new ComboBox();
            btnDodaj = new Button();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvKvalifikacije).BeginInit();
            SuspendLayout();
            // 
            // dgvKvalifikacije
            // 
            dgvKvalifikacije.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKvalifikacije.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKvalifikacije.Location = new Point(39, 27);
            dgvKvalifikacije.Name = "dgvKvalifikacije";
            dgvKvalifikacije.RowTemplate.Height = 25;
            dgvKvalifikacije.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKvalifikacije.Size = new Size(702, 267);
            dgvKvalifikacije.TabIndex = 0;
            // 
            // txtNaziv
            // 
            txtNaziv.Location = new Point(331, 313);
            txtNaziv.Name = "txtNaziv";
            txtNaziv.Size = new Size(170, 23);
            txtNaziv.TabIndex = 1;
            // 
            // cmbStepen
            // 
            cmbStepen.FormattingEnabled = true;
            cmbStepen.Items.AddRange(new object[] { "Osnovni", "Srednji", "Napredni" });
            cmbStepen.Location = new Point(331, 358);
            cmbStepen.Name = "cmbStepen";
            cmbStepen.Size = new Size(170, 23);
            cmbStepen.TabIndex = 2;
            // 
            // btnDodaj
            // 
            btnDodaj.Location = new Point(383, 406);
            btnDodaj.Name = "btnDodaj";
            btnDodaj.Size = new Size(75, 23);
            btnDodaj.TabIndex = 3;
            btnDodaj.Text = "Dodaj";
            btnDodaj.UseVisualStyleBackColor = true;
            btnDodaj.Click += btnDodaj_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(286, 316);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 4;
            label1.Text = "Naziv:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(279, 361);
            label2.Name = "label2";
            label2.Size = new Size(46, 15);
            label2.TabIndex = 5;
            label2.Text = "Stepen:";
            // 
            // UCKvalifikacije
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnDodaj);
            Controls.Add(cmbStepen);
            Controls.Add(txtNaziv);
            Controls.Add(dgvKvalifikacije);
            Name = "UCKvalifikacije";
            Size = new Size(780, 460);
            ((System.ComponentModel.ISupportInitialize)dgvKvalifikacije).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvKvalifikacije;
        private TextBox txtNaziv;
        private ComboBox cmbStepen;
        private Button btnDodaj;
        private Label label1;
        private Label label2;
    }
}
