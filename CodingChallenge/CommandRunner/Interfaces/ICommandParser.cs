namespace CodingChallenge.App.CommandRunner.Interfaces;

public interface ICommandParser
{
    ParsedCommand Parse(string command);
}
