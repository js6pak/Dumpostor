namespace Dumpostor.Dumpers
{
    public interface IDumper
    {
        string FileName { get; }

        string Dump();
    }
}
