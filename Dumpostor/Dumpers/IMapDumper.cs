namespace Dumpostor.Dumpers
{
    // TODO make this totally independent from IDumper?
    public interface IMapDumper : IDumper
    {
        string IDumper.Dump()
        {
            return null;
        }

        string Dump(MapTypes mapType, ShipStatus shipStatus);
    }
}
