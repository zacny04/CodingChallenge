using Data.Dao;
using Data.Mappers;
using FluentAssertions;

namespace Data.UnitTests.Mappers;

public class HotelMapperTests
{
    [Fact]
    public void HotelMapper_MapToModel_MapsCorrectly()
    {
        // Arrange
        var dao = new HotelDao
        {
            Id = "hotel id",
            Name = "name",
            Rooms = [new RoomDao { }],
            RoomTypes = [new RoomTypeDao { }, new RoomTypeDao { }]
        };

        // Act
        var result = HotelMapper.ToModel(dao);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("name");
        result.Id.Should().Be("hotel id");
        result.Rooms.Should().HaveCount(1);
        result.RoomTypes.Should().HaveCount(2);
    }
}
