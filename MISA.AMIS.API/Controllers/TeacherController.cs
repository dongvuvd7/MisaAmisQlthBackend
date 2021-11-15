using Microsoft.AspNetCore.Http;
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
    public class TeacherController : BaseController<Teacher>
    {
        ITeacherRepository _teacherRepository;
        ITeacherServices _teacherServices;
        public TeacherController(ITeacherRepository teacherRepository, ITeacherServices teacherServices) : base(teacherRepository, teacherServices)
        {
            _teacherRepository = teacherRepository;
            _teacherServices = teacherServices;
        }


    }
}
