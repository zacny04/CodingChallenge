using Data.Dao;
using Data.Mappers;
using FluentAssertions;

namespace Data.UnitTests.Mappers;

public class BookingMapperTests
{
    [Fact]
    public void BookingMapper_MapToModel_MapsCorrectly()
    {
        // Arrange
        var dao = new BookingDao
        {
            HotelId = "hotel",
            Arrival = "20241103",
            Departure = "20241104",
            RoomRate = "Room Rate",
            RoomType = "room type",
        };

        // Act
        var result = BookingMapper.MapToModel(dao);

        // Assert
        result.Should().NotBeNull();
        result.HotelId.Should().Be(dao.HotelId);
        result.RoomType.Should().Be(dao.RoomType);
        result.RoomRate.Should().Be(dao.RoomRate);

        result.Arrival.Should().Be(new DateOnly(2024, 11, 3));
        result.Departure.Should().Be(new DateOnly(2024, 11, 4));
    }
}
