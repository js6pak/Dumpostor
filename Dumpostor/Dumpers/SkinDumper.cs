using System.Linq;
using System.Text.Json;

namespace Dumpostor.Dumpers
{
    public class SkinDumper : IDumper
    {
        public string FileName => "SkinType.json";

        public string Dump()
        {
            return JsonSerializer.Serialize(
                HatManager.Instance.AllSkins.ToArray()
                    .ToDictionary(k => k.name.Replace("rhm", "RightHandMan").Capitalize(), v => HatManager.Instance.GetIdFromSkin(v))
            );
        }
    }
}
