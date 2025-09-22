using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication
{
    /// <summary>
    /// Predstavlja zahtev koji se šalje serveru.
    /// Sadrži operaciju koja se izvršava i  argument.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Operacija koja se izvršava.
        /// </summary>
        public Operation Operation { get; set; }
        /// <summary>
        /// Argument.
        /// </summary>
        public object Argument { get; set; }
    }
}
