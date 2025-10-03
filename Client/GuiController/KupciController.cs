using Client.UserControls;
using Common.Communication;
using Common.Domain;
using System.Data;
using Client.Forms;

namespace Client.GuiController
{
    internal class KupciController
    {
        private readonly FrmMain frmMain;
        private readonly UCKupci ucKupci;

        public KupciController(FrmMain form)
        {
            frmMain = form;
            ucKupci = new UCKupci();

            ucKupci.SearchRequested += OnSearch;
            ucKupci.DetailsRequested += OnDetails;
            ucKupci.AddRequested += OnAdd;

            LoadKupaci();
        }

        private void OnAdd()
        {
            try
            {
                var noviKupac = new Kupac();

                Kupac kupac = Communication.Instance.SendRequest<Kupac>(Operation.KreirajKupac, noviKupac);
                if (kupac == null)
                {
                    ucKupci.ShowInfo("Sistem ne može da kreira kupca.");
                }
                else
                {
                    ucKupci.ShowInfo("Sistem je kreirao kupca.");
                    new DetaljiKupcaController(kupac);
                    LoadKupaci();
                }

            }
            catch (Exception ex)
            {
                ucKupci.ShowError($"Greška pri kreiranju kupca: {ex.Message}");
            }
        }

        private void LoadKupaci()
        {
            try
            {
                var searchKupac = new Kupac { IdKupac = -1 };
                List<Kupac> kupci = Communication.Instance.SendRequest<List<Kupac>>(Operation.VratiListuSviKupac, searchKupac);
                BindKupciToGrid(kupci);
            }
            catch (Exception ex)
            {
                ucKupci.ShowError($"Greška pri učitavanju kupca: {ex.Message}");
            }
        }

        private void OnSearch()
        {
            try
            {
                var searchKupac = new Kupac();

                if (!string.IsNullOrWhiteSpace(ucKupci.KupacEmail))
                {
                    searchKupac = new Kupac { Email = ucKupci.KupacEmail };
                }

                List<Kupac> kupci = Communication.Instance.SendRequest<List<Kupac>>(Operation.VratiListuKupac, searchKupac);

                if (kupci == null || kupci.Count == 0)
                {
                    ucKupci.ShowInfo("Sistem nije našao kupca po zadatim kriterijumima.");
                }
                else
                {
                    ucKupci.ShowInfo("Sistem je našao kupce po zadatim kriterijumima.");
                }

                BindKupciToGrid(kupci);
            }
            catch (Exception ex)
            {
                ucKupci.ShowError($"Greška pri pretrazi: {ex.Message}");
            }
        }

        private void OnDetails(int ugovorId)
        {
            try
            {
                if (ugovorId <= 0)
                {
                    ucKupci.ShowInfo("Molimo izaberite kupce iz liste.");
                    return;
                }

                var searchKupac = new Kupac { IdKupac = ugovorId };

                
                Kupac kupac = Communication.Instance.PretraziKupca(searchKupac);
                
                if (kupac == null)
                {
                    ucKupci.ShowInfo("Sistem ne može da nađe kupca.");
                }
                else
                {
                    ucKupci.ShowInfo("Sistem je našao kupca.");
                    new DetaljiKupcaController(kupac);
                    LoadKupaci();
                }

            }
            catch (Exception ex)
            {
                ucKupci.ShowError($"Greška pri prikazu kupca: {ex.Message}");
            }
        }

        public void Show()
        {
            frmMain.SetMainContent(ucKupci);
        }

        private void BindKupciToGrid(List<Kupac> kupci)
        {
            var grid = ucKupci.Grid;

            grid.DataSource = null;
            grid.Columns.Clear();

            grid.DataSource = kupci
                .Select(u => new
                {
                    u.IdKupac,
                    EmailKupca = u.Email,
                })
                .ToList();
            grid.Columns["IdKupac"].HeaderText = "ID";
            grid.Columns["EmailKupca"].HeaderText = "Email kupca";
        }

    }
}