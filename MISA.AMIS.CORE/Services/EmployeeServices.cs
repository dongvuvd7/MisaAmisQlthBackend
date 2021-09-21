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
    public class EmployeeServices : BaseServices<Employee>, IEmployeeServices
    {
        IEmployeeRepository _employeeRepository;

        public EmployeeServices(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Validate file Excel để xuất dữ liệu
        /// </summary>
        /// <returns>file Excel định dạng tương ứng</returns>
        /// CreatedBy: VDDong (09/06/2021)
        public Stream ExportExcel()
        {
            //lấy tất cả danh sách nhân viên để export ra file excel
            var list = _employeeRepository.GetAll().ToList<Employee>();
            //lấy tất cả danh sách phòng ban để format phòng ban (export ra departmentName chứ không nên departmentId)
            var departmentList = _employeeRepository.GetDepartmentName().ToList<EmployeeDepartment>();

            //khai báo khởi tạo file excel có tên DANH SÁCH NHÂN VIÊN
            var stream = new MemoryStream();
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH NHÂN VIÊN");

            // set độ rộng từng column
            workSheet.Column(1).Width = 5;
            workSheet.Column(2).Width = 15;
            workSheet.Column(3).Width = 30;
            workSheet.Column(4).Width = 10;
            workSheet.Column(5).Width = 15;
            workSheet.Column(6).Width = 30;
            workSheet.Column(7).Width = 30;
            workSheet.Column(8).Width = 15;
            workSheet.Column(9).Width = 30;

            //dòng đầu tiên - tiêu đề
            using (var range = workSheet.Cells["A1:I1"]) //độ rộng tiêu đề từ cột A1 đến cột I1
            {
                range.Merge = true; //gộp các cột lại (bỏ border ngăn giữa đi)
                range.Value = "DANH SÁCH NHÂN VIÊN"; //giá trị dòng đó
                range.Style.Font.Bold = true; //set font đậm
                range.Style.Font.Size = 16; //set font size
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //căn giữa dòng
            }

            // style cho excel.
            //gán giá trị cho từng ô[hàng, cột]
            workSheet.Cells[3, 1].Value = "STT";
            workSheet.Cells[3, 2].Value = "Mã nhân viên";
            workSheet.Cells[3, 3].Value = "Tên nhân viên";
            workSheet.Cells[3, 4].Value = "Giới tính";
            workSheet.Cells[3, 5].Value = "Ngày sinh";
            workSheet.Cells[3, 6].Value = "Chức danh";
            workSheet.Cells[3, 7].Value = "Tên đơn vị";
            workSheet.Cells[3, 8].Value = "Số tài khoản";
            workSheet.Cells[3, 9].Value = "Tên ngân hàng";

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
                //Format tên giới tính
                if (list[i].Gender == 1) list[i].GenderName = Properties.Resources.Male;
                else if (list[i].Gender == 0) list[i].GenderName = Properties.Resources.Female;
                else if (list[i].Gender == 2) list[i].GenderName = Properties.Resources.Rest;
                //Format tên phòng ban
                //list[i].DepartmentName = _employeeRepository.GetDepartmentById(list[i].DepartmentId).DepartmentName;
                for (int j = 0; j < departmentList.Count(); j++)
                {
                    if (list[i].DepartmentId == departmentList[j].DepartmentId) list[i].DepartmentName = departmentList[j].DepartmentName;
                }
                //bắt đầu từ dòng 4 nên (i+4)
                workSheet.Cells[i + 4, 1].Value = i + 1; //STT
                workSheet.Cells[i + 4, 2].Value = list[i].EmployeeCode;
                workSheet.Cells[i + 4, 3].Value = list[i].FullName;
                workSheet.Cells[i + 4, 4].Value = list[i].GenderName;
                workSheet.Cells[i + 4, 5].Value = list[i].DateOfBirth.ToString("dd/MM/yyyy");
                workSheet.Cells[i + 4, 6].Value = list[i].JobTitle;
                workSheet.Cells[i + 4, 7].Value = list[i].DepartmentName;
                workSheet.Cells[i + 4, 8].Value = list[i].BankAccount;
                workSheet.Cells[i + 4, 9].Value = list[i].BankName;

                //đóng khung border cho từng dòng từ ô[i+1, 1] đến ô[i+4, 9]
                using (var range = workSheet.Cells[i + 4, 1, i + 4, 9])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; //căn trái dữ liệu
                }
            }

            package.Save();
            stream.Position = 0;
            return package.Stream;
        }

        /// <summary>
        /// Lấy tên các phòng ban
        /// </summary>
        /// <returns>Trả về tên các phòng ban</returns>
        /// CreatedBy: VDDong (08/06/2021)
        public IEnumerable<EmployeeDepartment> GetDepartmentName()
        {
            var response = _employeeRepository.GetDepartmentName();
            return response;
        }

        /// <summary>
        /// Lấy danh sách nhân viên có lọc
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên 1 trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách nhân viên tương ứng</returns>
        /// CreatedBy: VDDong (08/06/2021)
        public IEnumerable<Employee> GetEmployees(int pageSize, int pageIndex, string filter)
        {
            if(pageSize <= 0 || pageIndex <= 0)
            {
                throw new EmployeeException(Properties.Resources.Invalid_Paging_number);
            }
            var response = _employeeRepository.GetEmployees(pageSize, pageIndex, filter);
            return response;
        }

        /// <summary>
        /// Lấy mã nhân viên lớn nhất trong database
        /// </summary>
        /// <returns>Mã nhân viên lớn nhất dạng NV-4số</returns>
        /// CreatedBy: VDDong (08/06/2021)
        public string GetMaxCode()
        {
            var response = _employeeRepository.GetMaxCode();
            int number = Int32.Parse(response);
            number += 1;
            var result = number.ToString();
            //fix sao cho mã nhân viên luôn là dạng chữ + 4 chữ số
            if (number < 10) result = Properties.Resources.NV_000 + result;
            else if (number >= 10 && number < 100) result = Properties.Resources.NV_00 + result;
            else if (number >= 100 && number < 1000) result = Properties.Resources.NV_0 + result;
            else result = Properties.Resources.NV_ + result;
            return result;
        }

        /// <summary>
        /// Lấy dữ liệu nhân viên theo tìm kiếm theo mã hoặc tên nhân viên
        /// </summary>
        /// <param name="searchResult"></param>
        /// <returns>Kết quả sau tìm kiếm</returns>
        /// CreatedBy: VDDong (08/06/2021)
        public IEnumerable<Employee> GetSearchResult(string searchResult)
        {
            var responses = _employeeRepository.GetSearchResult(searchResult);
            return responses;
        }

        protected override void CustomValidate(Employee entity, HttpType http)
        {
            if(entity is Employee)
            {
                var employee = entity as Employee;
                var employeeCode = employee.EmployeeCode;
                //Kiểm tra xem mã nhân viên tồn tại hay chưa (tùy thuộc vào post hay put mà lựa chọn cách kiểm tra khác nhau)
                var isEmployeeCodeExists = _employeeRepository.CheckDuplicateEmployeeCode(employee.EmployeeCode, employee.EmployeeId, http);
                //Nếu trùng
                if (isEmployeeCodeExists)
                {
                    throw new EmployeeException(Properties.Resources.Employee_Code + $" <{employeeCode}> " + Properties.Resources.Duplicate_msg);
                }
            }
        }
    }
}
