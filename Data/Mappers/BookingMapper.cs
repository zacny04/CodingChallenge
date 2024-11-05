using Data.Dao;
using Domain.Models;

namespace Data.Mappers;

internal static class BookingMapper
{
    public static Booking MapToModel(BookingDao dao) => new Booking
    {
        HotelId = dao.HotelId,
        RoomType = dao.RoomType,
        RoomRate = dao.RoomRate,
        Arrival = DateOnly.ParseExact(dao.Arrival, "yyyyMMdd"),
        Departure = DateOnly.ParseExact(dao.Departure, "yyyyMMdd")
    };
}
