using Command.Features.Search;
using Data.Interfaces;
using Domain.Exceptions;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Command.UnitTests.Features;

public class SearchCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly Mock<IBookingRepository> _bookingRepositoryMock = new();

    private readonly SearchCommandHandler _handler;
    public SearchCommandHandlerTests()
    {
        _handler = new(_bookingRepositoryMock.Object, _hotelRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NullCommand_ThrowsException()
    {
        // Arrange & Act
        var act = async () => await _handler.Handle(null, default);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task Handle_DaysLessThanOne_ThrowsException()
    {
        // Arrange
        var command = new SearchCommand("", 0, "");
        // Act
        var act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_WrongHotelId_ThrowsException()
    {
        // Arrange
        var command = new SearchCommand("hotelId", 5, "");
        // Act
        var act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_WrongRoomType_ThrowsException()
    {
        var hotelName = "hotel";
        _hotelRepositoryMock.Setup(r => r.GetHotelById(hotelName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Hotel()
            {
                Id = hotelName,
                RoomTypes = new List<RoomType>()
                {
                    new RoomType
                    {
                        Code = "Other code"
                    }
                }
            });
        var command = new SearchCommand(hotelName, 5, "Requested code");

        // Act
        var act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_ReturnsArray()
    {

    }
}
