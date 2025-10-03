using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Common.Communication;
using Common.Domain;
using Client.Forms;

namespace Client.GuiController
{
    internal class DetaljiUgovoraController
    {
        private readonly FrmDetaljiUgovora forma;
        private readonly Ugovor originalUgovor;

        private List<Kupac> sviKupci;
        private List<Prodavac> sviProdavci;
        private List<Automobil> sviAutomobili;

        public DetaljiUgovoraController(Ugovor ugovor)
        {
            forma = new FrmDetaljiUgovora();
            originalUgovor = ugovor;

            UcitajSvePodatke();
            InicijalizujFormu();
            PoveziEvente();

            forma.ShowDialog();
        }

        private void UcitajSvePodatke()
        {
            try
            {
                sviKupci = Communication.Instance.SendRequest<List<Kupac>>(Operation.VratiListuSviKupac, null);
                sviProdavci = Communication.Instance.SendRequest<List<Prodavac>>(Operation.VratiListuSviProdavac, null);
                sviAutomobili = Communication.Instance.SendRequest<List<Automobil>>(Operation.VratiListuSviAutomobil, null);
            }
            catch (Exception ex)
            {
                forma.ShowError($"Greška pri učitavanju podataka: {ex.Message}");
            }
        }

        private void InicijalizujFormu()
        {
            // Popuni ComboBox-ove
            forma.ComboKupac.DataSource = sviKupci;
            forma.ComboKupac.DisplayMember = "Email";
            forma.ComboKupac.ValueMember = "IdKupac";
            forma.ComboKupac.SelectedValue = originalUgovor.Kupac?.IdKupac ?? -1;

            forma.ComboProdavac.DataSource = sviProdavci;
            forma.ComboProdavac.DisplayMember = "Ime";
            forma.ComboProdavac.ValueMember = "IdProdavac";
            forma.ComboProdavac.SelectedValue = originalUgovor.Prodavac?.IdProdavac ?? -1;

            // Postavi PDV
            forma.NumericPDV.Value = (decimal)originalUgovor.PDV;

            // Inicijalizuj DataGridView kolone
            forma.GridStavke.Columns.Clear();
            forma.GridStavke.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = "Automobil",
                HeaderText = "Automobil",
                DataSource = sviAutomobili,
                DisplayMember = "Model",
                ValueMember = "IdAutomobil"
            });
            forma.GridStavke.Columns.Add("Cena", "Cena");
            forma.GridStavke.Columns.Add("Popust", "Popust");
            forma.GridStavke.Columns.Add("Iznos", "Iznos");

            // Popuni redove
            forma.GridStavke.Rows.Clear();
            foreach (var stavka in originalUgovor.Stavke)
            {
                forma.GridStavke.Rows.Add(
                    stavka.Automobil.IdAutomobil,
                    stavka.Automobil.Cena,
                    stavka.Popust,
                    stavka.Iznos
                );
            }

            //Datum
            forma.LabelDatum.Text = originalUgovor.Datum.ToString("dd.MM.yyyy");
        }

        private void PoveziEvente()
        {
            forma.ButtonDodajStavku.Click += (s, e) => forma.GridStavke.Rows.Add();
            forma.ButtonObrisiStavku.Click += (s, e) =>
            {
                foreach (DataGridViewRow row in forma.GridStavke.SelectedRows)
                    forma.GridStavke.Rows.Remove(row);
            };
            forma.ButtonSacuvaj.Click += SacuvajPromene;
            forma.ButtonOdustani.Click += (s, e) => forma.Close();

            forma.GridStavke.CellValueChanged += GridStavke_CellValueChanged;
            forma.GridStavke.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (forma.GridStavke.IsCurrentCellDirty)
                    forma.GridStavke.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
        }

        private void GridStavke_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var column = forma.GridStavke.Columns[e.ColumnIndex];
            var row = forma.GridStavke.Rows[e.RowIndex];

            if (column.Name == "Automobil")
            {
                if (row.Cells["Automobil"].Value == null) return;

                int automobilId = Convert.ToInt32(row.Cells["Automobil"].Value);
                var auto = sviAutomobili.FirstOrDefault(a => a.IdAutomobil == automobilId);
                if (auto != null)
                {
                    // Popuni kolone
                    row.Cells["Cena"].Value = auto.Cena;

                    // Postavi popust: 10% ako je električni, 0 inače
                    row.Cells["Popust"].Value = auto.TipGoriva.ToLower() == "elektricni" ? 0.1 : 0.0;

                    // Izračunaj iznos
                    double cena = Convert.ToDouble(row.Cells["Cena"].Value);
                    double popust = Convert.ToDouble(row.Cells["Popust"].Value);
                    row.Cells["Iznos"].Value = cena - (cena * popust);
                }
            }
        }


        private void SacuvajPromene(object sender, EventArgs e)
        {
            bool postojiValidnaStavka = forma.GridStavke.Rows
                .Cast<DataGridViewRow>()
                .Any(r => !r.IsNewRow && r.Cells["Automobil"].Value != null);

            if (!postojiValidnaStavka)
            {
                forma.ShowError("Ugovor mora imati bar jednu stavku sa izabranim automobilom!");
                return;
            }

            try
            {
                var azuriraniUgovor = new Ugovor
                {
                    IdUgovor = originalUgovor.IdUgovor,
                    Kupac = (Kupac)forma.ComboKupac.SelectedItem,
                    Prodavac = (Prodavac)forma.ComboProdavac.SelectedItem,
                    PDV = (double)forma.NumericPDV.Value,
                    Stavke = new List<StavkaUgovora>(),
                    Datum = originalUgovor.Datum
                };

                int rb = 1;
                foreach (DataGridViewRow row in forma.GridStavke.Rows)
                {
                    if (row.IsNewRow) continue;

                    int automobilId = Convert.ToInt32(row.Cells["Automobil"].Value);
                    double cena = Convert.ToDouble(row.Cells["Cena"].Value);
                    double popust = Convert.ToDouble(row.Cells["Popust"].Value);
                    double iznos = Convert.ToDouble(row.Cells["Iznos"].Value);

                    azuriraniUgovor.Stavke.Add(new StavkaUgovora
                    {
                        Ugovor = new Ugovor { IdUgovor = azuriraniUgovor.IdUgovor },
                        Rb = rb++,
                        Automobil = sviAutomobili.First(a => a.IdAutomobil == automobilId),
                        CenaAutomobila = cena,
                        Popust = popust,
                        Iznos = iznos,
                    });
                }

                bool uspesno = Communication.Instance.SendRequest<bool>(Operation.PromeniUgovor, azuriraniUgovor);

                if (uspesno)
                    forma.ShowInfo("Sistem je zapamtio ugovor.");
                else
                    forma.ShowError("Sistem ne može da zapamti ugovor.");
                if (uspesno) forma.Close();
            }
            catch (Exception ex)
            {
                forma.ShowError($"Greška: {ex.Message}");
            }
        }
    }
}
