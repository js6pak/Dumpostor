namespace Dumpostor.Dumpers
{
    public interface IMapDumper
    {
        string FileName { get; }

        string Dump(MapTypes mapType, ShipStatus shipStatus);
    }
}
