using CodingChallenge.App.CommandRunner;
using CodingChallenge.App.CommandRunner.Interfaces;
using CodingChallenge.App.CommandRunner.Runners;
using Command.Features.Search;
using Data.Extensions;
using Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace CodingChallenge.App;

public class Program
{
    /// <summary>
    /// Coding challenge. Application to manage hotel bookings.
    /// </summary>
    /// <param name="hotels">path to the json file containing hotels info</param>
    /// <param name="bookings">path to the json file containing bookings info</param>
    /// <returns></returns>
    public static async Task Main(string hotels, string bookings)
    {
        var provider = BuildServiceProvider(hotels, bookings);
        var commandParser = provider.GetRequiredService<ICommandParser>();
        var commandRunner = provider.GetRequiredService<ICommandRunnerResolver>();

        var result = Console.ReadLine();
        while (!string.IsNullOrEmpty(result))
        {
            try
            {
                var command = commandParser.Parse(result);
                await commandRunner.Execute(command);
            } catch (ValidationException e)
            {
                Console.WriteLine($"There was an error during command execution: {e.Message}");
            }
            
            result = Console.ReadLine();
        }

        Console.WriteLine("No command provided, finishing...");
    }

    private static IServiceProvider BuildServiceProvider(string hotels, string bookings)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddRepositories(hotels, bookings);
        serviceCollection.AddMediatR(c => c.RegisterServicesFromAssemblyContaining(typeof(SearchCommand)));

        serviceCollection.AddSingleton<ICommandRunner, SearchCommandRunner>();
        serviceCollection.AddSingleton<ICommandRunner, GetAvailabilityCommandRunner>();
        serviceCollection.AddSingleton<ICommandRunnerResolver, CommandRunnerResolver>();
        serviceCollection.AddSingleton<ICommandParser, CommandParser.CommandParser>();

        return serviceCollection.BuildServiceProvider();
    }
}