﻿using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Interfaces.Repositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// Lấy tên các phòng ban
        /// </summary>
        /// <returns>Trả về tên các phòng ban</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public IEnumerable<EmployeeDepartment> GetDepartmentName();

        /// <summary>
        /// Lấy tên phòng ban theo Id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns>Trả về phòng ban theo Id tương ứng</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public EmployeeDepartment GetDepartmentById(Guid departmentId);

        /// <summary>
        /// Kiểm tra trùng mã nhân viên trong database
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeId"></param>
        /// <param name="http"></param>
        /// <returns>true: trùng / false: không trùng</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public bool CheckDuplicateEmployeeCode(string employeeCode, Guid employeeId, HttpType http);

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
        /// Lấy số lượng nhân viên sau khi lọc
        /// </summary>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Số lương (int) nhân viên</returns>
        /// CreatedBy: VDDong (08/07/2021)
        public int GetTotalEmployees(string filter);
    }
}
