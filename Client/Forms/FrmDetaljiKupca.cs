using Common.Domain;
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
    public partial class FrmDetaljiKupca : Form
    {
        // Public properties for controller access
        public TextBox TxtEmail => txtEmail;
        public RadioButton RbFizicko => rbFizicko;
        public RadioButton RbPravno => rbPravno;
        public Panel PnlFizicko => pnlFizicko;
        public Panel PnlPravno => pnlPravno;

        // Fizicko Lice controls
        public TextBox TxtIme => txtIme;
        public TextBox TxtPrezime => txtPrezime;
        public TextBox TxtJMBG => txtJMBG;
        public TextBox TxtTelefon => txtTelefon;

        // Pravno Lice controls
        public TextBox TxtNaziv => txtNaziv;
        public TextBox TxtPIB => txtPIB;
        public TextBox TxtMaticni => txtMaticni;

        // Buttons
        public Button BtnSacuvaj => btnSacuvaj;
        public Button BtnObrisi => btnObrisi;
        public Button BtnOdustani => btnOdustani;

        public FrmDetaljiKupca()
        {
            InitializeComponent();
            rbFizicko.CheckedChanged += rbLice_CheckedChanged;
            rbPravno.CheckedChanged += rbLice_CheckedChanged;
        }

        private void rbLice_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFizicko.Checked)
            {
                pnlFizicko.Enabled = true;
                pnlFizicko.Visible = true;
                pnlPravno.Visible = false;
                pnlPravno.Enabled = false;
            }
            else if (rbPravno.Checked)
            {
                pnlPravno.Enabled = true;
                pnlPravno.Visible = true;
                pnlFizicko.Visible = false;
                pnlFizicko.Enabled = false;
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