using MISA.AMIS.CORE.AttributeCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Entities
{
    public class Account
    {
        public Guid AccountId { get; set; }

        [RequiredField]
        public string AccountName { get; set; }

        public int? AccountGroup { get; set; }

        public string AccountGroupName { get; set; }

        [RequiredField]
        public string AccountMoney { get; set; }

        [RequiredField]
        public int AccountMoneyType { get; set; }

        public string AccountMoneyTypeName { get; set; }

        [RequiredField]
        public int AccountArea { get; set; }

        public string AccountAreaName { get; set; }

        public int? AccountProperty { get; set; }

        public string AccountPropertyName { get; set; }

        [RequiredField]
        public int AccountSemester { get; set; }

        public string AccountSemesterName { get; set; }

        public int? AccountMiengiam { get; set; }

        public int? AccountBatbuoc { get; set; }

        public int? AccountHoadon { get; set; }

        public int? AccountChungtu { get; set; }

        public int? AccountHoantra { get; set; }

        public int? AccountNoibo { get; set; }

        public int? AccountFollow { get; set; }
    }
}
