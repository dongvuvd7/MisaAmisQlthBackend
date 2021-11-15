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
        /// Lấy các ngân hàng liên kết với nhân viên theo UserId (mã nhân viên)
        /// Phục vụ việc bind danh sách các ngân hàng của nhân viên lên tab ngân hàng
        /// </summary>
        /// <param name="UserId">Mã nhân viên</param>
        /// <returns>Danh sách các bản ghi ngân hàng tương ứng</returns>
        /// CreatedBy: VDDong (02/11/2021)
        public IEnumerable<BankEmp> GetBankEmpByUserId(Guid UserId)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetBankEmpByUserId";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("UserId", UserId);
                var response = dbConnection.Query<BankEmp>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
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
        /// Lấy bản ghi nhân viên theo EmployeeCode (mã nhân viên)
        /// Phục vụ việc lấy EmployeeId làm khóa ngoại để post các bankEmp lên
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>bản ghi nhân viên tương ứng</returns>
        /// CreatedBy: VDDong (02/11/2021)
        public Employee GetEmployeeByEmployeeCode(string employeeCode)
        {
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetEmployeeByEmployeeCode";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@EmployeeCode", employeeCode);
                var response = dbConnection.QueryFirstOrDefault<Employee>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
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
        /// Lấy danh sách nhân viên theo 2 cách:
        /// 1. Sắp xếp theo code (theo thứ tự thêm mới)
        /// 2. Sắp xếp theo tên (thứ tự anphabe)
        /// Kết hợp với nhóm theo phòng ban (đơn vị)
        /// </summary>
        /// <param name="pageSize">số bản ghi / trang</param>
        /// <param name="pageIndex">số trang</param>
        /// <param name="departmentString">điều kiện để nhóm phòng ban</param>
        /// <returns></returns>
        public IEnumerable<Employee> GetEmployeesSortByCode(int pageSize, int pageIndex, string departmentString)
        {
            if (departmentString == null) departmentString = "";
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetEmployeesSortByCode";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pageSize", pageSize);
                dynamicParameters.Add("@pageIndex", pageIndex);
                dynamicParameters.Add("@departmentId", departmentString);
                var responses = dbConnection.Query<Employee>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return responses;
            }
        }
        public IEnumerable<Employee> GetEmployeesSortByName(int pageSize, int pageIndex, string departmentString)
        {
            if (departmentString == null) departmentString = "";
            using (dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetEmployeesSortByName";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pageSize", pageSize);
                dynamicParameters.Add("@pageIndex", pageIndex);
                dynamicParameters.Add("@departmentId", departmentString);
                var responses = dbConnection.Query<Employee>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return responses;
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

        /// <summary>
        /// Lấy tổng số bản ghi theo điều kiện (sắp xếp và nhóm phòng ban(đơn vị))
        /// Phục vụ việc làm totalRecord cho phân trang khi sắp xếp và nhóm phòng ban
        /// </summary>
        /// <param name="departmentString"></param>
        /// <returns>Số bản ghi (int) theo điều kiện</returns>
        public int GetTotalEmployeesSortBy(string departmentString)
        {
            if (departmentString == null) departmentString = "";
            using(dbConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "Proc_GetTotalEmployeeSortBy";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@departmentId", departmentString);
                var responses = dbConnection.QueryFirstOrDefault<int>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return responses;
            }
        }
    }
}
