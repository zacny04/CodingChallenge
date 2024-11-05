using Data.Config;
using Data.Dao;
using Data.Interfaces;
using Data.Mappers;
using Domain.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Data.Repository;

internal class BookingRepository : IBookingRepository
{
    private readonly string _bookingFileLocation;

    public BookingRepository(IOptions<RepoConfiguration> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _bookingFileLocation = options.Value.BookingFile;
    }

    public async Task<IEnumerable<Booking>> GetByHotelId(string hotelId, CancellationToken ct)
    {
        using var sr = new StreamReader(_bookingFileLocation);

        var bookings = await JsonSerializer.DeserializeAsync<IEnumerable<BookingDao>>(sr.BaseStream, cancellationToken: ct);

        return bookings.Where(x => x.HotelId == hotelId).Select(BookingMapper.MapToModel);
    }

    public async Task<IEnumerable<Booking>> GetByHotelId(string hotelId, string roomType, CancellationToken ct)
    {
        using var sr = new StreamReader(_bookingFileLocation);

        var bookings = await JsonSerializer.DeserializeAsync<IEnumerable<BookingDao>>(sr.BaseStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken: ct);

        return bookings.Where(x => x.HotelId == hotelId && x.RoomType == roomType).Select(BookingMapper.MapToModel);
    }
}
