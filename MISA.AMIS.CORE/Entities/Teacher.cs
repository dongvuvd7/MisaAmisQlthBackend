using MISA.AMIS.CORE.AttributeCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Entities
{
    public class Teacher
    {
        #region Property of Teacher
        /// <summary>
        /// ID cán bộ - khóa chính
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public Guid TeacherId { get; set; }

        /// <summary>
        /// Mã cán bộ
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        [RequiredField]
        public string TeacherCode { get; set; }

        /// <summary>
        /// Tên cán bộ
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        [RequiredField]
        public string TeacherName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        [PhoneNumberField]
        public string TeacherPhone { get; set; }

        /// <summary>
        /// Email
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        [EmailField]
        public string TeacherEmail { get; set; }

        /// <summary>
        /// Tổ chuyên môn
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        [RequiredField]
        public Guid TeacherGroup { get; set; }

        /// <summary>
        /// Tên tổ chuyên môn
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public string TeacherGroupName { get; set; }

        /// <summary>
        /// Các môn học quản lý
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public string TeacherSubject { get; set; }

        /// <summary>
        /// Các kho phòng
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public string TeacherRoom { get; set; }

        /// <summary>
        /// Có nghiệp vụ quản lý thiết bị hay không
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public int? TeacherQltb { get; set; }

        /// <summary>
        /// Tình trạng công việc
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public int? TeacherStatus { get; set; }

        /// <summary>
        /// Ngày nghỉ việc
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public DateTime? TeacherStopday { get; set; }
        #endregion
    }
}
