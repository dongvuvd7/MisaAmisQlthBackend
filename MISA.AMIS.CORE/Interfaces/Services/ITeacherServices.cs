using MISA.AMIS.CORE.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Interfaces.Services
{
    public interface ITeacherServices : IBaseServices<Teacher>
    {
        /// <summary>
        /// Lấy tên các tổ
        /// </summary>
        /// <returns>Trả về tên các tổ</returns>
        /// CreatedBy: VDDong (15/11/2021)
        public IEnumerable<TeacherGroup> GetGroupName();

        /// <summary>
        /// Lấy dữ liệu giáo viên theo tìm kiếm theo mã hoặc tên giáo viên
        /// </summary>
        /// <param name="searchResult"></param>
        /// <returns>Kết quả sau tìm kiếm</returns>
        /// CreatedBy: VDDong (15/11/2021)
        public IEnumerable<Teacher> GetSearchResult(string searchResult);

        /// <summary>
        /// Lấy mã giáo viên lớn nhất trong database
        /// </summary>
        /// <returns>Mã giáo viên lớn nhất</returns>
        /// CreatedBy: VDDong (15/11/2021)
        public string GetMaxCode();

        /// <summary>
        /// Lấy danh sách giáo viên có lọc
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên 1 trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách giáo viên tương ứng</returns>
        /// CreatedBy: VDDong (15/11/2021)
        public IEnumerable<Teacher> GetTeachers(int pageSize, int pageIndex, string filter);

        /// <summary>
        /// Validate file Excel để xuất dữ liệu
        /// </summary>
        /// <returns>file Excel định dạng tương ứng</returns>
        /// CreatedBy: VDDong (15/11/2021)
        public Stream ExportExcel();

        /// <summary>
        /// Lấy danh sách giáo viên theo 2 cách:
        /// 1. Sắp xếp theo code (theo thứ tự thêm mới)
        /// 2. Sắp xếp theo tên (thứ tự anphabe)
        /// Kết hợp với nhóm theo tổ
        /// </summary>
        /// <param name="pageSize">số bản ghi / trang</param>
        /// <param name="pageIndex">số trang</param>
        /// <param name="groupString">điều kiện để nhóm tổ</param>
        /// <returns>Danh sách bản ghi tương ứng</returns>
        public IEnumerable<Teacher> GetTeachersSortByCode(int pageSize, int pageIndex, string groupString);
        public IEnumerable<Teacher> GetTeachersSortByName(int pageSize, int pageIndex, string groupString);

        /// <summary>
        /// Xóa nhiều bản ghi teacher
        /// </summary>
        /// <param name="recordIds">Chuỗi các id muốn xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        public int DeleteMultipleTeacher(string recordIds);

    }
}
