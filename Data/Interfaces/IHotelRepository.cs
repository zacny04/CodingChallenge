using Domain.Models;

namespace Data.Interfaces;

public interface IHotelRepository
{
    Task<Hotel?> GetHotelById(string hotelId, CancellationToken ct);
}
