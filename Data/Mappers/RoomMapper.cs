using Data.Dao;
using Domain.Models;

namespace Data.Mappers;

internal static class RoomMapper
{
    public static Room ToModel(RoomDao dao) => new()
    {
        RoomId = dao.RoomId,
        RoomType = dao.RoomType,
    };
}
