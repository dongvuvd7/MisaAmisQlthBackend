using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Exceptions
{
    public class NccBankException : Exception
    {
        public NccBankException(string message) : base(message)
        {

        }
    }
}
