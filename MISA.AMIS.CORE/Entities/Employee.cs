using MISA.AMIS.CORE.AttributeCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Entities
{
    public class Employee
    {
        /// <summary>
        /// ID nhân viên - Khóa chính
        /// CreatedBy: VDDong (08/07/2021)
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// CreatedBy: VDDong (08/07/2021)
        /// </summary>
        [RequiredField]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Họ và tên nhân viên
        /// CreatedBy: VDDong (08/07/2021)
        /// </summary>
        [RequiredField]
        public string FullName { get; set; }

        /// <summary>
        /// Giới tính (lưu trong database)
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// Giới tính (trả về phía client)
        /// </summary>
        public string GenderName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Số CMND
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp CMND
        /// </summary>
        public DateTime IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp CMND
        /// </summary>
        public string IdentityPlace { get; set; }

        /// <summary>
        /// Khóa chính (ID) chức vụ
        /// </summary>
        public Guid PositionId { get; set; }

        /// <summary>
        /// Tên chức vụ
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Khóa chính (ID) phòng ban
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [RequiredField]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string BankBranch { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        [PhoneNumberField]
        public string Phone { get; set; }

        /// <summary>
        /// Số điện thoại bàn
        /// </summary>
        [PhoneNumberField]
        public string Telephone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [EmailField]
        public string Email { get; set; }

    }
}
