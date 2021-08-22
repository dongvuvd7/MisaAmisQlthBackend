using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using MISA.AMIS.CORE.Exceptions;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MISA.AMIS.CORE.Interfaces.Services;
using System;
using System.Collections.Generic;
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

        public Stream ExportExcel()
        {
            throw new NotImplementedException();
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
