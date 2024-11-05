namespace Data.Dao;

internal class RoomTypeDao
{
    public string Code { get; set; }
    public string Description { get; set; }
    public IEnumerable<string> Amenities { get; set; }
    public IEnumerable<string> Features { get; set; }
}
