using CodingChallenge.App.CommandRunner.Enums;
using CodingChallenge.App.CommandRunner.Interfaces;
using Command.Features.Search;
using Domain.Exceptions;
using MediatR;

namespace CodingChallenge.App.CommandRunner.Runners;

public class SearchCommandRunner : ICommandRunner
{
    public string CommandName => CommandType.Search;

    private readonly IMediator _mediator;

    public SearchCommandRunner(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    public async Task Run(ParsedCommand command)
    {
        var parameters = command.Parameters;
        if (parameters.Length != 3)
        {
            throw new ValidationException("Cannot perform search, wrong argument count");
        }
        var hotelType = parameters[0];
        if (!int.TryParse(parameters[1], out var days))
        {
            throw new ValidationException("Cannot perform search, wrong days amount");
        }

        var roomType = parameters[2];

        var request = new SearchCommand(hotelType, days, roomType);

        var result = await _mediator.Send(request);

        var text = string.Join(", ", result.Select(x => $"({x.StartDate}-{x.EndDate}, {x.Availability})"));

        Console.WriteLine(text);
    }
}
