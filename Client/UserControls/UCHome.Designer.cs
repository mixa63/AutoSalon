namespace Client.UserControls
{
    partial class UCHome
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
            pictureBox1 = new PictureBox();
            lblDobrodosli = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.client_photo;
            pictureBox1.Location = new Point(47, 81);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(283, 250);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblDobrodosli
            // 
            lblDobrodosli.AutoSize = true;
            lblDobrodosli.BackColor = Color.White;
            lblDobrodosli.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblDobrodosli.ImageAlign = ContentAlignment.MiddleLeft;
            lblDobrodosli.Location = new Point(359, 141);
            lblDobrodosli.Name = "lblDobrodosli";
            lblDobrodosli.Size = new Size(114, 25);
            lblDobrodosli.TabIndex = 11;
            lblDobrodosli.Text = "Dobro došli,";
            lblDobrodosli.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.BackColor = Color.White;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(64, 64, 64);
            label1.Location = new Point(359, 166);
            label1.Name = "label1";
            label1.Size = new Size(290, 155);
            label1.TabIndex = 13;
            label1.Text = "Spremni za još jedan uspešan dan? Aplikacija je tu da vam olakša rad i pomogne da svaki klijent bude zadovoljan. Vaša kontrolna tabla čeka. Kreirajmo zajedno sjajne rezultate.";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // UCHome
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(label1);
            Controls.Add(lblDobrodosli);
            Controls.Add(pictureBox1);
            Name = "UCHome";
            Size = new Size(686, 437);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label lblDobrodosli;
        private Label label1;
    }
}
