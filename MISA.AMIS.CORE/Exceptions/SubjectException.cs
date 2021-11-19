using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Entities
{
    public class SubjectException : Exception
    {
        public SubjectException(string message) : base(message)
        {

        }
    }
}
