using MISA.AMIS.CORE.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Interfaces.Services
{
    public interface IEmployeeServices : IBaseServices<Employee>
    {
        /// <summary>
        /// Lấy tên các phòng ban
        /// </summary>
        /// <returns>Trả về tên các phòng ban</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public IEnumerable<EmployeeDepartment> GetDepartmentName();

        /// <summary>
        /// Lấy dữ liệu nhân viên theo tìm kiếm theo mã hoặc tên nhân viên
        /// </summary>
        /// <param name="searchResult"></param>
        /// <returns>Kết quả sau tìm kiếm</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public IEnumerable<Employee> GetSearchResult(string searchResult);

        /// <summary>
        /// Lấy mã nhân viên lớn nhất trong database
        /// </summary>
        /// <returns>Mã nhân viên lớn nhất</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public string GetMaxCode();

        /// <summary>
        /// Lấy danh sách nhân viên có lọc
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên 1 trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách nhân viên tương ứng</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public IEnumerable<Employee> GetEmployees(int pageSize, int pageIndex, string filter);

        /// <summary>
        /// Validate file Excel để xuất dữ liệu
        /// </summary>
        /// <returns>file Excel định dạng tương ứng</returns>
        /// CreatedBy: VDDong (09/07/2021)
        public Stream ExportExcel();

        /// <summary>
        /// Lấy danh sách nhân viên theo 2 cách:
        /// 1. Sắp xếp theo code (theo thứ tự thêm mới)
        /// 2. Sắp xếp theo tên (thứ tự anphabe)
        /// Kết hợp với nhóm theo phòng ban (đơn vị)
        /// </summary>
        /// <param name="pageSize">số bản ghi / trang</param>
        /// <param name="pageIndex">số trang</param>
        /// <param name="departmentString">điều kiện để nhóm phòng ban</param>
        /// <returns></returns>
        public IEnumerable<Employee> GetEmployeesSortByCode(int pageSize, int pageIndex, string departmentString);
        public IEnumerable<Employee> GetEmployeesSortByName(int pageSize, int pageIndex, string departmentString);

        /// <summary>
        /// Lấy các ngân hàng liên kết với nhân viên theo UserId (mã nhân viên)
        /// Phục vụ việc bind danh sách các ngân hàng của nhân viên lên tab ngân hàng
        /// </summary>
        /// <param name="UserId">Mã nhân viên</param>
        /// <returns>Danh sách các bản ghi ngân hàng tương ứng</returns>
        public IEnumerable<BankEmp> GetBankEmpByUserId(Guid UserId);

        /// <summary>
        /// Lấy bản ghi nhân viên theo EmployeeCode (mã nhân viên)
        /// Phục vụ việc lấy EmployeeId làm khóa ngoại để post các bankEmp lên
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>bản ghi nhân viên tương ứng</returns>
        public Employee GetEmployeeByEmployeeCode(string employeeCode);
    }
}
