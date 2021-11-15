using Dapper;
using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.INFRASTRUCTURE.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        /// <summary>
        /// Kiểm tra trùng tên khoản thu trong database
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountId"></param>
        /// <param name="http"></param>
        /// <returns>true: trùng / false: không trùng</returns>
        /// CreatedBy:VDDong (12/11/2021)
        public bool CheckDuplicateAccountName(string accountName, Guid accountId, HttpType http)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "";
                DynamicParameters dynamicPrameters = new DynamicParameters();
                if (http == HttpType.POST)
                {
                    sqlCommand = "Proc_CheckAccountNameExist";
                    dynamicPrameters.Add("AccountName", accountName);
                }
                else
                {
                    sqlCommand = "Proc_CheckAccountNameExistPut";
                    dynamicPrameters.Add("AccountName", accountName);
                    dynamicPrameters.Add("Id", accountId);
                }
                var response = dbConnection.QueryFirstOrDefault<bool>(sqlCommand, param: dynamicPrameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy danh sách khoản thu có lọc
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên 1 trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách khoản thu tương ứng</returns>
        /// CreatedBy: VDDong (12/11/2021)
        public IEnumerable<Account> GetAccounts(int pageSize, int pageIndex, string filter)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetAccountFilter";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pageIndex", pageIndex);
                dynamicParameters.Add("@pageSize", pageSize);
                dynamicParameters.Add("@filter", filter);
                var response = dbConnection.Query<Account>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy số lượng khoản thu sau khi lọc
        /// </summary>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Số lương (int) khoản thu</returns>
        /// CreatedBy: VDDong (12/11/2021)
        public int GetTotalAccounts(string filter)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetTotalAccounts";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@filter", filter);

                var response = dbConnection.QueryFirstOrDefault<int>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }
    }
}
