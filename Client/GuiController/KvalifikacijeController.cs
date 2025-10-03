using Client.UserControls;
using Common.Communication;
using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.GuiController
{
    internal class KvalifikacijeController
    {
        private readonly FrmMain frmMain;
        private readonly UCKvalifikacije ucKvalifikacije;

        public KvalifikacijeController(FrmMain frmMain)
        {
            this.frmMain = frmMain;
            ucKvalifikacije = new UCKvalifikacije();

            ucKvalifikacije.AddRequested += OnAdd;

            LoadKvalifikacije();
        }

        internal void Show()
        {
            frmMain.SetMainContent(ucKvalifikacije);
        }

        private void LoadKvalifikacije()
        {
            try
            {
                var kvalifikacije = Communication.Instance.SendRequest<List<Kvalifikacija>>(
                    Operation.VratiListuSviKvalifikacija);

                BindKvalifikacijeToGrid(kvalifikacije);
            }
            catch (Exception ex)
            {
                ucKvalifikacije.ShowError($"Greška pri učitavanju kvalifikacija: {ex.Message}");
            }
        }

        private void OnAdd()
        {
            try
            {
                if (!ValidirajUnos()) return;

                var nova = new Kvalifikacija
                {
                    Naziv = ucKvalifikacije.Naziv,
                    Stepen = ucKvalifikacije.Stepen
                };

                Kvalifikacija rezultat = Communication.Instance.SendRequest<Kvalifikacija>(
                    Operation.UbaciKvalifikacija, nova);

                if (rezultat == null)
                {
                    ucKvalifikacije.ShowInfo("Sistem ne može da zapamti kvalifikaciju.");
                }
                else
                {
                    ucKvalifikacije.ShowInfo("Sistem je zapamtio kvalifikaciju.");
                    LoadKvalifikacije();
                }
            }
            catch (Exception ex)
            {
                ucKvalifikacije.ShowError($"Greška pri pamćenju kvalifikacije: {ex.Message}");
            }
        }


        private void BindKvalifikacijeToGrid(List<Kvalifikacija> kvalifikacije)
        {
            var grid = ucKvalifikacije.Grid;

            grid.DataSource = null;
            grid.Columns.Clear();

            grid.DataSource = kvalifikacije
                .Select(k => new
                {
                    k.Naziv,
                    k.Stepen
                })
                .ToList();

            grid.Columns["Naziv"].HeaderText = "Naziv kvalifikacije";
            grid.Columns["Stepen"].HeaderText = "Stepen";
        }
        private bool ValidirajUnos()
        {
            if (string.IsNullOrWhiteSpace(ucKvalifikacije.Naziv))
            {
                ucKvalifikacije.ShowError("Naziv kvalifikacije je obavezno polje.");
                return false;
            }

            if (ucKvalifikacije.Stepen == null || string.IsNullOrWhiteSpace(ucKvalifikacije.Stepen))
            {
                ucKvalifikacije.ShowError("Stepen kvalifikacije je obavezno polje.");
                return false;
            }

            return true;
        }
    }
}
