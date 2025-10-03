using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.UserControls
{
    public partial class UCKvalifikacije : UserControl
    {
        public event Action AddRequested;
        public string Naziv => txtNaziv.Text;
        public DataGridView Grid => dgvKvalifikacije;
        public string Stepen => cmbStepen.SelectedItem?.ToString();
        public UCKvalifikacije()
        {
            InitializeComponent();
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowInfo(string message)
        {
            MessageBox.Show(message, "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            AddRequested?.Invoke();
        }
    }
}
