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
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class BankNccController : BaseController<BankNcc>
    {
        IBankNccRepository _bankNccRepository;
        IBankNccServices _bankNccServices;
        public BankNccController(IBankNccRepository bankNccRepository, IBankNccServices bankNccServices) : base(bankNccRepository, bankNccServices)
        {
            _bankNccRepository = bankNccRepository;
            _bankNccServices = bankNccServices;
        }
    }
}
