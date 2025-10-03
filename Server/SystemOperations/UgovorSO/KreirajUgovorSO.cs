using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain;

namespace ServerApp.SystemOperations.UgovorSO
{
    /// <summary>
    /// Sistemaska operacija koja kreira novi ugovor u bazi podataka.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku kreiranja ugovora.
    /// </summary>
    internal class KreirajUgovorSO : SystemOperationBase
    {
        /// <summary>
        /// Ugovor koji treba da bude kreiran u bazi.
        /// </summary>
        private readonly Ugovor ugovor;

        /// <summary>
        /// Rezultat operacije. 
        /// Nakon uspešnog izvršenja sadrži instancu <see cref="Ugovor"/> sa generisanim <c>IdUgovor</c>.
        /// </summary>
        internal Ugovor Result { get; private set; }

        /// <summary>
        /// Inicijalizuje novu instancu sistemske operacije za kreiranje ugovora.
        /// </summary>
        /// <param name="ugovor">Instanca ugovora koja se prosleđuje kao argument operacije.</param>
        /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="ugovor"/> null.</exception>
        public KreirajUgovorSO(Ugovor ugovor)
        {
            this.ugovor = ugovor ?? throw new ArgumentNullException(nameof(ugovor), "Ugovor ne može biti null");
        }

        /// <summary>
        /// Izvršava konkretnu logiku operacije kreiranja ugovora.
        /// </summary>
        /// <remarks>
        /// - Proverava da li postoji bar jedan <see cref="Prodavac"/> i jedan <see cref="Kupac"/> u bazi.
        /// Ako ne postoje, baca izuzetak.<br/>
        /// - Kreira novi ugovor sa trenutnim datumom i default PDV vrednošću (<c>0.2</c>).<br/>
        /// - Čuva ugovor u bazi i dodeljuje mu generisani <c>IdUgovor</c>.<br/>
        /// - Rezultat se čuva u <see cref="Result"/>.
        /// </remarks>
        /// <exception cref="Exception">
        /// Ako u bazi ne postoji nijedan prodavac ili nijedan kupac.
        /// </exception>
        protected override void ExecuteConcreteOperation()
        {
            int idProdavac = broker.GetFirstId(new Prodavac());

            if (idProdavac == -1)
                throw new Exception("Mora postojati bar jedan prodavac pre kreiranja ugovora!");

            int idKupac = broker.GetFirstId(new Kupac());

            if (idKupac == -1)
                throw new Exception("Mora postojati bar jedan kupac pre kreiranja ugovora!");

            Ugovor ugovor = new Ugovor
            {
                Datum = DateTime.Now.Date,
                Kupac = new Kupac { IdKupac = idKupac },
                Prodavac = new Prodavac { IdProdavac = idProdavac },
                PDV = 0.2,
            };

            int idUgovor = broker.AddWithReturnId(ugovor);
            ugovor.IdUgovor = idUgovor;
            Result = ugovor;
        }
    }
}
