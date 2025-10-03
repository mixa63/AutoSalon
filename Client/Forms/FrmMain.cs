using Client.UserControls;

namespace Client
{
    public partial class FrmMain : Form
    {
        public event Action HomeClicked;
        public event Action UgovoriClicked;
        public event Action KupciClicked;
        public event Action KvalifikacijeClicked;

        public FrmMain()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.client_icon;
            homeToolStripMenuItem.Click += (s, e) => HomeClicked?.Invoke();
            ugovoriToolStripMenuItem.Click += (s, e) => UgovoriClicked?.Invoke();
            kupciToolStripMenuItem.Click += (s, e) => KupciClicked?.Invoke();
            kvalifikacijeToolStripMenuItem.Click += (s, e) => KvalifikacijeClicked?.Invoke();
        }
        
        public void SetMainContent(UserControl uc)
        {
            pnlMain.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(uc);
        }
    }
}
