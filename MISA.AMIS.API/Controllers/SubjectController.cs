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
    public class SubjectController : BaseController<Subject>
    {
        ISubjectRepository _subjectRepository;
        ISubjectServices _subjectServices;

        public SubjectController(ISubjectRepository subjectRepository, ISubjectServices subjectServices) : base(subjectRepository, subjectServices)
        {
            _subjectRepository = subjectRepository;
            _subjectServices = subjectServices;
        }

    }
}
