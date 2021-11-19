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
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class TeacherController : BaseController<Teacher>
    {
        #region Constructor
        ITeacherRepository _teacherRepository;
        ITeacherServices _teacherServices;
        public TeacherController(ITeacherRepository teacherRepository, ITeacherServices teacherServices) : base(teacherRepository, teacherServices)
        {
            _teacherRepository = teacherRepository;
            _teacherServices = teacherServices;
        }
        #endregion

        #region API Methods
        /// <summary>
        /// Lấy tất cả tổ từ database
        /// </summary>
        /// <returns>Tên tổ tương ứng</returns>
        /// <response code = "200">: có dữ liệu trả về</response>
        /// <response code = "204">: không có dữ liệu trả về</response>
        /// CreatedBy: VDDong (19/11/2021)
        [HttpGet("Group")]
        public IActionResult GetGroupName()
        {
            var groups = _teacherServices.GetGroupName();
            if (groups.Count() > 0) return Ok(groups);
            else return NoContent();
        }

        /// <summary>
        /// Lấy dữ liệu phù hợp với kết quả tìm kiếm của người dùng
        /// </summary>
        /// <param name="searchResult"></param>
        /// <returns>Dữ liệu bản ghi phù hợp với kết quả tìm kiếm</returns>
        /// <response code = "200">: có dữ liệu trả về</response>
        /// <response code = "204">: không có dữ liệu trả về</response>
        /// CreatedBy: VDDong (19/11/2021)
        [HttpGet("Search/{searchResult}")]
        public IActionResult GetSearchResult(string searchResult)
        {
            var response = _teacherServices.GetSearchResult(searchResult);
            if (response.Count() > 0) return Ok(response);
            else return NoContent();
        }

        /// <summary>
        /// Lấy mã giáo viên lớn nhất trong database
        /// </summary>
        /// <returns>Mã giáo viên lớn nhất</returns>
        /// <response code = "200">: có dữ liệu trả về</response>
        /// <response code = "204">: không có dữ liệu trả về</response>
        /// CreatedBy: VDDong (19/11/2021)
        [HttpGet("MaxCode")]
        public IActionResult GetMaxCode()
        {
            var response = _teacherServices.GetMaxCode();
            if (response != null) return Ok(response);
            else return NoContent();
        }

        /// <summary>
        /// Lấy danh sách giáo viên có lọc
        /// </summary>
        /// <param name="pageSize">số lượng giáo viên / trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách giáo viên</returns>
        /// <response code = "200">: có dữ liệu trả về</response>
        /// <response code = "204">: không có dữ liệu trả về</response>
        /// CreatedBy: VDDong (19/11/2021)
        [HttpGet("Filter")]
        public IActionResult GetTeachers(int pageSize, int pageIndex, string filter)
        {
            var totalRecord = _teacherRepository.GetTotalTeachers(filter);
            var teachers = _teacherServices.GetTeachers(pageSize, pageIndex, filter);
            var response = new
            {
                totalRecord = totalRecord,
                data = teachers
            };
            if (response.data.Any() && response.totalRecord != 0)
            {
                return Ok(response);
            }
            else return NoContent();
        }

        /// <summary>
        /// Xuất dữ liệu cán bộ ra file excel
        /// </summary>
        /// <returns>file excel</returns>
        /// CreatedBy: VDDong (19/11/2021)
        [HttpGet("Export")]
        public IActionResult Export()
        {
            var stream = _teacherServices.ExportExcel();
            string excelName = $"{Properties.Resources.FileTeacherExcel}(ExportByVVDong).xlsx";
            //return File(stream, "application/octet-stream", excelName);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        /// <summary>
        /// Lấy danh sách giáo viên theo 2 cách:
        /// 1. Sắp xếp theo code (theo thứ tự thêm mới)
        /// 2. Sắp xếp theo tên (thứ tự anphabe)
        /// Kết hợp với nhóm theo phòng ban (đơn vị)
        /// </summary>
        /// <param name="pageSize">số bản ghi / trang</param>
        /// <param name="pageIndex">số trang</param>
        /// <param name="departmentString">điều kiện để nhóm phòng ban</param>
        /// <returns></returns>
        /// CreatedBy: VDDong (19/11/2021)
        [HttpGet("CodeDecrease")]
        public IActionResult GetTeachersSortByCode(int pageSize, int pageIndex, string groupString)
        {
            var totalRecord = _teacherRepository.GetTotalTeachersSortBy(groupString);
            var teachers = _teacherServices.GetTeachersSortByCode(pageSize, pageIndex, groupString);
            var response = new
            {
                totalRecord = totalRecord,
                data = teachers,
            };
            if (response.data.Any() && response.totalRecord != 0)
            {
                return Ok(response);
            }
            else return NoContent();
        }
        [HttpGet("NameAnphabe")]
        public IActionResult GetTeachersSortByName(int pageSize, int pageIndex, string groupString)
        {
            var totalRecord = _teacherRepository.GetTotalTeachersSortBy(groupString);
            var teachers = _teacherServices.GetTeachersSortByName(pageSize, pageIndex, groupString);
            var response = new
            {
                totalRecord = totalRecord,
                data = teachers,
            };
            if (response.data.Any() && response.totalRecord != 0)
            {
                return Ok(response);
            }
            else return NoContent();
        }

        /// <summary>
        /// Xóa nhiều bản ghi một lúc
        /// </summary>
        /// <param name="recordIds"></param>
        /// <returns>Số bản ghi ảnh hưởng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        [HttpDelete("MultipleDelete/{recordIds}")]
        public IActionResult DeleteMultiple(string recordIds)
        {
            var rowsAffect = _teacherServices.DeleteMultipleTeacher(recordIds);
            if (rowsAffect > 0) return Ok(rowsAffect);
            else return NoContent();
        }
        #endregion
    }
}
