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
        public Guid TeacherId { get; set; }

        [RequiredField]
        public string TeacherCode { get; set; }

        [RequiredField]
        public string TeacherName { get; set; }

        [PhoneNumberField]
        public string TeacherPhone { get; set; }

        [EmailField]
        public string TeacherEmail { get; set; }

        [RequiredField]
        public Guid TeacherGroup { get; set; }

        public string TeacherGroupName { get; set; }

        public string TeacherSubject { get; set; }

        public string TeacherRoom { get; set; }

        public int? TeacherQltb { get; set; }

        public int? TeacherStatus { get; set; }

        public DateTime? TeacherStopday { get; set; }
    }
}
