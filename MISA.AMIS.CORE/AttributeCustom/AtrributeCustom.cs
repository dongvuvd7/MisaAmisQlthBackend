using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.AttributeCustom
{
    /// <summary>
    /// Trường kiểm tra xem thuộc tính này có bỏ trống không?
    /// CreatedBy: VDDong (19/11/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredField : Attribute
    {
        public string MsgError = string.Empty;
        public RequiredField(string msgError = "")
        {
            MsgError = msgError;
        }
    }

    /// <summary>
    /// Trường kiểm tra email có hợp lệ không
    /// CreatedBy: VDDong (19/11/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailField : Attribute
    {
    }

    /// <summary>
    /// Trường kiểm tra số điện thoại có hợp lệ không
    /// CreatedBy: VDDong (19/11/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneNumberField : Attribute
    {
    }

    /// <summary>
    /// Trường kiểm tra xem property có quá số lượng ký tự cho phép không
    /// CreatedBy: VDDong (19/11/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLengthField : Attribute
    {
        public int MaxLength = 20;
        public string MsgError = string.Empty;
    }

    /// <summary>
    /// Trường kiểm tra định dạng website
    /// CreatedBy: VDDong (19/11/2021)
    /// </summary>
    public class WebsiteField : Attribute
    {

    }


}
