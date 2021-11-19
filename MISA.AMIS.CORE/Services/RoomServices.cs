using MISA.AMIS.CORE.Entities;
using MISA.AMIS.CORE.Interfaces.Repositories;
using MISA.AMIS.CORE.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Services
{
    public class RoomServices : BaseServices<Room>, IRoomServices
    {
        IRoomRepository _roomRepository;
        public RoomServices(IRoomRepository roomRepository) : base(roomRepository)
        {
            _roomRepository = roomRepository;
        }
    }
}
