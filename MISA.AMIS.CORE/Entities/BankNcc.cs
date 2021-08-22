using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Entities
{
    public class BankNcc
    {
        public Guid BankId { get; set; }
        public Guid UserId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string BankPlace { get; set; }
    }
}
