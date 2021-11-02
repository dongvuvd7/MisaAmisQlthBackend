using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MISA.AMIS.CORE.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Services
{
    public class BankEmpServices : BaseServices<BankEmp>, IBankEmpServices
    {
        IBankEmpRepository _bankEmpRepository; 
        public BankEmpServices(IBankEmpRepository bankEmpRepository) : base(bankEmpRepository)
        {
            _bankEmpRepository = bankEmpRepository;
        }
    }
}
