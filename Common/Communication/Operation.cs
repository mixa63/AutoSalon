using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication
{
    /// <summary>
    /// Predstavlja operacije koje se mogu izvršiti preko servisa ili servera.
    /// </summary>
    public enum Operation
    {
        None,
        KreirajUgovor,
        PretraziUgovor,
        VratiListuUgovor,
        PromeniUgovor,
        KreirajKupac,
        PretraziKupac,
        VratiListuKupac,
        PromeniKupac,
        ObrisiKupac,
        UbaciKvalifikacija,
        VratiListuSviKvalifikacija,
        PrijaviProdavac,
        VratiListuSviProdavac,
        VratiListuSviKupac,
        VratiListuSviAutomobil
    }
}
