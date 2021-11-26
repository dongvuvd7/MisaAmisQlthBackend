using Dapper;
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
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity> where MISAEntity : class
    {
        #region Connection
        protected string connectionString = "" +
            "Host = 47.241.69.179; " +
            "Port = 3306; " +
            "Database = MISAAMIS_MF975_VDD; " +
            "User Id = nvmanh; " +
            "Password = 12345678; " +
            "Convert Zero Datetime = True;";

        protected IDbConnection dbConnection;
        #endregion

        #region Methods
        string tableName = typeof(MISAEntity).Name;

        /// <summary>
        /// Xóa bản ghi trong database
        /// </summary>
        /// <param name="entityId">Id của bản ghi muốn xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public int Delete(Guid entityId)
        {
            using(dbConnection = new MySqlConnection(connectionString))
            {
                dbConnection.Open();
                var sqlTransaction = dbConnection.BeginTransaction();

                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add($"@{tableName}Id", entityId);
                var rowsAffects = dbConnection.Execute($"Proc_Delete{tableName}", param: dynamicParameters, commandType: CommandType.StoredProcedure, transaction: sqlTransaction);
                sqlTransaction.Commit();
                return rowsAffects;
            }
        }

        /// <summary>
        /// Lấy tất cả bản ghi từ database
        /// </summary>
        /// <returns>Tất cả bản ghi</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public IEnumerable<MISAEntity> GetAll()
        {
            using(dbConnection = new MySqlConnection(connectionString))
            {
                var response = dbConnection.Query<MISAEntity>($"Proc_Get{tableName}s", commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy bản ghi theo id
        /// </summary>
        /// <param name="entityId">id của bản ghi muốn lấy</param>
        /// <returns>Trả về bản ghi tương ứng với id</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public MISAEntity GetById(Guid entityId)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add($"@{tableName}Id", entityId);
                var response = dbConnection.QueryFirstOrDefault<MISAEntity>($"Proc_Get{tableName}ById", param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>Số lượng bản ghi mỗi tranng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public IEnumerable<MISAEntity> GetPaging(int pageIndex, int pageSize)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@m_PageIndex", pageIndex);
                dynamicParameters.Add("@m_PageSize", pageSize);
                var response = dbConnection.Query<MISAEntity>($"Proc_Get{tableName}Paging", param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Thêm mới dữ liệu vào database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số lượng bản ghi ảnh hưởng (thêm được)</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public int Post(MISAEntity entity)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                dbConnection.Open();
                var sqlTransaction = dbConnection.BeginTransaction();
                var rowAffects = dbConnection.Execute($"Proc_Insert{tableName}", param: entity, commandType: CommandType.StoredProcedure, transaction: sqlTransaction);
                sqlTransaction.Commit();
                return rowAffects;
            }
        }

        /// <summary>
        /// Chỉnh sửa dữ liệu trong database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số bản ghi ảnh hưởng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public int Put(MISAEntity entity)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                dbConnection.Open();
                var sqlTransaction = dbConnection.BeginTransaction();
                var rowAffects = dbConnection.Execute($"Proc_Update{tableName}", param: entity, commandType: CommandType.StoredProcedure, transaction: sqlTransaction);
                sqlTransaction.Commit();
                return rowAffects;
            }
        }
        #endregion
    }
}
