namespace Domain.Models;

public class Booking
{
    public string HotelId { get; set; }
    public DateOnly Arrival { get; set; }
    public DateOnly Departure { get; set; }
    public string RoomType { get; set; }
    public string RoomRate { get; set; }
}
