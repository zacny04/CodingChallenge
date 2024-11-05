using Data.Config;
using Data.Dao;
using Data.Interfaces;
using Data.Mappers;
using Domain.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Data.Repository;

internal class HotelRepository : IHotelRepository
{
    private readonly string _hotelFileLocation;

    public HotelRepository(IOptions<RepoConfiguration> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _hotelFileLocation = options.Value.HotelFile;
    }

    public async Task<Hotel?> GetHotelById(string hotelId, CancellationToken ct)
    {
        using var sr = new StreamReader(_hotelFileLocation);

        var hotels = await JsonSerializer.DeserializeAsync<IEnumerable<HotelDao>>(sr.BaseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken: ct);

        var hotel = hotels?.FirstOrDefault(x => x.Id == hotelId);
        if (hotel == null)
        {
            return null;
        }

        return HotelMapper.ToModel(hotel);
    }
}
