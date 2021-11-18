using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using MISA.AMIS.CORE.Exceptions;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MISA.AMIS.CORE.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        /// <summary>
        /// Xóa nhiều bản ghi teacher
        /// </summary>
        /// <param name="recordIds">Chuỗi các id muốn xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        public int DeleteMultipleTeacher(string recordIds)
        {
            var rowsAffects = _teacherRepository.DeleteMultipleTeacher(recordIds);
            return rowsAffects;
        }

        /// <summary>
        /// Xuất bản ra file excel
        /// </summary>
        /// <returns>File excel danh sách các bản ghi cán bộ giáo viên</returns>
        public Stream ExportExcel()
        {
            //lấy tất cả danh sách giáo viên để export ra file excel
            var list = _teacherRepository.GetAll().ToList<Teacher>();
            //lấy tất cả danh sách tổ để format tổ (export ra groupName chứ không nên groupId)
            var groupList = _teacherRepository.GetGroupName().ToList<TeacherGroup>();

            //khai báo khởi tạo tiêu đề sheet trong file excel là DANH SÁCH CÁN BỘ
            var stream = new MemoryStream();
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH CÁN BỘ");

            // set độ rộng từng column
            workSheet.Column(1).Width = 5; //STT
            workSheet.Column(2).Width = 15; //Mã
            workSheet.Column(3).Width = 30; //Tên
            workSheet.Column(4).Width = 20; //ĐT
            workSheet.Column(5).Width = 25; //Tổ
            workSheet.Column(6).Width = 30; //Môn
            workSheet.Column(7).Width = 30; //Phòng
            workSheet.Column(8).Width = 15; //QLTB
            workSheet.Column(9).Width = 15; //Trạng thái

            //dòng đầu tiên - tiêu đề
            using (var range = workSheet.Cells["A1:I1"]) //độ rộng tiêu đề từ cột A1 đến cột I1
            {
                range.Merge = true; //gộp các cột lại (bỏ border ngăn giữa đi)
                range.Value = "DANH SÁCH CÁN BỘ"; //giá trị dòng đó
                range.Style.Font.Bold = true; //set font đậm
                range.Style.Font.Size = 16; //set font size
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //căn giữa dòng
            }

            // style cho excel.
            //gán giá trị cho từng ô[hàng, cột]
            workSheet.Cells[3, 1].Value = "STT";
            workSheet.Cells[3, 2].Value = "Số hiệu cán bộ";
            workSheet.Cells[3, 3].Value = "Họ và tên";
            workSheet.Cells[3, 4].Value = "Số điện thoại";
            workSheet.Cells[3, 5].Value = "Tổ chuyên môn";
            workSheet.Cells[3, 6].Value = "Quản lý thiết bị môn";
            workSheet.Cells[3, 7].Value = "Quản lý kho - phòng";
            workSheet.Cells[3, 8].Value = "Đào tạo QLTB";
            workSheet.Cells[3, 9].Value = "Đang làm việc";

            //style cho các ô từ A3 đến I3
            using (var range = workSheet.Cells["A3:I3"])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray); //background color
                range.Style.Font.Bold = true; //set font đậm
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin); //border xung quanh
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //căn giữa dòng
            }



            // đổ dữ liệu từ list vào.
            for (int i = 0; i < list.Count(); i++)
            {

                //Format tên tổ
                for (int j = 0; j < groupList.Count(); j++)
                {
                    if (list[i].TeacherGroup == groupList[j].GroupId) list[i].TeacherGroupName = groupList[j].GroupName;
                }
                //Format đào tạo QLTB
                String qltb = "";
                if(list[i].TeacherQltb == 1) qltb = "x";
                //Format tình trạng công việc
                String StatusWork = "";
                if (list[i].TeacherStatus == 1) StatusWork = "x";

                //bắt đầu từ dòng 4 nên (i+4)
                workSheet.Cells[i + 4, 1].Value = i + 1; //STT
                workSheet.Cells[i + 4, 2].Value = list[i].TeacherCode;
                workSheet.Cells[i + 4, 3].Value = list[i].TeacherName;
                workSheet.Cells[i + 4, 4].Value = list[i].TeacherPhone;
                workSheet.Cells[i + 4, 5].Value = list[i].TeacherGroupName;
                workSheet.Cells[i + 4, 6].Value = list[i].TeacherSubject;
                workSheet.Cells[i + 4, 7].Value = list[i].TeacherRoom;
                workSheet.Cells[i + 4, 8].Value = qltb;
                workSheet.Cells[i + 4, 9].Value = StatusWork;

                //đóng khung border cho từng dòng từ ô[i+1, 1] đến ô[i+4, 9]
                using (var range = workSheet.Cells[i + 4, 1, i + 4, 7])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; //căn trái dữ liệu
                }
                using (var range = workSheet.Cells[i + 4, 8, i + 4, 9])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //căn giữa dữ liệu
                }
            }

            package.Save();
            stream.Position = 0;
            return package.Stream;
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
