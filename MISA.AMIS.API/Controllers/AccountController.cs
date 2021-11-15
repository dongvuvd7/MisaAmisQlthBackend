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
    [Route("api/[controller]s")]
    [ApiController]
    public class AccountController : BaseController<Account>
    {
        IAccountRepository _accountRepository;
        IAccountServices _accountServices;

        public AccountController(IAccountRepository accountRepository, IAccountServices accountServices) : base(accountRepository, accountServices)
        {
            _accountRepository = accountRepository;
            _accountServices = accountServices;
        }

        /// <summary>
        /// Lấy danh sách khoản thu có lọc
        /// </summary>
        /// <param name="pageSize">số lượng khoản thu / trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách khoản thu</returns>
        /// <response code = "200">: có dữ liệu trả về</response>
        /// <response code = "204">: không có dữ liệu trả về</response>
        /// CreatedBy: VDDong (12/11/2021)
        [HttpGet("Filter")]
        public IActionResult GetAccounts(int pageSize, int pageIndex, string filter)
        {
            var totalRecord = _accountRepository.GetTotalAccounts(filter);
            var accounts = _accountServices.GetAccounts(pageSize, pageIndex, filter);
            var response = new
            {
                totalRecord = totalRecord,
                data = accounts
            };
            if (response.data.Any() && response.totalRecord != 0)
            {
                return Ok(response);
            }
            else return NoContent();
        }

    }
}
