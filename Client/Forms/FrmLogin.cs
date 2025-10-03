using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class FrmLogin : Form
    {
        public event Action<string, string> LoginClicked;
        public FrmLogin()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.client_icon;
            btnLogin.Click += (s, e) => LoginClicked?.Invoke(txtUsername.Text, txtPassword.Text);
        }
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowInfo(string message)
        {
            MessageBox.Show(message, "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
