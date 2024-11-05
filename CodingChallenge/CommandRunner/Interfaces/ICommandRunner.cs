namespace CodingChallenge.App.CommandRunner.Interfaces;

public interface ICommandRunner
{
    string CommandName { get; }
    Task Run(ParsedCommand command);
}
