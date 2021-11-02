using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MISA.AMIS.CORE.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.AMIS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BankEmpController : BaseController<BankEmp>
    {
        IBankEmpRepository _bankEmpRepository;
        IBankEmpServices _bankEmpServices;
        public BankEmpController(IBankEmpRepository bankEmpRepository, IBankEmpServices bankEmpServices) : base(bankEmpRepository, bankEmpServices)
        {
            _bankEmpRepository = bankEmpRepository;
            _bankEmpServices = bankEmpServices;
        }
    }
}
