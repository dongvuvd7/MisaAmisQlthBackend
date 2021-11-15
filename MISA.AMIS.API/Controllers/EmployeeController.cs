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
    public class EmployeeController : BaseController<Employee>
    {
        IEmployeeRepository _employeeRepository;
        IEmployeeServices _employeeServices;
        public EmployeeController(IEmployeeRepository employeeRepository, IEmployeeServices employeeServices) : base(employeeRepository, employeeServices)
        {
            _employeeRepository = employeeRepository;
            _employeeServices = employeeServices;
        }

        /// <summary>
        /// Lấy tất cả phòng ban từ database
        /// </summary>
        /// <returns>Tên phòng ban tương ứng</returns>
        /// <response code = "200">: có dữ liệu trả về</response>
        /// <response code = "204">: không có dữ liệu trả về</response>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpGet("Department")]
        public IActionResult GetDepartmentName()
        {
            var departments = _employeeServices.GetDepartmentName();
            if (departments.Count() > 0) return Ok(departments);
            else return NoContent();
        }

        /// <summary>
        /// Lấy dữ liệu phù hợp với kết quả tìm kiếm của người dùng
        /// </summary>
        /// <param name="searchResult"></param>
        /// <returns>Dữ liệu bản ghi phù hợp với kết quả tìm kiếm</returns>
        /// <response code = "200">: có dữ liệu trả về</response>
        /// <response code = "204">: không có dữ liệu trả về</response>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpGet("Search/{searchResult}")]
        public IActionResult GetSearchResult(string searchResult)
        {
            var response = _employeeServices.GetSearchResult(searchResult);
            if (response.Count() > 0) return Ok(response);
            else return NoContent();
        }

        /// <summary>
        /// Lấy mã nhân viên lớn nhất trong database
        /// </summary>
        /// <returns>Mã nhân viên lớn nhất</returns>
        /// <response code = "200">: có dữ liệu trả về</response>
        /// <response code = "204">: không có dữ liệu trả về</response>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpGet("MaxCode")]
        public IActionResult GetMaxCode()
        {
            var response = _employeeServices.GetMaxCode();
            if (response != null) return Ok(response);
            else return NoContent();
        }

        /// <summary>
        /// Lấy danh sách nhân viên có lọc
        /// </summary>
        /// <param name="pageSize">số lượng nhân viên / trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách nhân viên</returns>
        /// <response code = "200">: có dữ liệu trả về</response>
        /// <response code = "204">: không có dữ liệu trả về</response>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpGet("Filter")]
        public IActionResult GetEmployees(int pageSize, int pageIndex, string filter)
        {
            var totalRecord = _employeeRepository.GetTotalEmployees(filter);
            var employees = _employeeServices.GetEmployees(pageSize, pageIndex, filter);
            var response = new
            {
                totalRecord = totalRecord,
                data = employees
            };
            if (response.data.Any() && response.totalRecord != 0)
            {
                return Ok(response);
            }
            else return NoContent();
        }

        /// <summary>
        /// Xuất dữ liệu employee ra file excel
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>file excel</returns>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpGet("Export")]
        public IActionResult Export()
        {
            var stream = _employeeServices.ExportExcel();
            string excelName = $"{Properties.Resources.ExcelFileName}(ExportByVVDong).xlsx";
            //return File(stream, "application/octet-stream", excelName);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

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
        [HttpGet("SortByCode")]
        public IActionResult GetEmployeesSortByCode(int pageSize, int pageIndex, string departmentString)
        {
            var totalRecord = _employeeRepository.GetTotalEmployeesSortBy(departmentString);
            var employees = _employeeServices.GetEmployeesSortByCode(pageSize, pageIndex, departmentString);
            var response = new
            {
                totalRecord = totalRecord,
                data = employees,
            };
            if (response.data.Any() && response.totalRecord != 0)
            {
                return Ok(response);
            }
            else return NoContent();
        }
        [HttpGet("SortByName")]
        public IActionResult GetEmployeesSortByName(int pageSize, int pageIndex, string departmentString)
        {
            var totalRecord = _employeeRepository.GetTotalEmployeesSortBy(departmentString);
            var employees = _employeeServices.GetEmployeesSortByName(pageSize, pageIndex, departmentString);
            var response = new
            {
                totalRecord = totalRecord,
                data = employees,
            };
            if (response.data.Any() && response.totalRecord != 0)
            {
                return Ok(response);
            }
            else return NoContent();
        }

        /// <summary>
        /// Lấy các ngân hàng liên kết với nhân viên theo UserId (mã nhân viên)
        /// Phục vụ việc bind danh sách các ngân hàng của nhân viên lên tab ngân hàng
        /// </summary>
        /// <param name="UserId">Mã nhân viên</param>
        /// <returns>Danh sách các bản ghi ngân hàng tương ứng</returns>
        /// CreatedBy: VDDong (02/11/2021)
        [HttpGet("GetAllBankEmpByUserId/{UserId}")]
        public IActionResult GetBankEmpByUserId(Guid UserId)
        {
            var responses = _employeeServices.GetBankEmpByUserId(UserId);
            if (responses.Count() > 0) return Ok(responses);
            else return NoContent();
        }

        /// <summary>
        /// Lấy bản ghi nhân viên theo EmployeeCode (mã nhân viên)
        /// Phục vụ việc lấy EmployeeId làm khóa ngoại để post các bankEmp lên
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>Bản ghi nhân viên tương ứng</returns>
        /// CreatedBy: VDDong (02/11/2021)
        [HttpGet("GetEmployeeByEmployeeCode/{employeeCode}")]
        public IActionResult GetEmployeeByEmployeeCode(string employeeCode)
        {
            var response = _employeeServices.GetEmployeeByEmployeeCode(employeeCode);
            if (response != null) return Ok(response);
            else return NoContent();
        }


    }
}
