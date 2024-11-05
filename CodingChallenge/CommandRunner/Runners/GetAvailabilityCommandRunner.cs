using CodingChallenge.App.CommandRunner.Enums;
using CodingChallenge.App.CommandRunner.Interfaces;
using Command.Features.GetAvailability;
using Domain.Exceptions;
using MediatR;

namespace CodingChallenge.App.CommandRunner.Runners;

public class GetAvailabilityCommandRunner : ICommandRunner
{
    private readonly IMediator _mediator;

    public GetAvailabilityCommandRunner(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    public string CommandName => CommandType.Availability;

    public async Task Run(ParsedCommand command)
    {
        var parameters = command.Parameters;
        if (parameters.Length != 3)
        {
            throw new ValidationException("Cannot check availability, wrong argument count");
        }
        var hotelType = parameters[0];
        DateOnly startDate, endDate;

        if (parameters[1].Contains('-'))
        {
            var dates = parameters[1].Split('-');
            if (dates.Length != 2)
            {
                throw new ValidationException("Cannot check availability, wrong date period");
            }

            if (!DateOnly.TryParseExact(dates[0], "yyyyMMdd", out startDate))
            {
                throw new ValidationException("Provided date is in a wrong format");
            }

            if (!DateOnly.TryParseExact(dates[1], "yyyyMMdd", out endDate))
            {
                throw new ValidationException("Provided date is in a wrong format");
            }
        }
        else
        {
            if (!DateOnly.TryParseExact(parameters[1], "yyyyMMdd", out var date))
            {
                throw new ValidationException("Provided date is in a wrong format");
            }
            startDate = date;
            endDate = date;
        }

        var roomType = parameters[2];

        var request = new GetAvailabilityCommand(hotelType, startDate, endDate, roomType);

        var result = await _mediator.Send(request);

        Console.WriteLine(result);
    }
}
