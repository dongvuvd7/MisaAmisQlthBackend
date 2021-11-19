using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Entities
{
    public class TeacherGroup
    {
        #region Property of Group
        /// <summary>
        /// ID tổ chuyên môn - khóa chính
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// Tên tổ
        /// CreatedBy: VDDong (19/11/2021)
        /// </summary>
        public string GroupName { get; set; }
        #endregion
    }
}
