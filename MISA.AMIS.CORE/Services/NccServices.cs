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
    public class NccServices : BaseServices<Ncc>, INccServices
    {
        INccRepository _nccRepository;
        public NccServices(INccRepository nccRepository) : base(nccRepository)
        {
            _nccRepository = nccRepository;
        }

        /// <summary>
        /// Validate file Excel để xuất dữ liệu
        /// </summary>
        /// <returns>file Excel định dạng tương ứng</returns>
        /// CreatedBy: VDDong (21/09/2021)
        public Stream ExportExcel()
        {
            //lấy tất cả danh sách nhân viên để export ra file excel
            var list = _nccRepository.GetAll().ToList<Ncc>();

            //khai báo khởi tạo file excel có tên DANH SÁCH NHÀ CUNG CẤP
            var stream = new MemoryStream();
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH NHÀ CUNG CẤP");

            // set độ rộng từng column
            workSheet.Column(1).Width = 5;
            workSheet.Column(2).Width = 15;
            workSheet.Column(3).Width = 30;
            workSheet.Column(4).Width = 20;
            workSheet.Column(5).Width = 30;
            workSheet.Column(6).Width = 35;
            workSheet.Column(7).Width = 40;

            //dòng đầu tiên - tiêu đề
            using (var range = workSheet.Cells["A1:G1"]) //độ rộng tiêu đề từ cột A1 đến cột I1
            {
                range.Merge = true; //gộp các cột lại (bỏ border ngăn giữa đi)
                range.Value = "DANH SÁCH NHÀ CUNG CẤP"; //giá trị dòng đó
                range.Style.Font.Bold = true; //set font đậm
                range.Style.Font.Size = 16; //set font size
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //căn giữa dòng
            }

            // style cho excel.
            //gán giá trị cho từng ô[hàng, cột]
            workSheet.Cells[3, 1].Value = "STT";
            workSheet.Cells[3, 2].Value = "Mã NCC";
            workSheet.Cells[3, 3].Value = "Tên NCC";
            workSheet.Cells[3, 4].Value = "Điện thoại";
            workSheet.Cells[3, 5].Value = "Nhóm NCC";
            workSheet.Cells[3, 6].Value = "Địa chỉ";
            workSheet.Cells[3, 7].Value = "Ghi chú";

            //style cho các ô từ A3 đến I3
            using (var range = workSheet.Cells["A3:G3"])
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
                //bắt đầu từ dòng 4 nên (i+4)
                workSheet.Cells[i + 4, 1].Value = i + 1; //STT
                workSheet.Cells[i + 4, 2].Value = list[i].NccCode;
                workSheet.Cells[i + 4, 3].Value = list[i].NccName;
                workSheet.Cells[i + 4, 4].Value = list[i].NccPhone;
                workSheet.Cells[i + 4, 5].Value = list[i].NhomNcc;
                workSheet.Cells[i + 4, 6].Value = list[i].NccAddress;
                workSheet.Cells[i + 4, 7].Value = list[i].Ghichu;

                //đóng khung border cho từng dòng từ ô[i+1, 1] đến ô[i+4, 7]
                using (var range = workSheet.Cells[i + 4, 1, i + 4, 7])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; //căn trái dữ liệu
                }
            }

            package.Save();
            stream.Position = 0;
            return package.Stream;
        }

        public IEnumerable<BankNcc> GetBankNccByUserId(Guid UserId)
        {
            var responses = _nccRepository.GetBankNccByUserId(UserId);
            return responses;
        }

        public string GetMaxCode()
        {
            var response = _nccRepository.GetMaxCode();
            int number = Int32.Parse(response);
            number += 1;
            var result = number.ToString();
            //fix sao cho mã nhân viên luôn là dạng chữ + 3 chữ số
            if (number < 10) result = "NCC00" + result;
            else if (number >= 10 && number < 100) result = "NCC0" + result;
            else result = "NCC" + result;
            return result;
        }

        public IEnumerable<Ncc> GetNccs(int pageSize, int pageIndex, string filter)
        {
            if (pageSize <= 0 || pageIndex <= 0)
            {
                throw new NccException(Properties.Resources.Invalid_Paging_number);
            }
            var response = _nccRepository.GetNccs(pageSize, pageIndex, filter);
            return response;
        }

        public IEnumerable<Ncc> GetSearchResult(string searchResult)
        {
            var responses = _nccRepository.GetSearchResult(searchResult);
            return responses;
        }

        public int GetTotalNccs(string filter)
        {
            var responses = _nccRepository.GetTotalNccs(filter);
            return responses;
        }

        public Ncc GetNccByNccCode(string nccCode)
        {
            var response = _nccRepository.GetNccByNccCode(nccCode);
            return response;
        }

        protected override void CustomValidate(Ncc entity, HttpType http)
        {
            if (entity is Ncc)
            {
                var ncc = entity as Ncc;
                var nccCode = ncc.NccCode;
                //Kiểm tra xem mã nhà cung cấp tồn tại hay chưa (tùy thuộc vào post hay put mà lựa chọn cách kiểm tra khác nhau)
                var isNccCodeExists = _nccRepository.CheckDuplicateNccCode(ncc.NccCode, ncc.NccId, http);
                //Nếu trùng
                if (isNccCodeExists)
                {
                    throw new EmployeeException(Properties.Resources.Ncc_Code + $" <{nccCode}> " + Properties.Resources.Duplicate_msg);
                }
            }
        }
    }
}
