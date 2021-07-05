using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Dumpostor.Dumpers
{
    public class HatDumper : IDumper
    {
        public string FileName => "HatType.json";

        private string FilterHatName(string name)
        {
            var index = name.LastIndexOf("_", StringComparison.Ordinal);

            if (index != -1)
            {
                name = name[(index + 1)..];
            }

            return name.Replace(" ", "").Replace("\u0027", "").Capitalize();
        }

        public string Dump()
        {
            var dictionary = new Dictionary<string, uint>();

            foreach (var hat in HatManager.Instance.AllHats)
            {
                var id = HatManager.Instance.GetIdFromHat(hat);

                if (id == 43)
                {
                    dictionary.Add("BlackFedora", id);
                    continue;
                }

                if (id == 56)
                {
                    dictionary.Add("GreenFedora", id);
                    continue;
                }

                if (id == 93)
                {
                    dictionary.Add("SnowCrewmate", id);
                    continue;
                }

                if (!dictionary.TryAdd(FilterHatName(hat.name.Replace("rhm", "RightHandMan")), id))
                {
                    dictionary.Add(FilterHatName(hat.MainImage.name)[..^"0001".Length], id);
                }
            }

            return JsonSerializer.Serialize(dictionary);
        }
    }
}
