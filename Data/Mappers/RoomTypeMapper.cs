using Data.Dao;
using Domain.Models;

namespace Data.Mappers
{
    internal static class RoomTypeMapper
    {
        public static RoomType ToModel(RoomTypeDao dao) => new()
        {
            Code = dao.Code,
            Description = dao.Description,
            Amenities = dao.Amenities,
            Features = dao.Features,
        };
    }
}
