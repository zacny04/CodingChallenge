using Data.Dao;
using Domain.Models;

namespace Data.Mappers;

internal static class HotelMapper
{
    public static Hotel ToModel(HotelDao dao) => new()
    {
        Id = dao.Id,
        Name = dao.Name,
        Rooms = dao.Rooms.Select(RoomMapper.ToModel),
        RoomTypes = dao.RoomTypes.Select(RoomTypeMapper.ToModel),
    };
}
