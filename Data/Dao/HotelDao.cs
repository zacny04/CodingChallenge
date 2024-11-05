namespace Data.Dao;

internal class HotelDao
{
    public string Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<RoomTypeDao> RoomTypes { get; set; }
    public IEnumerable<RoomDao> Rooms { get; set; }
}
