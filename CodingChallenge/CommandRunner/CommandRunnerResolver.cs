using CodingChallenge.App.CommandRunner.Interfaces;
using Domain.Exceptions;

namespace CodingChallenge.App.CommandRunner;

public class CommandRunnerResolver : ICommandRunnerResolver
{
    private readonly IEnumerable<ICommandRunner> _commandRunners;
    public CommandRunnerResolver(IEnumerable<ICommandRunner> commandRunners)
    {
        ArgumentNullException.ThrowIfNull(commandRunners);

        _commandRunners = commandRunners;
    }

    public async Task Execute(ParsedCommand command)
    {
        var runner = _commandRunners.FirstOrDefault(x => x.CommandName == command.CommandName);
        if (runner == null)
        {
            throw new ValidationException($"Command not supported: {command.CommandName}");
        }

        await runner.Run(command);
    }
}
