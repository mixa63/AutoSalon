using Common.Communication;
using Common.Domain;
using Client.Forms;
using System.DirectoryServices.ActiveDirectory;

namespace Client.GuiController
{
    public class LoginController
    {
        private FrmLogin frmLogin;

        public LoginController(FrmLogin form)
        {
            frmLogin = form;
            frmLogin.LoginClicked += OnLogin;
        }

        private void OnLogin(string username, string password)
        {
            try
            {
                Communication.Instance.Connect();
                var prodavacRequest = new Prodavac
                {
                    Username = username,
                    Password = password
                };

                var prodavac = Communication.Instance.SendRequest<Prodavac>(Operation.PrijaviProdavac, prodavacRequest);

                if (prodavac == null)
                {
                    throw new Exception("Korisničko ime i šifra nisu ispravni.");
                }
                frmLogin.ShowInfo("Korisničko ime i šifra su ispravni.");
                Session.Instance.Prodavac = prodavac;

                frmLogin.Hide();
                var frmMain = new FrmMain();
                var mainController = new MainController(frmMain);
                frmMain.ShowDialog();
                frmLogin.Close();
            }
            catch (Exception ex)
            {
                frmLogin.ShowError(ex.Message);
                Communication.Instance.Close();
            }
        }
    }
}
