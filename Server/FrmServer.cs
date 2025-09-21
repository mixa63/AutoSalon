namespace ServerApp
{
    public partial class FrmServer : Form
    {
        private Server server;
        public FrmServer()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.server_icon;
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                server = new Server();

                btnToggle.Text = "Zaustavi server";
                lblStatus.Text = "pokrenut";

                server.Start();

                server.ClientsCountChanged += HandleClientCountChanged;

                return;
            }

            btnToggle.Text = "Pokreni server";
            lblStatus.Text = "zaustavljen";

            server.Stop();

            server = null;
        }

        private void HandleClientCountChanged(int count)
        {
            if (this.IsDisposed) return;

            if (InvokeRequired)
                Invoke(new Action(() =>
                {
                    if (!this.IsDisposed)
                        lblBrojKlijenata.Text = $"{count}";
                }));
            else
                lblBrojKlijenata.Text = $"{count}";
        }

        private void FrmServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (server != null)
            {
                server.ClientsCountChanged -= HandleClientCountChanged;
                server.Stop();
            }

            Application.Exit();
        }
    }
}
