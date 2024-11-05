using Data.Config;
using Data.Interfaces;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Data.UnitTests")]
namespace Data.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void AddRepositories(this IServiceCollection services, string hotelLocation, string bookingLocation)
    {
        services.Configure<RepoConfiguration>(x =>
        {
            x.BookingFile = bookingLocation;
            x.HotelFile = hotelLocation;
        });

        services.AddTransient<IBookingRepository, BookingRepository>();
        services.AddTransient<IHotelRepository, HotelRepository>();
    }
}