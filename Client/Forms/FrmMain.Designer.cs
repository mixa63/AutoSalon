namespace Client
{
    partial class FrmMain
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
            menuStrip1 = new MenuStrip();
            homeToolStripMenuItem = new ToolStripMenuItem();
            ugovoriToolStripMenuItem = new ToolStripMenuItem();
            kupciToolStripMenuItem = new ToolStripMenuItem();
            kvalifikacijeToolStripMenuItem = new ToolStripMenuItem();
            pnlMain = new Panel();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.ActiveCaption;
            menuStrip1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            menuStrip1.Items.AddRange(new ToolStripItem[] { homeToolStripMenuItem, ugovoriToolStripMenuItem, kupciToolStripMenuItem, kvalifikacijeToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(784, 29);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // homeToolStripMenuItem
            // 
            homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            homeToolStripMenuItem.Size = new Size(84, 25);
            homeToolStripMenuItem.Text = "Početna";
            // 
            // ugovoriToolStripMenuItem
            // 
            ugovoriToolStripMenuItem.Name = "ugovoriToolStripMenuItem";
            ugovoriToolStripMenuItem.Size = new Size(84, 25);
            ugovoriToolStripMenuItem.Text = "Ugovori";
            // 
            // kupciToolStripMenuItem
            // 
            kupciToolStripMenuItem.Name = "kupciToolStripMenuItem";
            kupciToolStripMenuItem.Size = new Size(65, 25);
            kupciToolStripMenuItem.Text = "Kupci";
            // 
            // kvalifikacijeToolStripMenuItem
            // 
            kvalifikacijeToolStripMenuItem.Name = "kvalifikacijeToolStripMenuItem";
            kvalifikacijeToolStripMenuItem.Size = new Size(115, 25);
            kvalifikacijeToolStripMenuItem.Text = "Kvalifikacije";
            // 
            // pnlMain
            // 
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 29);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(784, 532);
            pnlMain.TabIndex = 1;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(784, 561);
            Controls.Add(pnlMain);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            Name = "FrmMain";
            Text = "Auto salon";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem homeToolStripMenuItem;
        private Panel pnlMain;
        private ToolStripMenuItem ugovoriToolStripMenuItem;
        private ToolStripMenuItem kupciToolStripMenuItem;
        private ToolStripMenuItem kvalifikacijeToolStripMenuItem;
    }
}
