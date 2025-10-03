using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Domain;

namespace Client.Forms
{
    public partial class FrmDetaljiUgovora : Form
    {
        public ComboBox ComboKupac => cmbKupac;
        public ComboBox ComboProdavac => cmbProdavac;
        public DataGridView GridStavke => dgvStavke;
        public NumericUpDown NumericPDV => numPDV;
        public Button ButtonDodajStavku => btnDodajStavku;
        public Button ButtonObrisiStavku => btnObrisiStavku;
        public Button ButtonSacuvaj => btnSacuvaj;
        public Button ButtonOdustani => btnOdustani;

        public Label LabelDatum => lblDate;

        public FrmDetaljiUgovora()
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

    }
}
