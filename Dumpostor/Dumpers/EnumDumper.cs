using System;
using System.Linq;
using System.Text.Json;

namespace Dumpostor.Dumpers
{
    public class EnumDumper<T> : IDumper where T : Enum
    {
        public string FileName => typeof(T).Name + ".json";

        public string Dump()
        {
            return JsonSerializer.Serialize(
                Extensions.GetValues<T>()
                    .ToDictionary(k => Enum.GetName(typeof(T), k), v => v)
            );
        }
    }
}
