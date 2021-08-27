using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Interfaces.Repositories
{
    public interface INccRepository : IBaseRepository<Ncc>
    {
        public IEnumerable<BankNcc> GetBankNccName();
        public IEnumerable<BankNcc> GetBankNccByUserId(Guid UserId);
        public bool CheckDuplicateNccCode(string nccCode, Guid nccId, HttpType http);
        public IEnumerable<Ncc> GetSearchResult(string searchResult);
        public string GetMaxCode();
        public IEnumerable<Ncc> GetNccs(int pageSize, int pageIndex, string filter);
        public int GetTotalNccs(string filter);
        public Ncc GetNccByNccCode(string nccCode);
    }
}
