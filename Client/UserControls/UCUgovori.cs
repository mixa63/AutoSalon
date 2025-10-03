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
    public partial class UCUgovori : UserControl
    {
        public event Action SearchRequested;
        public event Action<int> DetailsRequested;
        public event Action AddRequested;

        public string KupacEmail => txtKupac.Text;
        public string ProdavacIme => txtProdavac.Text;
        public string MinIznos => txtUgovor.Text;
        public string AutomobilModel => txtAutomobil.Text;
        public DataGridView Grid => dgvUgovori;

        public UCUgovori()
        {
            InitializeComponent();
            btnDetalji.Enabled = false;
            dgvUgovori.SelectionChanged += (s, e) =>
            {
                btnDetalji.Enabled = dgvUgovori.SelectedRows.Count > 0;
            };

            btnPretrazi.Click += btnPretrazi_Click;
            btnDetalji.Click += btnDetalji_Click;
        }


        public int GetSelectedUgovorId()
        {
            if (dgvUgovori.SelectedRows.Count > 0 && dgvUgovori.SelectedRows[0].Cells["IdUgovor"].Value != null)
            {
                return Convert.ToInt32(dgvUgovori.SelectedRows[0].Cells["IdUgovor"].Value);
            }
            return -1;
        }

        private void btnPretrazi_Click(object sender, EventArgs e)
        {
            SearchRequested?.Invoke();
        }

        private void btnDetalji_Click(object sender, EventArgs e)
        {
            var selectedId = GetSelectedUgovorId();
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

        private void btnKreiraj_Click(object sender, EventArgs e)
        {
            AddRequested?.Invoke();
        }
    }
}