using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Entities
{
    public class Room
    {
        #region Property of Room
        /// <summary>
        /// ID phòng - khóa chính
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Tên phòng
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public string RoomName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public string ModifiedBy { get; set; }
        #endregion
    }
}
