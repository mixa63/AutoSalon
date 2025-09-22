namespace ServerApp
{
    partial class FrmServer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblNaslov = new Label();
            pbServer = new PictureBox();
            btnToggle = new Button();
            label1 = new Label();
            lblBrojKlijenata = new Label();
            label2 = new Label();
            lblStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)pbServer).BeginInit();
            SuspendLayout();
            // 
            // lblNaslov
            // 
            lblNaslov.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            lblNaslov.Location = new Point(226, 9);
            lblNaslov.Name = "lblNaslov";
            lblNaslov.Size = new Size(316, 115);
            lblNaslov.TabIndex = 0;
            lblNaslov.Text = "Sistem za praćenje prodaje automobila";
            lblNaslov.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pbServer
            // 
            pbServer.Image = Properties.Resources.server_photo;
            pbServer.InitialImage = null;
            pbServer.Location = new Point(90, 127);
            pbServer.Name = "pbServer";
            pbServer.Size = new Size(589, 245);
            pbServer.SizeMode = PictureBoxSizeMode.Zoom;
            pbServer.TabIndex = 1;
            pbServer.TabStop = false;
            // 
            // btnToggle
            // 
            btnToggle.BackColor = Color.Gainsboro;
            btnToggle.Cursor = Cursors.Hand;
            btnToggle.FlatStyle = FlatStyle.Flat;
            btnToggle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnToggle.Location = new Point(316, 391);
            btnToggle.Name = "btnToggle";
            btnToggle.Size = new Size(136, 33);
            btnToggle.TabIndex = 2;
            btnToggle.Text = "Pokreni server";
            btnToggle.UseVisualStyleBackColor = false;
            btnToggle.Click += btnToggle_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(13, 467);
            label1.Name = "label1";
            label1.Size = new Size(142, 20);
            label1.TabIndex = 3;
            label1.Text = "Povezano klijenata -";
            // 
            // lblBrojKlijenata
            // 
            lblBrojKlijenata.AutoSize = true;
            lblBrojKlijenata.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblBrojKlijenata.Location = new Point(150, 467);
            lblBrojKlijenata.Name = "lblBrojKlijenata";
            lblBrojKlijenata.Size = new Size(17, 20);
            lblBrojKlijenata.TabIndex = 4;
            lblBrojKlijenata.Text = "0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(571, 467);
            label2.Name = "label2";
            label2.Size = new Size(114, 20);
            label2.TabIndex = 5;
            label2.Text = "Status servera - ";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatus.Location = new Point(678, 467);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(82, 20);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "zaustavljen";
            // 
            // FrmServer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(798, 516);
            Controls.Add(lblStatus);
            Controls.Add(label2);
            Controls.Add(lblBrojKlijenata);
            Controls.Add(label1);
            Controls.Add(btnToggle);
            Controls.Add(pbServer);
            Controls.Add(lblNaslov);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FrmServer";
            Text = "Server";
            FormClosed += FrmServer_FormClosed;
            ((System.ComponentModel.ISupportInitialize)pbServer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNaslov;
        private PictureBox pbServer;
        private Button btnToggle;
        private Label label1;
        private Label lblBrojKlijenata;
        private Label label2;
        private Label lblStatus;
    }
}
