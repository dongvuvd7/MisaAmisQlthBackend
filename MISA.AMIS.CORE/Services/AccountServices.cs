using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using MISA.AMIS.CORE.Exceptions;
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
    public class AccountServices : BaseServices<Account>, IAccountServices
    {
        IAccountRepository _accountRepository;

        public AccountServices(IAccountRepository accountRepository) : base(accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Validate file Excel để xuất dữ liệu
        /// </summary>
        /// <returns>file Excel định dạng tương ứng</returns>
        /// CreatedBy: VDDong (12/11/2021)
        public Stream ExportExcel()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy danh sách khoản thu có lọc
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên 1 trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách khoản thu tương ứng</returns>
        /// CreatedBy: VDDong (12/11/2021)
        public IEnumerable<Account> GetAccounts(int pageSize, int pageIndex, string filter)
        {
            if (pageSize <= 0 || pageIndex <= 0)
            {
                throw new AccountException(Properties.Resources.Invalid_Paging_number);
            }
            var response = _accountRepository.GetAccounts(pageSize, pageIndex, filter);
            return response;
        }

        protected override void CustomValidate(Account entity, HttpType http)
        {
            if (entity is Account)
            {
                var account = entity as Account;
                var accountName = account.AccountName;
                //Kiểm tra xem tên khoản thu tồn tại hay chưa (tùy thuộc vào post hay put mà lựa chọn cách kiểm tra khác nhau)
                var isAccountNameExists = _accountRepository.CheckDuplicateAccountName(account.AccountName, account.AccountId, http);
                //Nếu trùng
                if (isAccountNameExists)
                {
                    throw new AccountException(Properties.Resources.Account_Name + $" <{accountName}> " + Properties.Resources.Duplicate_msg);
                }
            }
        }

    }
}
