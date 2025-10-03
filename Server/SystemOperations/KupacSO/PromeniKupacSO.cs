using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerApp.SystemOperations.KupacSO
{
    /// <summary>
    /// Sistemska operacija koja menja postojeći <see cref="Kupac"/> u bazi podataka.
    /// Nasleđuje <see cref="SystemOperationBase"/> i implementira konkretnu logiku ažuriranja kupca.
    /// </summary>
    internal class PromeniKupacSO : SystemOperationBase
    {
        /// <summary>
        /// Kupac koji treba da bude ažuriran u bazi.
        /// </summary>
        private readonly Kupac kupac;

        /// <summary>
        /// Rezultat operacije.
        /// <c>true</c> ako je ažuriranje uspešno izvršeno, <c>false</c> inače.
        /// </summary>
        internal bool Result { get; private set; }

        /// <summary>
        /// Konstruktor koji inicijalizuje sistemsku operaciju sa kupcem koji treba da bude ažuriran.
        /// </summary>
        /// <param name="kupac">Instanca <see cref="Kupac"/> koja će biti ažurirana u bazi podataka.</param>
        /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="kupac"/> null.</exception>
        public PromeniKupacSO(Kupac kupac)
        {
            this.kupac = kupac ?? throw new ArgumentNullException(nameof(kupac), "Kupac ne može biti null");
        }

        /// <summary>
        /// Izvršava konkretnu logiku ažuriranja kupca.
        /// </summary>
        /// <remarks>
        /// - Ažurira osnovne podatke kupca (<see cref="Kupac.IdKupac"/> i <see cref="Kupac.Email"/>).<br/>
        /// - Ako je kupac tipa <see cref="FizickoLice"/>, proverava da li postoji u bazi i ažurira ili dodaje novu instancu.<br/>
        /// - Ako je kupac tipa <see cref="PravnoLice"/>, proverava da li postoji u bazi i ažurira ili dodaje novu instancu.<br/>
        /// - Rezultat se postavlja na <c>true</c> ako je operacija uspešno izvršena.
        /// </remarks>
        protected override void ExecuteConcreteOperation()
        {
            // Ažuriranje osnovnog Kupac zapisa
            broker.Update(new Kupac
            {
                IdKupac = kupac.IdKupac,
                Email = kupac.Email
            });

            // Ažuriranje ili dodavanje specifičnog tipa kupca
            if (kupac is FizickoLice fl)
            {
                var postojeci = broker.GetByCondition(new FizickoLice { IdKupac = fl.IdKupac });
                if (postojeci.Any())
                {
                    broker.Update(fl);
                }
                else
                {
                    broker.Add(fl);
                }
            }
            else if (kupac is PravnoLice pl)
            {
                var postojeci = broker.GetByCondition(new PravnoLice { IdKupac = pl.IdKupac });
                if (postojeci.Any())
                {
                    broker.Update(pl);
                }
                else
                {
                    broker.Add(pl);
                }
            }

            Result = true;
        }
    }
}
