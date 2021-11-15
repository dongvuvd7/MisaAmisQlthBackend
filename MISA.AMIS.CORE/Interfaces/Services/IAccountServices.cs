using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Interfaces.Services
{
    public interface IAccountServices : IBaseServices<Account>
    {

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
        /// Validate file Excel để xuất dữ liệu
        /// </summary>
        /// <returns>file Excel định dạng tương ứng</returns>
        /// CreatedBy: VDDong (12/11/2021)
        public Stream ExportExcel();


    }
}
