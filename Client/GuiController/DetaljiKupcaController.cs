using Client.Forms;
using Common.Communication;
using Common.Domain;
using System;
using System.Windows.Forms;

namespace Client.GuiController
{
    internal class DetaljiKupcaController
    {
        private readonly Kupac originalniKupac;
        private readonly FrmDetaljiKupca forma;

        public DetaljiKupcaController(Kupac k)
        {
            this.originalniKupac = k;
            forma = new FrmDetaljiKupca();

            InicijalizujFormu();
            PoveziEvente();

            forma.ShowDialog();
        }

        private void InicijalizujFormu()
        {
            // Mod za prikaz detalja - Fizičko lice
            if (originalniKupac is FizickoLice fl)
            {
                forma.Text = "Detalji - Fizičko lice";
                forma.RbFizicko.Checked = true;
                forma.RbPravno.Enabled = false;
                forma.RbFizicko.Enabled = false;

                // Pristup emailu preko ugnježdenog Kupac objekta
                forma.TxtEmail.Text = fl.Email;
                forma.TxtIme.Text = fl.Ime;
                forma.TxtPrezime.Text = fl.Prezime;
                forma.TxtJMBG.Text = fl.JMBG;
                forma.TxtTelefon.Text = fl.Telefon;
            }
            // Mod za prikaz detalja - Pravno lice
            else if (originalniKupac is PravnoLice pl)
            {
                forma.Text = "Detalji - Pravno lice";
                forma.RbPravno.Checked = true;
                forma.RbPravno.Enabled = false;
                forma.RbFizicko.Enabled = false;

                // Pristup emailu preko ugnježdenog Kupac objekta
                forma.TxtEmail.Text = pl.Email;
                forma.TxtNaziv.Text = pl.NazivFirme;
                forma.TxtPIB.Text = pl.PIB;
                forma.TxtMaticni.Text = pl.MaticniBroj;
            }
            // Mod za kreiranje novog kupca (primljen je samo osnovni Kupac objekat sa ID-jem)
            else
            {
                forma.Text = "Kreiraj kupca";
                forma.TxtEmail.Text = originalniKupac.Email;
                forma.RbFizicko.Checked = true;
                forma.BtnObrisi.Enabled = false;
                forma.BtnOdustani.Click += (s, e) => Communication.Instance.SendRequest<bool>(Operation.ObrisiKupac, originalniKupac);
            }
        }

        private void PoveziEvente()
        {
            forma.BtnSacuvaj.Click += SacuvajKupca;
            forma.BtnObrisi.Click += ObrisiKupca;
            forma.BtnOdustani.Click += (s, e) => forma.Close();
        }

        private void ObrisiKupca(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Da li ste sigurni da želite da obrišete kupca?", "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No) return;

            try
            {
                bool uspesno = Communication.Instance.SendRequest<bool>(Operation.ObrisiKupac, originalniKupac);
                if (uspesno)
                {
                    forma.ShowInfo("Sistem je obrisao kupca.");
                    forma.Close();
                }
                else
                {
                    forma.ShowError("Sistem ne može da obriše kupca.");
                }
            }
            catch (Exception ex)
            {
                forma.ShowError($"Greška pri brisanju: {ex.Message}");
            }
        }

        private void SacuvajKupca(object sender, EventArgs e)
        {
            if (!ValidirajUnos()) return;

            try
            {
                int idKupca = ((Kupac)originalniKupac).IdKupac;

                

                Kupac kupacZaSlanje;
                if (forma.RbFizicko.Checked)
                {
                    kupacZaSlanje = new FizickoLice
                    {
                        IdKupac = idKupca,
                        Email = forma.TxtEmail.Text,
                        Ime = forma.TxtIme.Text,
                        Prezime = forma.TxtPrezime.Text,
                        JMBG = forma.TxtJMBG.Text,
                        Telefon = forma.TxtTelefon.Text
                    };
                }
                else
                {
                    if (!int.TryParse(forma.TxtPIB.Text, out int pibValue))
                    {
                        forma.ShowError("PIB mora biti validan broj.");
                        return;
                    }
                    kupacZaSlanje = new PravnoLice
                    {
                        IdKupac = idKupca,
                        Email = forma.TxtEmail.Text,
                        NazivFirme = forma.TxtNaziv.Text,
                        PIB = forma.TxtPIB.Text,
                        MaticniBroj = forma.TxtMaticni.Text
                    };
                }

                bool uspesno = Communication.Instance.SendRequest<bool>(Operation.PromeniKupac, kupacZaSlanje);

                if (uspesno)
                {
                    forma.ShowInfo("Sistem je zapamtio kupca.");
                    forma.Close();
                }
                else
                {
                    forma.ShowError("Sistem ne može da zapamti kupca.");
                }
            }
            catch (Exception ex)
            {
                forma.ShowError($"Greška: {ex.Message}");
            }
        }

        private bool ValidirajUnos()
        {
            if (string.IsNullOrWhiteSpace(forma.TxtEmail.Text))
            {
                forma.ShowError("Email je obavezno polje.");
                return false;
            }

            if (forma.RbFizicko.Checked)
            {
                if (string.IsNullOrWhiteSpace(forma.TxtIme.Text)) { forma.ShowError("Ime je obavezno polje."); return false; }
                if (string.IsNullOrWhiteSpace(forma.TxtPrezime.Text)) { forma.ShowError("Prezime je obavezno polje."); return false; }
                if (string.IsNullOrWhiteSpace(forma.TxtJMBG.Text)) { forma.ShowError("JMBG je obavezno polje."); return false; }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(forma.TxtNaziv.Text)) { forma.ShowError("Naziv firme je obavezno polje."); return false; }
                if (string.IsNullOrWhiteSpace(forma.TxtPIB.Text)) { forma.ShowError("PIB je obavezno polje."); return false; }
                if (string.IsNullOrWhiteSpace(forma.TxtMaticni.Text)) { forma.ShowError("Matični broj je obavezno polje."); return false; }
            }

            return true;
        }
    }
}