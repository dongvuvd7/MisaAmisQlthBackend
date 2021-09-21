using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.CORE.Entities;
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
    public class NccController : BaseController<Ncc>
    {
        INccRepository _nccRepository;
        INccServices _nccServices;

        public NccController(INccRepository nccRepository, INccServices nccServices) : base(nccRepository, nccServices)
        {
            _nccRepository = nccRepository;
            _nccServices = nccServices;
        }

        [HttpGet("GetAllBankNccByUserId/{UserId}")]
        public IActionResult GetBankNccByUserId(Guid UserId)
        {
            var responses = _nccServices.GetBankNccByUserId(UserId);
            if (responses.Count() > 0) return Ok(responses);
            else return NoContent();
        }

        [HttpGet("MaxCode")]
        public IActionResult GetMaxCode()
        {
            var response = _nccServices.GetMaxCode();
            if (response != null) return Ok(response);
            else return NoContent();
        }

        [HttpGet("Filter")]
        public IActionResult GetNccs(int pageSize, int pageIndex, string filter)
        {
            var totalRecord = _nccRepository.GetTotalNccs(filter);
            var nccs = _nccServices.GetNccs(pageSize, pageIndex, filter);
            var response = new
            {
                totalRecord = totalRecord,
                data = nccs
            };
            if (response.data.Any() && response.totalRecord != 0)
            {
                return Ok(response);
            }
            else return NoContent();
        }

        [HttpGet("Search/{searchResult}")]
        public IActionResult GetSearchResult(string searchResult)
        {
            var responses = _nccServices.GetSearchResult(searchResult);
            if (responses.Count() > 0) return Ok(responses);
            else return NoContent();
        }

        /// <summary>
        /// Lấy tổng số bản ghi nhà cung cấp
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Số bản ghi</returns>
        /// CreatedBy: VDDong (22/08/2021)
        [HttpGet("TotalRecordNccs")]
        //Nếu muốn bắt buộc phải nhập tham số filter thì viết TotalRecordNccs/{filter} -> url bắt buộc phải có cả filter mới chạy
        public IActionResult GetTotalNccs(string filter)
        {
            var responses = _nccServices.GetTotalNccs(filter);
            if (responses >= 0) return Ok(responses);
            else return NoContent();
        }

        [HttpGet("GetNccByNccCode/{nccCode}")]
        public IActionResult GetNccByNccCode(string nccCode)
        {
            var response = _nccServices.GetNccByNccCode(nccCode);
            if (response != null) return Ok(response);
            else return NoContent();
        }

        /// <summary>
        /// Xuất dữ liệu ncc ra file excel
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>file excel</returns>
        /// CreatedBy: VDDong (09/06/2021)
        [HttpGet("Export")]
        public IActionResult Export()
        {
            var stream = _nccServices.ExportExcel();
            string excelName = $"NCC_Data(ExportByVVDong).xlsx";
            //return File(stream, "application/octet-stream", excelName);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

        }


    }
}
