using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Interfaces.Repositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        /// <summary>
        /// Kiểm tra trùng tên khoản thu trong database
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountId"></param>
        /// <param name="http"></param>
        /// <returns>true: trùng / false: không trùng</returns>
        /// CreatedBy:VDDong (12/11/2021)
        public bool CheckDuplicateAccountName(string accountName, Guid accountId, HttpType http);

        /// <summary>
        /// Lấy danh sách khoản thu có lọc
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên 1 trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách khoản thu tương ứng</returns>
        /// CreatedBy: VDDong (12/11/2021)
        public IEnumerable<Account> GetAccounts(int pageSize, int pageIndex, string filter);

        /// <summary>
        /// Lấy số lượng khoản thu sau khi lọc
        /// </summary>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Số lương (int) khoản thu</returns>
        /// CreatedBy: VDDong (12/11/2021)
        public int GetTotalAccounts(string filter);



    }
}
