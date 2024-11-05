using CodingChallenge.App.CommandRunner;
using CodingChallenge.App.CommandRunner.Interfaces;
using Domain.Exceptions;

namespace CodingChallenge.App.CommandParser;


public class CommandParser : ICommandParser
{
    public ParsedCommand Parse(string command)
    {
        var argumentPosition = command.IndexOf('(');
        var argumentEndPosition = command.IndexOf(')');

        if (argumentPosition < 1 || argumentEndPosition <= argumentPosition)
        {
            throw new ValidationException("Command does not contain valid parameters");
        }

        var commandName = command[..argumentPosition];
        var parameters = command.Substring(argumentPosition + 1, argumentEndPosition - argumentPosition - 1).Split(',').Select(x => x.Trim()).ToArray();

        return new()
        {
            CommandName = commandName,
            Parameters = parameters,
        };
    }
}
