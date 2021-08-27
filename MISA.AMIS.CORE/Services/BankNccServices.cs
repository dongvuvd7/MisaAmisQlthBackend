using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MISA.AMIS.CORE.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Services
{
    public class BankNccServices : BaseServices<BankNcc>, IBankNccServices
    {
        IBankNccRepository _bankNccRepository;
        public BankNccServices(IBankNccRepository bankNccRepository) : base(bankNccRepository)
        {
            _bankNccRepository = bankNccRepository;
        }

    }
}
