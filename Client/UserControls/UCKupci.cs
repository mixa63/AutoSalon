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
    public partial class UCKupci : UserControl
    {
        public event Action SearchRequested;
        public event Action<int> DetailsRequested;
        public event Action AddRequested;
        public string KupacEmail => txtKupac.Text;
        public DataGridView Grid => dgvKupci;
        public UCKupci()
        {

            InitializeComponent();
            btnDetalji.Enabled = false;
            dgvKupci.SelectionChanged += (s, e) =>
            {
                btnDetalji.Enabled = dgvKupci.SelectedRows.Count > 0;
            };

        }
        public int GetSelectedKupacId()
        {
            if (dgvKupci.SelectedRows.Count > 0 && dgvKupci.SelectedRows[0].Cells["IdKupac"].Value != null)
            {
                return Convert.ToInt32(dgvKupci.SelectedRows[0].Cells["IdKupac"].Value);
            }
            return -1;
        }

        private void btnPretrazi_Click(object sender, EventArgs e)
        {
            SearchRequested?.Invoke();
        }

        private void btnKreiraj_Click(object sender, EventArgs e)
        {
            AddRequested?.Invoke();
        }

        private void btnDetalji_Click(object sender, EventArgs e)
        {
            var selectedId = GetSelectedKupacId();
            if (selectedId > 0)
            {
                DetailsRequested?.Invoke(selectedId);
            }
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
