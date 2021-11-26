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
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        #region Methods
        /// <summary>
        /// Kiểm tra trùng mã giáo viên trong database
        /// </summary>
        /// <param name="teacherCode"></param>
        /// <param name="teacherId"></param>
        /// <param name="http"></param>
        /// <returns>true: trùng / false: không trùng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public bool CheckDuplicateTeacherCode(string teacherCode, Guid teacherId, HttpType http)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "";
                DynamicParameters dynamicPrameters = new DynamicParameters();
                if (http == HttpType.POST)
                {
                    sqlCommand = "Proc_CheckTeacherCodeExist";
                    dynamicPrameters.Add("TeacherCode", teacherCode);
                }
                else
                {
                    sqlCommand = "Proc_CheckTeacherCodeExistPut";
                    dynamicPrameters.Add("TeacherCode", teacherCode);
                    dynamicPrameters.Add("Id", teacherId);
                }
                var response = dbConnection.QueryFirstOrDefault<bool>(sqlCommand, param: dynamicPrameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi teacher
        /// </summary>
        /// <param name="recordIds">Chuỗi các id muốn xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public int DeleteMultipleTeacher(string recordIds)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                dbConnection.Open();
                var sqlTransaction = dbConnection.BeginTransaction();

                //DynamicParameters dynamicPrameters = new DynamicParameters();
                //dynamicPrameters.Add("@m_recordIds", recordIds);
                //var sqlCommand = $"DELETE FROM Teacher WHERE TeacherId IN @m_recordIds";
                //var rowsAffect = dbConnection.Execute(sqlCommand, param: dynamicPrameters, commandType: CommandType.Text);
                //return rowsAffect;

                var sqlCommand = $"DELETE FROM Teacher WHERE TeacherId IN {recordIds}";
                var rowsAffect = dbConnection.Execute(sqlCommand, transaction: sqlTransaction);
                sqlTransaction.Commit();
                return rowsAffect;

            }
        }

        /// <summary>
        /// Lấy tên tổ theo Id
        /// </summary>
        /// <param name="groupId">ID tổ</param>
        /// <returns>Trả về tổ theo Id tương ứng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public TeacherGroup GetGroupById(Guid groupId)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetGroupById";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("groupId", groupId);
                var response = dbConnection.QueryFirstOrDefault<TeacherGroup>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy tên các tổ
        /// </summary>
        /// <returns>Trả về tên các tổ</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public IEnumerable<TeacherGroup> GetGroupName()
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetGroups";
                var response = dbConnection.Query<TeacherGroup>(sqlCommand, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy mã giáo viên lớn nhất trong database
        /// </summary>
        /// <returns>Mã giáo viên lớn nhất</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public string GetMaxCode()
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetMaxTeacherCode";

                var response = dbConnection.QueryFirstOrDefault<string>(sqlCommand, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy dữ liệu giáo viên theo tìm kiếm theo mã hoặc tên giáo viên
        /// </summary>
        /// <param name="searchResult"></param>
        /// <returns>Kết quả sau tìm kiếm</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public IEnumerable<Teacher> GetSearchResult(string searchResult)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_SearchTeacher";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("SearchField", searchResult);

                var response = dbConnection.Query<Teacher>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy danh sách giáo viên có lọc
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên 1 trang</param>
        /// <param name="pageIndex">trang số bao nhiêu</param>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Danh sách giáo viên tương ứng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public IEnumerable<Teacher> GetTeachers(int pageSize, int pageIndex, string filter)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetTeacherFilter";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pageIndex", pageIndex);
                dynamicParameters.Add("@pageSize", pageSize);
                dynamicParameters.Add("@filter", filter);
                var response = dbConnection.Query<Teacher>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy danh sách giáo viên theo 2 cách:
        /// 1. Sắp xếp theo code (theo thứ tự thêm mới)
        /// 2. Sắp xếp theo tên (thứ tự anphabe)
        /// Kết hợp với nhóm theo tổ
        /// </summary>
        /// <param name="pageSize">số bản ghi / trang</param>
        /// <param name="pageIndex">số trang</param>
        /// <param name="groupString">điều kiện để nhóm tổ</param>
        /// <returns>Danh sách bản ghi tương ứng</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public IEnumerable<Teacher> GetTeachersSortByCode(int pageSize, int pageIndex, string groupString)
        {
            if (groupString == null) groupString = "";
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetTeachersSortByCode";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pageSize", pageSize);
                dynamicParameters.Add("@pageIndex", pageIndex);
                dynamicParameters.Add("@groupId", groupString);
                var responses = dbConnection.Query<Teacher>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return responses;
            }
        }

        public IEnumerable<Teacher> GetTeachersSortByName(int pageSize, int pageIndex, string groupString)
        {
            if (groupString == null) groupString = "";
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetTeachersSortByName";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pageSize", pageSize);
                dynamicParameters.Add("@pageIndex", pageIndex);
                dynamicParameters.Add("@groupId", groupString);
                var responses = dbConnection.Query<Teacher>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return responses;
            }
        }

        /// <summary>
        /// Lấy số lượng giáo viên sau khi lọc
        /// </summary>
        /// <param name="filter">chuỗi để lọc</param>
        /// <returns>Số lương (int) giáo viên</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public int GetTotalTeachers(string filter)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetTotalTeachers";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@filter", filter);

                var response = dbConnection.QueryFirstOrDefault<int>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy tổng số bản ghi theo điều kiện (sắp xếp và nhóm tổ)
        /// Phục vụ việc làm totalRecord cho phân trang khi sắp xếp và nhóm tổ
        /// </summary>
        /// <param name="groupString"></param>
        /// <returns>Số bản ghi (int) theo điều kiện</returns>
        /// CreatedBy: VDDong (19/11/2021)
        public int GetTotalTeachersSortBy(string groupString)
        {
            if (groupString == null) groupString = "";
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetTotalTeacherSortBy";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@groupId", groupString);
                var responses = dbConnection.QueryFirstOrDefault<int>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return responses;
            }
        }
        #endregion
    }
}
