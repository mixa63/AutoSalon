using Client.UserControls;
using Common.Communication;
using Common.Domain;
using System.Data;
using Client.Forms;

namespace Client.GuiController
{
    internal class UgovoriController
    {
        private readonly FrmMain frmMain;
        private readonly UCUgovori ucUgovori;

        public UgovoriController(FrmMain form)
        {
            frmMain = form;
            ucUgovori = new UCUgovori();

            ucUgovori.SearchRequested += OnSearch;
            ucUgovori.DetailsRequested += OnDetails;
            ucUgovori.AddRequested += OnAdd;

            LoadUgovori();
        }

        private void OnAdd()
        {
            try
            {
                var noviUgovor = new Ugovor();

                Ugovor ugovor = Communication.Instance.SendRequest<Ugovor>(Operation.KreirajUgovor, noviUgovor);
                if (ugovor == null)
                {
                    ucUgovori.ShowInfo("Sistem ne može da kreira ugovor.");
                }
                else
                {
                    ucUgovori.ShowInfo("Sistem je kreirao ugovor.");
                    new DetaljiUgovoraController(ugovor);
                    LoadUgovori();
                }

            }
            catch (Exception ex)
            {
                ucUgovori.ShowError($"Greška pri kreiranju ugovora: {ex.Message}");
            }
        }

        private void LoadUgovori()
        {
            try
            {
                var searchUgovor = new Ugovor { IdUgovor = -1 };
                List<Ugovor> ugovori = Communication.Instance.SendRequest<List<Ugovor>>(Operation.VratiListuUgovor, searchUgovor);
                BindUgovoriToGrid(ugovori);
            }
            catch (Exception ex)
            {
                ucUgovori.ShowError($"Greška pri učitavanju ugovora: {ex.Message}");
            }
        }

        private void OnSearch()
        {
            try
            {
                var searchUgovor = new Ugovor();

                if (!string.IsNullOrWhiteSpace(ucUgovori.KupacEmail))
                {
                    searchUgovor.Kupac = new Kupac { Email = ucUgovori.KupacEmail };
                }

                if (!string.IsNullOrWhiteSpace(ucUgovori.ProdavacIme))
                {
                    searchUgovor.Prodavac = new Prodavac { Ime = ucUgovori.ProdavacIme };
                }

                if (double.TryParse(ucUgovori.MinIznos, out double iznosSaPdv))
                {
                    searchUgovor.IznosSaPDV = iznosSaPdv;
                }

                if (!string.IsNullOrWhiteSpace(ucUgovori.AutomobilModel))
                {
                    searchUgovor.Stavke.Add(new StavkaUgovora
                    {
                        Automobil = new Automobil { Model = ucUgovori.AutomobilModel }
                    });
                }

                List<Ugovor> ugovori = Communication.Instance.SendRequest<List<Ugovor>>(Operation.VratiListuUgovor, searchUgovor);

                if (ugovori == null || ugovori.Count == 0)
                {
                    ucUgovori.ShowInfo("Sistem nije našao ugovor po zadatim kriterijumima.");
                }
                else
                {
                    ucUgovori.ShowInfo("Sistem je našao ugovore po zadatim kriterijumima.");
                }

                BindUgovoriToGrid(ugovori);
            }
            catch (Exception ex)
            {
                ucUgovori.ShowError($"Greška pri pretrazi: {ex.Message}");
            }
        }

        private void OnDetails(int ugovorId)
        {
            try
            {
                if (ugovorId <= 0)
                {
                    ucUgovori.ShowInfo("Molimo izaberite ugovor iz liste.");
                    return;
                }

                var searchUgovor = new Ugovor { IdUgovor = ugovorId };

                Ugovor ugovor = Communication.Instance.SendRequest<Ugovor>(Operation.PretraziUgovor, searchUgovor);
                if (ugovor == null)
                {
                    ucUgovori.ShowInfo("Sistem ne može da nađe ugovor.");
                }
                else
                {
                    ucUgovori.ShowInfo("Sistem je našao ugovor.");
                    new DetaljiUgovoraController(ugovor);
                    LoadUgovori();
                }
                
            }
            catch (Exception ex)
            {
                ucUgovori.ShowError($"Greška pri prikazu ugovora: {ex.Message}");
            }
        }

        public void Show()
        {
            frmMain.SetMainContent(ucUgovori);
        }

        private void BindUgovoriToGrid(List<Ugovor> ugovori)
        {
            var grid = ucUgovori.Grid;

            grid.DataSource = null;
            grid.Columns.Clear();

            grid.DataSource = ugovori
                .Select(u => new
                {
                    u.IdUgovor,
                    Kupac = u.Kupac?.Email,
                    Prodavac = u.Prodavac?.Ime,
                    IznosBezPDV = u.IznosBezPDV,
                    IznosSaPDV = u.IznosSaPDV,
                    BrojAutomobila = u.BrAutomobila
                })
                .ToList();
            grid.Columns["IdUgovor"].HeaderText = "ID";
            grid.Columns["IznosBezPDV"].HeaderText = "Iznos bez PDV";
            grid.Columns["IznosSaPDV"].HeaderText = "Iznos sa PDV";
            grid.Columns["BrojAutomobila"].HeaderText = "Br. automobila";
            grid.Columns["IdUgovor"].Visible = false;
        }

    }
}