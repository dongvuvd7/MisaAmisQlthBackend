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
    public class TeacherServices : BaseServices<Teacher>, ITeacherServices
    {
        ITeacherRepository _teacherRepository;

        public TeacherServices(ITeacherRepository teacherRepository) : base(teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }


        public Stream ExportExcel()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy tên các tổ
        /// </summary>
        /// <returns>Trả về tên các tổ</returns>
        /// CreatedBy: VDDong (15/11/2021)
        public IEnumerable<TeacherGroup> GetGroupName()
        {
            var response = _teacherRepository.GetGroupName();
            return response;
        }

        /// <summary>
        /// Lấy mã giáo viên lớn nhất trong database
        /// </summary>
        /// <returns>Mã giáo viên lớn nhất</returns>
        /// CreatedBy: VDDong (15/11/2021)
        public string GetMaxCode()
        {
            var response = _teacherRepository.GetMaxCode();
            int number = Int32.Parse(response);
            number += 1;
            var result = number.ToString();
            //fix sao cho mã giáo viên luôn là dạng chữ + 3 chữ số
            if (number < 10) result = Properties.Resources.MS00 + result;
            else if (number >= 10 && number < 100) result = Properties.Resources.MS0 + result;
            else result = Properties.Resources.MS + result;
            return result;
        }

        /// <summary>
        /// Lấy dữ liệu giáo viên theo tìm kiếm theo mã hoặc tên giáo viên
        /// </summary>
        /// <param name="searchResult"></param>
        /// <returns>Kết quả sau tìm kiếm</returns>
        /// CreatedBy: VDDong (15/11/2021)
        public IEnumerable<Teacher> GetSearchResult(string searchResult)
        {
            var responses = _teacherRepository.GetSearchResult(searchResult);
            return responses;
        }

        /// <summary>
        /// Lấy danh sách giáo viên có lọc
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên 1 trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách giáo viên tương ứng</returns>
        /// CreatedBy: VDDong (15/11/2021)
        public IEnumerable<Teacher> GetTeachers(int pageSize, int pageIndex, string filter)
        {
            if (pageSize <= 0 || pageIndex <= 0)
            {
                throw new TeacherException(Properties.Resources.Invalid_Paging_number);
            }
            var response = _teacherRepository.GetTeachers(pageSize, pageIndex, filter);
            return response;
        }

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
        public IEnumerable<Teacher> GetTeachersSortByCode(int pageSize, int pageIndex, string groupString)
        {
            if (pageSize <= 0 || pageIndex <= 0)
            {
                throw new TeacherException(Properties.Resources.Invalid_Paging_number);
            }
            var responses = _teacherRepository.GetTeachersSortByCode(pageSize, pageIndex, groupString);
            return responses;
        }

        public IEnumerable<Teacher> GetTeachersSortByName(int pageSize, int pageIndex, string groupString)
        {
            if (pageSize <= 0 || pageIndex <= 0)
            {
                throw new TeacherException(Properties.Resources.Invalid_Paging_number);
            }
            var responses = _teacherRepository.GetTeachersSortByName(pageSize, pageIndex, groupString);
            return responses;
        }


        protected override void CustomValidate(Teacher entity, HttpType http)
        {
            if (entity is Teacher)
            {
                var teacher = entity as Teacher;
                var teacherCode = teacher.TeacherCode;
                //Kiểm tra xem mã giáo viên tồn tại hay chưa (tùy thuộc vào post hay put mà lựa chọn cách kiểm tra khác nhau)
                var isTeacherCodeExists = _teacherRepository.CheckDuplicateTeacherCode(teacher.TeacherCode, teacher.TeacherId, http);
                //Nếu trùng
                if (isTeacherCodeExists)
                {
                    throw new TeacherException(Properties.Resources.Teacher_Code + $" <{teacherCode}> " + Properties.Resources.Duplicate_msg);
                }
            }
        }
    }
}
