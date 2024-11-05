using Data.Dao;
using Data.Mappers;
using FluentAssertions;

namespace Data.UnitTests.Mappers;

public class RoomTypeMapperTests
{
    [Fact]
    public void RoomTypeMapper_MapToModel_MapsCorrectly()
    {
        // Arrange
        var dao = new RoomTypeDao
        {
            Amenities = ["amenity"],
            Features = ["feature"],
            Code = "code",
            Description = "description",
        };

        // Act
        var result = RoomTypeMapper.ToModel(dao);

        // Assert
        result.Should().NotBeNull();
        result.Features.Should().BeEquivalentTo(dao.Features);
        result.Amenities.Should().BeEquivalentTo(dao.Amenities);
        result.Code.Should().Be(dao.Code);
        result.Description.Should().Be(dao.Description);
    }
}
