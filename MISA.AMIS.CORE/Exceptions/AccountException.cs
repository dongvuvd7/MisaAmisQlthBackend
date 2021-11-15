using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Exceptions
{
    public class AccountException : Exception
    {
        public AccountException(string message) : base(message)
        {

        }
    }
}
