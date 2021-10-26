using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Dumpostor.Dumpers
{
    public class TranslationsDumper : IDumper
    {
        public string FileName => "translations.json";

        public string Dump()
        {
            Dictionary<string, Dictionary<string, string>>
                output = new Dictionary<string, Dictionary<string, string>>();

            foreach (TranslatedImageSet translationSet in TranslationController.Instance.Languages)
            {
                Dictionary<string, string> translations = new Dictionary<string, string>();
                string[] rows = translationSet.Data.text.Split("\r\n");

                for (int i = 0; i < rows.Length; i++)
                {
                    if (i == 0) // skip header row
                        continue;

                    string row = rows[i];
                    string[] columns = row.Split("\t");

                    if (!translations.ContainsKey(columns[0]))
                    {
                        translations.Add(columns[0], columns[1]);
                    }
                }

                if (!output.ContainsKey(translationSet.Name))
                {
                    output.Add(translationSet.Name, translations);
                }
            }
            
            return JsonSerializer.Serialize(output);
        }
    }
}
