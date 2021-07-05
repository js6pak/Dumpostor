using System.Linq;
using System.Text.Json;

namespace Dumpostor.Dumpers
{
    public class PetDumper : IDumper
    {
        public string FileName => "PetType.json";

        public string Dump()
        {
            return JsonSerializer.Serialize(
                HatManager.Instance.AllPets.ToArray()
                    .ToDictionary(k => k.name.Capitalize(), v => HatManager.Instance.GetIdFromPet(v))
            );
        }
    }
}
