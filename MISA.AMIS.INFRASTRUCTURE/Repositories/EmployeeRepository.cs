using Dapper;
using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Enums;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MySqlConnector;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace MISA.AMIS.INFRASTRUCTURE.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        /// <summary>
        /// Kiểm tra trùng mã nhân viên cho việc post và put (2 cái check khác nhau)
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeId"></param>
        /// <param name="http">kiểu post / put</param>
        /// <returns>true-trùng / false-không trùng</returns>
        /// CreatedBy: VDDong (08/06/2021)
        public bool CheckDuplicateEmployeeCode(string employeeCode, Guid employeeId, HttpType http)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "";
                DynamicParameters dynamicPrameters = new DynamicParameters();
                if(http == HttpType.POST)
                {
                    sqlCommand = "Proc_CheckEmployeeCodeExist";
                    dynamicPrameters.Add("EmployeeCode", employeeCode);
                }
                else
                {
                    sqlCommand = "Proc_CheckEmployeeCodeExistPut";
                    dynamicPrameters.Add("EmployeeCode", employeeCode);
                    dynamicPrameters.Add("Id", employeeId);
                }
                var response = dbConnection.QueryFirstOrDefault<bool>(sqlCommand, param: dynamicPrameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy phòng ban dựa theo id
        /// </summary>
        /// <param name="departmentId">id của phòng ban cần lấy</param>
        /// <returns>phòng ban tương ứng</returns>
        /// CreatedBy: VDDong (08/06/2021)
        public EmployeeDepartment GetDepartmentById(Guid departmentId)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetDepartmentById";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("departmentId", departmentId);
                var response = dbConnection.QueryFirstOrDefault<EmployeeDepartment>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy tên các phòng ban từ database
        /// </summary>
        /// <returns>Tên các phòng ban</returns>
        /// CreatedBy: VDDong (08/06/2021)
        public IEnumerable<EmployeeDepartment> GetDepartmentName()
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetDepartments";
                var response = dbConnection.Query<EmployeeDepartment>(sqlCommand, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy danh sách nhân viên có lọc
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="filter"></param>
        /// <returns>Danh sách nhân viên sau khi lọc</returns>
        /// CreatedBy: VDDong (08/06/2021)
        public IEnumerable<Employee> GetEmployees(int pageSize, int pageIndex, string filter)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetEmployeeFilter";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pageIndex", pageIndex);
                dynamicParameters.Add("@pageSize", pageSize);
                dynamicParameters.Add("@filter", filter);
                var response = dbConnection.Query<Employee>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy mã nhân viên lớn nhất
        /// </summary>
        /// <returns>mã nhân viên lớn nhất</returns>
        /// CreatedBy: VDDong (08/06/2021)
        public string GetMaxCode()
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetMaxEmployeeCode";

                var response = dbConnection.QueryFirstOrDefault<string>(sqlCommand, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy các bản ghi từ database tìm kiếm theo mã hoặc tên nhân viên
        /// </summary>
        /// <param name="searchResult"></param>
        /// <returns>các bản ghi tương ứng</returns>
        /// CreatedBy: VDDong (08/06/2021)
        public IEnumerable<Employee> GetSearchResult(string searchResult)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_SearchEmployee";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("SearchField", searchResult);

                var response = dbConnection.Query<Employee>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }

        /// <summary>
        /// Lấy số lượng nhân viên sau khi lọc
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Trả về số lượng (int) nhân viên sau khi lọc</returns>
        public int GetTotalEmployees(string filter)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetTotalEmployees";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@filter", filter);

                var response = dbConnection.QueryFirstOrDefault<int>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return response;
            }
        }
    }
}
