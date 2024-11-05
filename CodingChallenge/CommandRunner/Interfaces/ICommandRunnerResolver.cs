namespace CodingChallenge.App.CommandRunner.Interfaces;

public interface ICommandRunnerResolver
{
    Task Execute(ParsedCommand command);
}
