using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MISA.AMIS.CORE.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.AMIS.API.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class BaseController<MISAEntity> : ControllerBase where MISAEntity : class
    {
        IBaseServices<MISAEntity> _dataAccessBaseServices;

        public BaseController(IBaseRepository<MISAEntity> baseRepository, IBaseServices<MISAEntity> dataAccessBaseServices)
        {
            _dataAccessBaseServices = dataAccessBaseServices;
        }

        /// <summary>
        /// Lấy tất cả các bản ghi từ database
        /// </summary>
        /// <returns>Tất cả các bản ghi</returns>
        /// <response code="200">có dữ liệu trả về -> lấy thành công</response>
        /// <response code="204">không có dữ liệu trả về -> lấy thất bại</response>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _dataAccessBaseServices.GetAll();
            if (entities.Count() > 0) return Ok(entities);
            else return NoContent();
        }

        /// <summary>
        /// Lấy một bản ghi từ database theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Bản ghi tương ứng</returns>
        /// <response code="200">có dữ liệu trả về -> lấy thành công</response>
        /// <response code="204">không có dữ liệu trả về -> lấy thất bại</response>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpGet("{entityId}")]
        public IActionResult GetById(Guid entityId)
        {
            var entity = _dataAccessBaseServices.GetById(entityId);
            if (entity != null) return Ok(entity);
            else return NoContent();
        }

        /// <summary>
        /// Insert 1 bản ghi vào database 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// <response code="200">có số lượng bản ghi bị ảnh hưởng, thêm thành công</response>
        /// <response code="204">không có số lượng bản ghi bị ảnh hưởng, thêm thất bại</response>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpPost]
        public IActionResult Post([FromBody] MISAEntity entity)
        {
            var rowsAffect = _dataAccessBaseServices.Post(entity);
            if (rowsAffect > 0) return Ok(rowsAffect);
            else return NoContent();
        }

        /// <summary>
        /// Insert 1 bản ghi vào database 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="entity"></param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// <response code="200">có số lượng bản ghi bị ảnh hưởng, thêm thành công</response>
        /// <response code="204">không có số lượng bản ghi bị ảnh hưởng, thêm thất bại</response>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] MISAEntity entity)
        {
            var properties = typeof(MISAEntity).GetProperties();
            var tableName = typeof(MISAEntity).Name;
            foreach(var item in properties)
            {
                if(item.Name == $"{tableName}id")
                {
                    item.SetValue(entity, id);
                }
            }
            var rowsAffect = _dataAccessBaseServices.Put(entity);
            if (rowsAffect > 0) return Ok(rowsAffect);
            else return NoContent();
        }

        /// <summary>
        /// Xóa 1 bản ghi trong database 
        /// </summary>
        /// <param name="entityid"></param>
        /// <returns>Số lượng bản ghi bị ảnh hưởng</returns>
        /// <response code="200">có số lượng bản ghi bị ảnh hưởng, xóa thành công</response>
        /// <response code="204">không có số lượng bản ghi bị ảnh hưởng, xóa thất bại</response>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpDelete("{entityid}")]
        public IActionResult Delete(Guid entityid)
        {
            var rowsAffect = _dataAccessBaseServices.Delete(entityid);
            if (rowsAffect > 0) return Ok(rowsAffect);
            else return NoContent();
        }

        /// <summary>
        /// Lấy các bản ghi theo phân trang
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>Các bản ghi tương ứng phân trang</returns>
        /// <response code"200">lấy thành công</response>
        /// <response code"204">Lấy thất bại</response>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpGet("Paging")]
        public IActionResult GetPaging(int pageIndex, int pageSize)
        {
            var entities = _dataAccessBaseServices.GetPaging(pageIndex, pageSize);
            if (entities.Count() > 0) return Ok(entities);
            else return NoContent();
        }


    }
}
