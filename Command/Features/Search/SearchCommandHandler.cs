using Data.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Command.Features.Search;

public record SearchCommand(string HotelId, int Days, string RoomType): IRequest<IEnumerable<SearchCommandResult>>;

public class SearchCommandHandler: IRequestHandler<SearchCommand, IEnumerable<SearchCommandResult>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;

    public SearchCommandHandler(IBookingRepository bookingRepository, IHotelRepository hotelRepository)
    {
        ArgumentNullException.ThrowIfNull(bookingRepository);
        ArgumentNullException.ThrowIfNull(hotelRepository);

        _bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
    }

    public async Task<IEnumerable<SearchCommandResult>> Handle(SearchCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        if(command.Days < 1)
        {
            throw new ValidationException("Days must be a positive number");
        }

        var hotel = await _hotelRepository.GetHotelById(command.HotelId, ct);
        if (hotel == null) 
        {
            throw new ValidationException("Hotel with provided id does not exist");
        }

        if (!hotel.RoomTypes.Any(rt => rt.Code == command.RoomType))
        {
            throw new ValidationException("Selected hotel does not offer rooms with provded type");
        }

        var allRooms = hotel.Rooms.Where(x => x.RoomType == command.RoomType).Count();

        var bookings = await _bookingRepository.GetByHotelId(command.HotelId, command.RoomType, ct);        

        var startDate = DateOnly.FromDateTime(DateTime.Today.Date); 
        var endDate = startDate.AddDays(command.Days-1);

        var roomsAvailability = new Dictionary<DateOnly, int>();
        for(var d = startDate; d <= endDate; d = d.AddDays(1))
        {
            roomsAvailability[d] = allRooms;
        }

        foreach (var booking in bookings)
        {
            if(booking.Departure < startDate || booking.Arrival > endDate)
            {
                continue;
            }

            var firstDay = booking.Arrival < startDate ? startDate : booking.Arrival;
            var endDay = booking.Departure < endDate ? booking.Departure.AddDays(-1) : endDate;

            for (var d = firstDay; d <= endDay; d = d.AddDays(1))
            {
                roomsAvailability[d]--;
            }
        }
        var f = roomsAvailability.First();
        
        var results = new List<SearchCommandResult>();

        var availabilityResult = new SearchCommandResult
        {
            Availability = f.Value,
            StartDate = f.Key,
            EndDate = f.Key,
        };

        foreach (var (day, availability) in roomsAvailability)
        {
            if(availability != availabilityResult.Availability)
            {
                results.Add(availabilityResult);

                availabilityResult = new SearchCommandResult
                {
                    Availability = availability,
                    StartDate = day,
                    EndDate = day,
                };
            }

            availabilityResult.EndDate = day;
        }

        if (!results.Contains(availabilityResult))
        {
            results.Add(availabilityResult);
        }

        return results;
    }
}
