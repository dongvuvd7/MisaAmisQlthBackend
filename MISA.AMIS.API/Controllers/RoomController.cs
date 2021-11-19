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
    public class RoomController : BaseController<Room>
    {
        IRoomRepository _roomRepository;
        IRoomServices _roomServices;
        public RoomController(IRoomRepository roomRepository, IRoomServices roomServices) : base(roomRepository, roomServices)
        {
            _roomRepository = roomRepository;
            _roomServices = roomServices;
        }
    }
}
