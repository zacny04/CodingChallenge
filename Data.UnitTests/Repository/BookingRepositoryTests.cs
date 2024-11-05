using Data.Config;
using Data.Dao;
using Data.Repository;
using Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Data.UnitTests.Repository;

public class BookingRepositoryTests
{
    [Fact]
    public async Task GetByHotelId_ReturnsData()
    {
        // Arrange
        List<BookingDao> bookings = [
            new(){
                HotelId = "hotelId",
                Arrival = "20241103",
                Departure = "20241104",
                RoomRate = "",
                RoomType = "",
            },
            new(){
                HotelId = "hotelId",
                Arrival = "20241103",
                Departure = "20241104",
                RoomRate = "",
                RoomType = "",
            },
            new(){
                HotelId = "hotelId2",
                Arrival = "20241103",
                Departure = "20241104",
                RoomRate = "",
                RoomType = "",
            }
        ];

        PrepareTestFile("bookings.json", bookings);

        var options = Options.Create(new RepoConfiguration
        {
            BookingFile = "bookings.json"
        });

        var repository = new BookingRepository(options);

        // Act
        var result = await repository.GetByHotelId("hotelId", default);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByHotelId_WrongId_ReturnsEmptySet()
    {
        // Arrange
        List<BookingDao> bookings = [
            new(){
                HotelId = "hotelId",
                Arrival = "20241103",
                Departure = "20241104",
                RoomRate = "",
                RoomType = "",
            },
            new(){
                HotelId = "hotelId",
                Arrival = "20241103",
                Departure = "20241104",
                RoomRate = "",
                RoomType = "",
            },
            new(){
                HotelId = "hotelId2",
                Arrival = "20241103",
                Departure = "20241104",
                RoomRate = "",
                RoomType = "",
            }
        ];

        PrepareTestFile("bookings.json", bookings);

        var options = Options.Create(new RepoConfiguration
        {
            BookingFile = "bookings.json"
        });

        var repository = new BookingRepository(options);

        // Act
        var result = await repository.GetByHotelId("hotelId3", default);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByHotelId_RoomType_ReturnsFilteredData()
    {
        // Arrange
        List<BookingDao> bookings = [
            new(){
                HotelId = "hotelId",
                Arrival = "20241103",
                Departure = "20241104",
                RoomRate = "",
                RoomType = "S",
            },
            new(){
                HotelId = "hotelId",
                Arrival = "20241103",
                Departure = "20241104",
                RoomRate = "",
                RoomType = "D",
            },
            new(){
                HotelId = "hotelId2",
                Arrival = "20241103",
                Departure = "20241104",
                RoomRate = "",
                RoomType = "D",
            }
        ];

        PrepareTestFile("bookings.json", bookings);

        var options = Options.Create(new RepoConfiguration
        {
            BookingFile = "bookings.json"
        });

        var repository = new BookingRepository(options);

        // Act
        var result = await repository.GetByHotelId("hotelId", "D", default);

        // Assert
        result.Should().HaveCount(1);
    }

    private static void PrepareTestFile<T>(string location, T data)
    {
        using var sw = new StreamWriter(location);

        JsonSerializer.Serialize(sw.BaseStream, data);
    }
}


