using Data.Config;
using Data.Repository;
using Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Data.UnitTests.Repository;

public class HotelRepositoryTests
{
    [Fact]
    public async Task GetByHotelId_ReturnsData()
    {
        // Arrange
        List<Hotel> hotels = [
            new(){
                Id = "hotelId",
                Name = "Name",
                Rooms = [],
                RoomTypes = [],
            }
            ];

        PrepareTestFile("hotels.json", hotels);

        var options = Options.Create(new RepoConfiguration
        {
            HotelFile = "hotels.json"
        });

        var repository = new HotelRepository(options);

        // Act
        var result = await repository.GetHotelById("hotelId", default);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByHotelId_WrongId_ReturnsNull()
    {
        // Arrange
        List<Hotel> hotels = [
            new(){
                Id = "hotelId",
                Name = "Name",
                Rooms = [],
                RoomTypes = [],
            }
            ];

        PrepareTestFile("hotels.json", hotels);

        var options = Options.Create(new RepoConfiguration
        {
            HotelFile = "hotels.json"
        });

        var repository = new HotelRepository(options);

        // Act
        var result = await repository.GetHotelById("other id", default);

        // Assert
        result.Should().BeNull();
    }

    private static void PrepareTestFile<T>(string location, T data)
    {
        using var sw = new StreamWriter(location);

        JsonSerializer.Serialize(sw.BaseStream, data);
    }
}


