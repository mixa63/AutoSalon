using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication
{
    /// <summary>
    /// Predstavlja odgovor koji dolazi sa servera.
    /// Sadrži rezultat operacije ili poruku o izuzetku ukoliko je došlo do greške.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Rezultat izvršene operacije.
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// Poruka o izuzetku ukoliko je došlo do greške prilikom izvršavanja operacije.
        /// </summary>
        public string ExceptionMessage { get; set; }
    }
}
