using MISA.AMIS.CORE.AttributeCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Entities
{
    public class Ncc
    {
        /// <summary>
        /// Id Nha cung cap - Khoa chinh
        /// CreatedBy: VDDong (21/08/2021)
        /// </summary>
        public Guid NccId { get; set; }

        [RequiredField]
        public string NccCode { get; set; }
        public string NccMst { get; set; }

        [RequiredField]
        public string NccName { get; set; }
        public string NccAddress { get; set; }
        public string NccPhone { get; set; }

        [WebsiteField]
        public string NccWebsite { get; set; }
        public string NhomNcc { get; set; }
        public string NvMuahang { get; set; }
        public string NlhXungho { get; set; }
        public string NlhName { get; set; }

        [EmailField]
        public string NlhEmail { get; set; }
        public string NlhPhone { get; set; }
        public string NlhPhapluat { get; set; }
        public string DkttMa { get; set; }
        public string TkcnMa { get; set; }
        public string DkttSongay { get; set; }
        public string DkttSono { get; set; }
        public string DckQg { get; set; }
        public string DckTt { get; set; }
        public string DckQh { get; set; }
        public string DckXp { get; set; }
        public string Dcgh { get; set; }
        public string Ghichu { get; set; }
    }
}
