using Client.UserControls;

namespace Client.GuiController
{
    public class MainController
    {
        private FrmMain frmMain;
        private UgovoriController ugovoriController;
        private KupciController kupciController;
        private KvalifikacijeController kvalifikacijeController;
        public MainController(FrmMain form)
        {
            frmMain = form;
            frmMain.HomeClicked += OnHome;
            frmMain.UgovoriClicked += OnUgovori;
            frmMain.KupciClicked += OnKupci;
            frmMain.KvalifikacijeClicked += OnKvalifikacije;
            Init();
        }

        private void Init()
        {
            var ucHome = new UCHome();
            ucHome.SetWelcomeMessage(
                $"Dobrodošli, {Session.Instance.Prodavac?.Ime} {Session.Instance.Prodavac?.Prezime}!");
            frmMain.SetMainContent(ucHome);
        }

        private void OnHome()
        {
            Init();
        }
        private void OnUgovori()
        {
            ugovoriController ??= new UgovoriController(frmMain);
            ugovoriController.Show();
        }
        private void OnKupci()
        {
            kupciController ??= new KupciController(frmMain);
            kupciController.Show();
        }
        private void OnKvalifikacije()
        {
            kvalifikacijeController ??= new KvalifikacijeController(frmMain);
            kvalifikacijeController.Show();
        }
    }
}
