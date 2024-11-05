using Data.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Command.Features.GetAvailability;

public record GetAvailabilityCommand(string HotelId, DateOnly StartDate, DateOnly EndDate, string RoomType): IRequest<int>;

public class GetAvailabilityCommandHandler: IRequestHandler<GetAvailabilityCommand, int>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;

    public GetAvailabilityCommandHandler(IBookingRepository bookingRepository, IHotelRepository hotelRepository)
    {
        ArgumentNullException.ThrowIfNull(bookingRepository);
        ArgumentNullException.ThrowIfNull(hotelRepository);

        _bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
    }

    public async Task<int> Handle(GetAvailabilityCommand command, CancellationToken ct)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if(command.EndDate < command.StartDate)
        {
            throw new ValidationException("Start date must be before End Date");
        }

        var hotel = await _hotelRepository.GetHotelById(command.HotelId, ct);

        if (hotel == null)
        {
            throw new ValidationException("There is no hotel with provided id");
        }

        if (!hotel.RoomTypes.Any(h => h.Code == command.RoomType)) 
        {
            throw new ValidationException("Provided room type is not available for selected hotel");
        }

        var allRoomAmount = hotel.Rooms.Where(r => r.RoomType == command.RoomType).Count();
        
        var bookings = await _bookingRepository.GetByHotelId(command.HotelId, command.RoomType, ct);
        var highestBooking = 0;
        for(var date = command.StartDate; date <= command.EndDate; date = date.AddDays(1))
        {
            // we assume that other guest can arrive at the departure date of another guest
            var bookingAmount = bookings.Where(b => b.Arrival <= date && b.Departure > date).Count();
            if (bookingAmount > highestBooking) { 
                highestBooking = bookingAmount;
            }
        }

        return allRoomAmount - highestBooking;
    }
}
