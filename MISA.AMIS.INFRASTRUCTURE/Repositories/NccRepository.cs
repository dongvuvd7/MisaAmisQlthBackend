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
    public class NccRepository : BaseRepository<Ncc>, INccRepository
    {
        public bool CheckDuplicateNccCode(string nccCode, Guid nccId, HttpType http)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "";
                DynamicParameters dynamicPrameters = new DynamicParameters();
                if (http == HttpType.POST)
                {
                    sqlCommand = "Proc_CheckNccCodeExist";
                    dynamicPrameters.Add("NccCode", nccCode);
                }
                else
                {
                    sqlCommand = "Proc_CheckNccCodeExistPut";
                    dynamicPrameters.Add("NccCode", nccCode);
                    dynamicPrameters.Add("Id", nccId);
                }
                var response = dbConnection.QueryFirstOrDefault<bool>(sqlCommand, param: dynamicPrameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        public IEnumerable<BankNcc> GetBankNccByUserId(Guid UserId)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetBankNccByUserId";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("UserId", UserId);
                var response = dbConnection.Query<BankNcc>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        public IEnumerable<BankNcc> GetBankNccName()
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetBankNccs";
                var response = dbConnection.Query<BankNcc>(sqlCommand, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        public string GetMaxCode()
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetMaxNccCode";
                var response = dbConnection.QueryFirstOrDefault<string>(sqlCommand, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        public IEnumerable<Ncc> GetNccs(int pageSize, int pageIndex, string filter)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetNccFilter";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pageIndex", pageIndex);
                dynamicParameters.Add("@pageSize", pageSize);
                dynamicParameters.Add("@filter", filter);
                var response = dbConnection.Query<Ncc>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        public IEnumerable<Ncc> GetSearchResult(string searchResult)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_SearchNcc";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("SearchField", searchResult);
                var response = dbConnection.Query<Ncc>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        public int GetTotalNccs(string filter)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetTotalNccs";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@filter", filter);
                var response = dbConnection.QueryFirstOrDefault<int>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }
    }
}
