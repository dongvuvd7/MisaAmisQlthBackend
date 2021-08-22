using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Interfaces.Services
{
    public interface INccServices : IBaseServices<Ncc>
    {
        public IEnumerable<BankNcc> GetBankNccByUserId(Guid UserId);
        public IEnumerable<Ncc> GetSearchResult(string searchResult);
        public string GetMaxCode();
        public IEnumerable<Ncc> GetNccs(int pageSize, int pageIndex, string filter);
        public int GetTotalNccs(string filter);
        public Stream ExportExcel();
    }
}
