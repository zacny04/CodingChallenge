using Command.Features.GetAvailability;
using Data.Interfaces;
using Domain.Exceptions;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Command.UnitTests.Features;

public  class GetAvailabilityCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly Mock<IBookingRepository> _bookingRepositoryMock = new();

    private readonly GetAvailabilityCommandHandler _handler;
    public GetAvailabilityCommandHandlerTests()
    {
        _handler = new(_bookingRepositoryMock.Object, _hotelRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NullCommand_ThrowsException()
    {
        // Arrange
        GetAvailabilityCommand command = null;

        // act
        var act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();

    }

    [Fact]
    public async Task Handle_EndDateBeforeStartDate_ThrowsException()
    {
        // Arrange
        var command = new GetAvailabilityCommand("", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), "");

        // Act
        var act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_HotelDoesNotExist_ThrowsException()
    {
        // Arrange
        var command = new GetAvailabilityCommand("does not exist", DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(5)), "");

        // Act
        var act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_WrongHotelType_ThrowsException()
    {
        // Arrange
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
        var command = new GetAvailabilityCommand(hotelName, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(5)), "Requested code");

        // Act
        var act = async () => await _handler.Handle(command, default);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_MultipleBookings_ReturnsLowestValue()
    {
        // Arrange
        var hotelName = "hotel";
        var roomCode = "sgl";

        var arrival1 = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var departure1 = DateOnly.FromDateTime(DateTime.Now.AddDays(3));

        var arrival2 = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        var departure2 = DateOnly.FromDateTime(DateTime.Now.AddDays(3));

        _hotelRepositoryMock.Setup(r => r.GetHotelById(hotelName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Hotel()
            {
                Id = hotelName,
                RoomTypes = new List<RoomType>()
                {
                    new RoomType
                    {
                        Code = roomCode,
                    }
                },
                Rooms = Enumerable.Range(0, 5).Select(x => new Room { RoomId = x.ToString(), RoomType = roomCode })
            });

        _bookingRepositoryMock.Setup(b => b.GetByHotelId(hotelName, roomCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                new Booking {
                    Arrival = arrival1,
                    Departure = departure1,
                },
                new Booking {
                    Arrival = arrival2,
                    Departure = departure2,
                }
            ]);

        var command = new GetAvailabilityCommand(hotelName, DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(5)), roomCode);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Should().Be(3);

    }
}
