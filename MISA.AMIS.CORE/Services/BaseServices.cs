using MISA.AMIS.CORE.AttributeCustom;
using MISA.AMIS.CORE.Enums;
using MISA.AMIS.CORE.Exceptions;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MISA.AMIS.CORE.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Services
{
    public class BaseServices<MISAEntity> : IBaseServices<MISAEntity> where MISAEntity : class
    {
        #region Constructor
        IBaseRepository<MISAEntity> _dataAccessBaseRepository;

        public BaseServices(IBaseRepository<MISAEntity> dataAccessBaseRepository)
        {
            _dataAccessBaseRepository = dataAccessBaseRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Xóa 1 bản ghi theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public int Delete(Guid entityId)
        {
            var rowsAffects = _dataAccessBaseRepository.Delete(entityId);
            return rowsAffects;
        }

        /// <summary>
        /// Lấy tất cả bản ghi từ database
        /// </summary>
        /// <returns>Tất cả bản ghi</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public IEnumerable<MISAEntity> GetAll()
        {
            var response = _dataAccessBaseRepository.GetAll();
            return response;
        }

        /// <summary>
        /// Lấy 1 bản ghi theo Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Bản ghi tương ứng với Id</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public MISAEntity GetById(Guid entityId)
        {
            var response = _dataAccessBaseRepository.GetById(entityId);
            return response;
        }

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>Dữ liệu phân trang các bản ghi</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public IEnumerable<MISAEntity> GetPaging(int pageIndex, int pageSize)
        {
            if(pageIndex < 0 || pageSize < 0)
            {
                var response = new
                {
                    devMsg = Properties.Resources.Invalid_Paging_number,
                    MISACode = Properties.Resources.MISACode
                };
                throw new EmployeeException(response.devMsg);
            }

            var result = _dataAccessBaseRepository.GetPaging(pageIndex, pageSize);
            return result;
        }

        /// <summary>
        /// Insert 1 bản ghi vào database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public int Post(MISAEntity entity)
        {
            Validate(entity, HttpType.POST);
            var rowsAffects = _dataAccessBaseRepository.Post(entity);
            return rowsAffects;
        }

        /// <summary>
        /// Update 1 bản ghi theo id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public int Put(MISAEntity entity)
        {
            Validate(entity, HttpType.PUT);
            var rowsAffects = _dataAccessBaseRepository.Put(entity);
            return rowsAffects;
        }

        /// <summary>
        /// Validate trước khi request vào database
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="http"></param>
        /// CreatedBy: VDDong (19/11/2021)
        private void Validate(MISAEntity entity, HttpType http)
        {
            //Lấy ra tất cả các property của class
            var properties = typeof(MISAEntity).GetProperties();
            foreach(var property in properties)
            {
                //Lấy giá trị của ô input Field đấy
                var propertyValue = property.GetValue(entity);

                //Kiểm tra requred field có null không, nếu có thì sẽ throw ra exception
                //Lấy property mà có RequiedFiled
                var requiredProperties = property.GetCustomAttributes(typeof(RequiredField), true);
                if(requiredProperties.Length > 0)
                {
                    //Kiểm tra nếu giá trị là null
                    if(propertyValue == null)
                    {
                        propertyValue = "";
                    }
                    //Kiểm tra nếu giá trị bị bỏ trống
                    if (string.IsNullOrEmpty(propertyValue.ToString()))
                    {
                        var msgError = (requiredProperties[0] as RequiredField).MsgError;
                        if (string.IsNullOrEmpty(msgError))
                        {
                            msgError = $"{property.Name} {Properties.Resources.Null_msg}";
                        }
                        throw new EmployeeException(msgError);
                    }
                }

                //Kiểm tra email field có đúng định dạng không
                //Lấy ra property email field
                var emailProperties = property.GetCustomAttributes(typeof(EmailField), true);
                if(emailProperties.Length > 0)
                {
                    if(propertyValue == null || propertyValue  =="") continue;
                    else
                    {
                        var emailTemplate = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                        if(!Regex.IsMatch(propertyValue.ToString(), emailTemplate))
                        {
                            var response = new
                            {
                                devMsg = Properties.Resources.Email_Invalid_msg,
                                MISACode = Properties.Resources.MISACode
                            };
                        throw new EmployeeException(response.devMsg);
                        }
                    }
                }

                //Kiểm tra phoneNumberField có đúng định dạng hay không, nếu có thì throw exception
                //Lấy ra property phoneNumberField
                var phoneNumberProperties = property.GetCustomAttributes(typeof(PhoneNumberField), true);
                if (phoneNumberProperties.Length > 0)
                {
                    //Kiểm tra nếu giá trị là null thì vẫn pass regex
                    if (propertyValue == null)
                    {
                        propertyValue = "";
                    }

                    var phoneNumberTemplate = @"^[0-9]*$";
                    //Kiểm tra giá trị
                    if (!Regex.IsMatch(propertyValue.ToString(), phoneNumberTemplate))
                    {
                        var response = new
                        {
                            devMsg = Properties.Resources.PhoneNumber_Invalid_msg,
                            MISACode = Properties.Resources.MISACode
                        };
                        var nameProperty = "";
                        if (String.Equals(property.Name, "Phone")) nameProperty = "ĐT di động";
                        else if (String.Equals(property.Name, "TeacherPhone")) nameProperty = "Số điện thoại";
                        else if (String.Equals(property.Name, "Telephone")) nameProperty = "ĐT cố định";
                        throw new EmployeeException(nameProperty + " chưa đúng định dạng !");
                    }
                }

                //Kiểm tra MaxLengthField không quá 20 kí tự, nếu quá thì throw exception
                //Lấy ra property MaxLengthField
                var maxLengthProperties = property.GetCustomAttributes(typeof(MaxLengthField), true);
                if (maxLengthProperties.Length > 0)
                {
                    var maxLength = (maxLengthProperties[0] as MaxLengthField).MaxLength;
                    if (propertyValue.ToString().Length > maxLength)
                    {
                        var msgError = (maxLengthProperties[0] as MaxLengthField).MsgError;
                        if (string.IsNullOrEmpty(msgError))
                        {
                            msgError = $"{property.Name} {Properties.Resources.Max_length_msg} {maxLength}";
                        }
                        throw new EmployeeException(msgError);
                    }
                }

                //Kiểm tra WebsiteField có đúng định dạng @TênWebsite không, nếu không thì throw exception
                //Lấy ra property WebsiteField
                var websiteProperties = property.GetCustomAttributes(typeof(WebsiteField), true);
                if (websiteProperties.Length > 0)
                {
                    if (propertyValue == null || propertyValue == "") continue;
                    else
                    {
                        var websiteTemplate = @"@[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)?";
                        if (!Regex.IsMatch(propertyValue.ToString(), websiteTemplate))
                        {
                            var response = new
                            {
                                devMsg = Properties.Resources.Website_Invalid_msg,
                                MISACode = Properties.Resources.MISACode
                            };
                            throw new EmployeeException(response.devMsg);
                        }
                    }
                }

            }

            CustomValidate(entity, http);
        }

        /// <summary>
        /// Validate riêng cho mỗi thực thể
        /// Thực thể nào dùng thì override lại
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="http"></param>
        /// CreatedBy: VDDong (19/11/2021)
        protected virtual void CustomValidate (MISAEntity entity, HttpType http)
        {

        }

        #endregion
    }
}
