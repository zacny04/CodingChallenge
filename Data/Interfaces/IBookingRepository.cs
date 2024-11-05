using Domain.Models;

namespace Data.Interfaces;

public interface IBookingRepository
{
    Task<IEnumerable<Booking>> GetByHotelId(string hotelId, CancellationToken ct);
    Task<IEnumerable<Booking>> GetByHotelId(string hotelId, string roomType, CancellationToken ct);
}
