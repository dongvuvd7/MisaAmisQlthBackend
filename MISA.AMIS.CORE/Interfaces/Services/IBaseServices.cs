using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Interfaces.Services
{
    public interface IBaseServices<MISAEntity> where MISAEntity : class
    {
        /// <summary>
        /// Lấy tất cả bản ghi từ database
        /// </summary>
        /// <returns>Tất cả bản ghi</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public IEnumerable<MISAEntity> GetAll();

        /// <summary>
        /// Lấy 1 bản ghi theo Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Bản ghi tương ứng với Id</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public MISAEntity GetEmployeeById(Guid entityId);

        /// <summary>
        /// Insert 1 bản ghi vào database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public int Post(MISAEntity entity);

        /// <summary>
        /// Update 1 bản ghi theo id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public int Put(MISAEntity entity);

        /// <summary>
        /// Xóa 1 bản ghi theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public int Delete(Guid entityId);

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>Dữ liệu phân trang các bản ghi</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public IEnumerable<MISAEntity> GetPaging(int pageIndex, int pageSize);
    }
}
