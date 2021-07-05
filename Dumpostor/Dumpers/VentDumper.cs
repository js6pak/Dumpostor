using System.Linq;
using System.Text.Json;
using UnityEngine;

namespace Dumpostor.Dumpers
{
    public class VentDumper : IMapDumper
    {
        public string FileName => "vents.json";

        public string Dump(MapTypes mapType, ShipStatus shipStatus)
        {
            return JsonSerializer.Serialize(
                shipStatus.AllVents.ToDictionary(k => k.Id, vent => new VentInfo(vent.Id, vent.transform.position, vent.Left?.Id, vent.Center?.Id, vent.Right?.Id)),
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters =
                    {
                        new Vector2Converter()
                    }
                });
        }
    }

    public class VentInfo
    {
        public VentInfo(int id, Vector2 position, int? left, int? center, int? right)
        {
            Id = id;
            Position = position;
            Left = left;
            Center = center;
            Right = right;
        }

        public int Id { get; }

        public Vector2 Position { get; }

        public int? Left { get; }

        public int? Center { get; }

        public int? Right { get; }
    }
}
