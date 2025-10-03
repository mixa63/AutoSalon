using Client.GuiController;
using Client.Forms;
namespace Client
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var frmLogin = new FrmLogin();
            var controller = new LoginController(frmLogin);
            Application.Run(frmLogin);
        }
    }
}