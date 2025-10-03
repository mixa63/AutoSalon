namespace Client.Forms
{
    partial class FrmDetaljiKupca
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
            btnSacuvaj = new Button();
            btnOdustani = new Button();
            lblKupac = new Label();
            btnObrisi = new Button();
            txtEmail = new TextBox();
            rbPravno = new RadioButton();
            rbFizicko = new RadioButton();
            pnlFizicko = new Panel();
            lblTelefon = new Label();
            lblJMBG = new Label();
            lblPrezime = new Label();
            lblIme = new Label();
            txtTelefon = new TextBox();
            txtJMBG = new TextBox();
            txtPrezime = new TextBox();
            txtIme = new TextBox();
            pnlPravno = new Panel();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtMaticni = new TextBox();
            txtPIB = new TextBox();
            txtNaziv = new TextBox();
            pnlFizicko.SuspendLayout();
            pnlPravno.SuspendLayout();
            SuspendLayout();
            // 
            // btnSacuvaj
            // 
            btnSacuvaj.Location = new Point(369, 320);
            btnSacuvaj.Name = "btnSacuvaj";
            btnSacuvaj.Size = new Size(75, 30);
            btnSacuvaj.TabIndex = 19;
            btnSacuvaj.Text = "Sačuvaj";
            // 
            // btnOdustani
            // 
            btnOdustani.Location = new Point(450, 320);
            btnOdustani.Name = "btnOdustani";
            btnOdustani.Size = new Size(75, 30);
            btnOdustani.TabIndex = 20;
            btnOdustani.Text = "Odustani";
            // 
            // lblKupac
            // 
            lblKupac.AutoSize = true;
            lblKupac.Location = new Point(192, 18);
            lblKupac.Name = "lblKupac";
            lblKupac.Size = new Size(39, 15);
            lblKupac.TabIndex = 21;
            lblKupac.Text = "Email:";
            // 
            // btnObrisi
            // 
            btnObrisi.Location = new Point(263, 320);
            btnObrisi.Name = "btnObrisi";
            btnObrisi.Size = new Size(100, 30);
            btnObrisi.TabIndex = 17;
            btnObrisi.Text = "Obriši kupca";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(239, 12);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(120, 23);
            txtEmail.TabIndex = 22;
            // 
            // rbPravno
            // 
            rbPravno.AutoSize = true;
            rbPravno.Location = new Point(286, 60);
            rbPravno.Name = "rbPravno";
            rbPravno.Size = new Size(83, 19);
            rbPravno.TabIndex = 23;
            rbPravno.TabStop = true;
            rbPravno.Text = "Pravno lice";
            rbPravno.UseVisualStyleBackColor = true;
            // 
            // rbFizicko
            // 
            rbFizicko.AutoSize = true;
            rbFizicko.Location = new Point(181, 60);
            rbFizicko.Name = "rbFizicko";
            rbFizicko.Size = new Size(82, 19);
            rbFizicko.TabIndex = 24;
            rbFizicko.TabStop = true;
            rbFizicko.Text = "Fizičko lice";
            rbFizicko.UseVisualStyleBackColor = true;
            // 
            // pnlFizicko
            // 
            pnlFizicko.Controls.Add(lblTelefon);
            pnlFizicko.Controls.Add(lblJMBG);
            pnlFizicko.Controls.Add(lblPrezime);
            pnlFizicko.Controls.Add(lblIme);
            pnlFizicko.Controls.Add(txtTelefon);
            pnlFizicko.Controls.Add(txtJMBG);
            pnlFizicko.Controls.Add(txtPrezime);
            pnlFizicko.Controls.Add(txtIme);
            pnlFizicko.Location = new Point(56, 85);
            pnlFizicko.Name = "pnlFizicko";
            pnlFizicko.Size = new Size(439, 220);
            pnlFizicko.TabIndex = 25;
            // 
            // lblTelefon
            // 
            lblTelefon.AutoSize = true;
            lblTelefon.Location = new Point(129, 162);
            lblTelefon.Name = "lblTelefon";
            lblTelefon.Size = new Size(48, 15);
            lblTelefon.TabIndex = 15;
            lblTelefon.Text = "Telefon:";
            // 
            // lblJMBG
            // 
            lblJMBG.AutoSize = true;
            lblJMBG.Location = new Point(137, 122);
            lblJMBG.Name = "lblJMBG";
            lblJMBG.Size = new Size(40, 15);
            lblJMBG.TabIndex = 14;
            lblJMBG.Text = "JMBG:";
            // 
            // lblPrezime
            // 
            lblPrezime.AutoSize = true;
            lblPrezime.Location = new Point(125, 82);
            lblPrezime.Name = "lblPrezime";
            lblPrezime.Size = new Size(52, 15);
            lblPrezime.TabIndex = 13;
            lblPrezime.Text = "Prezime:";
            // 
            // lblIme
            // 
            lblIme.AutoSize = true;
            lblIme.Location = new Point(147, 42);
            lblIme.Name = "lblIme";
            lblIme.Size = new Size(30, 15);
            lblIme.TabIndex = 12;
            lblIme.Text = "Ime:";
            // 
            // txtTelefon
            // 
            txtTelefon.Location = new Point(183, 159);
            txtTelefon.Name = "txtTelefon";
            txtTelefon.Size = new Size(131, 23);
            txtTelefon.TabIndex = 11;
            // 
            // txtJMBG
            // 
            txtJMBG.Location = new Point(183, 119);
            txtJMBG.Name = "txtJMBG";
            txtJMBG.Size = new Size(131, 23);
            txtJMBG.TabIndex = 10;
            // 
            // txtPrezime
            // 
            txtPrezime.Location = new Point(183, 79);
            txtPrezime.Name = "txtPrezime";
            txtPrezime.Size = new Size(131, 23);
            txtPrezime.TabIndex = 9;
            // 
            // txtIme
            // 
            txtIme.Location = new Point(183, 39);
            txtIme.Name = "txtIme";
            txtIme.Size = new Size(131, 23);
            txtIme.TabIndex = 8;
            // 
            // pnlPravno
            // 
            pnlPravno.Controls.Add(label2);
            pnlPravno.Controls.Add(label3);
            pnlPravno.Controls.Add(label4);
            pnlPravno.Controls.Add(txtMaticni);
            pnlPravno.Controls.Add(txtPIB);
            pnlPravno.Controls.Add(txtNaziv);
            pnlPravno.Location = new Point(56, 85);
            pnlPravno.Name = "pnlPravno";
            pnlPravno.Size = new Size(439, 220);
            pnlPravno.TabIndex = 26;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(107, 122);
            label2.Name = "label2";
            label2.Size = new Size(74, 15);
            label2.TabIndex = 14;
            label2.Text = "Matični broj:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(154, 82);
            label3.Name = "label3";
            label3.Size = new Size(27, 15);
            label3.TabIndex = 13;
            label3.Text = "PIB:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(111, 42);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
            label4.TabIndex = 12;
            label4.Text = "Naziv firme:";
            // 
            // txtMaticni
            // 
            txtMaticni.Location = new Point(183, 119);
            txtMaticni.Name = "txtMaticni";
            txtMaticni.Size = new Size(131, 23);
            txtMaticni.TabIndex = 10;
            // 
            // txtPIB
            // 
            txtPIB.Location = new Point(183, 79);
            txtPIB.Name = "txtPIB";
            txtPIB.Size = new Size(131, 23);
            txtPIB.TabIndex = 9;
            // 
            // txtNaziv
            // 
            txtNaziv.Location = new Point(183, 39);
            txtNaziv.Name = "txtNaziv";
            txtNaziv.Size = new Size(131, 23);
            txtNaziv.TabIndex = 8;
            // 
            // FrmDetaljiKupca
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            ClientSize = new Size(550, 370);
            Controls.Add(pnlPravno);
            Controls.Add(pnlFizicko);
            Controls.Add(rbFizicko);
            Controls.Add(rbPravno);
            Controls.Add(txtEmail);
            Controls.Add(btnObrisi);
            Controls.Add(btnSacuvaj);
            Controls.Add(btnOdustani);
            Controls.Add(lblKupac);
            Name = "FrmDetaljiKupca";
            Text = "Detalji";
            pnlFizicko.ResumeLayout(false);
            pnlFizicko.PerformLayout();
            pnlPravno.ResumeLayout(false);
            pnlPravno.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSacuvaj;
        private Button btnOdustani;
        private Label lblKupac;
        private Button btnObrisi;
        private TextBox txtEmail;
        private RadioButton rbPravno;
        private RadioButton rbFizicko;
        private Panel pnlFizicko;
        private Label lblTelefon;
        private Label lblJMBG;
        private Label lblPrezime;
        private Label lblIme;
        private TextBox txtTelefon;
        private TextBox txtJMBG;
        private TextBox txtPrezime;
        private TextBox txtIme;
        private Panel pnlPravno;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtMaticni;
        private TextBox txtPIB;
        private TextBox txtNaziv;
    }
}