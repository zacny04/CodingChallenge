using Data.Dao;
using Data.Mappers;
using FluentAssertions;

namespace Data.UnitTests.Mappers;

public class RoomMapperTests
{
    [Fact]
    public void RoomMapper_MapToModel_MapsCorrectly()
    {
        // Arrange
        var dao = new RoomDao
        {
            RoomId = "room id",
            RoomType = "room type",
        };

        // Act
        var result = RoomMapper.ToModel(dao);

        // Assert
        result.Should().NotBeNull();
        result.RoomType.Should().Be(dao.RoomType);
        result.RoomId.Should().Be(dao.RoomId);
    }
}
