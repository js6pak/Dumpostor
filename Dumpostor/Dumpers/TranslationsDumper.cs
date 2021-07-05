using System.Linq;

namespace Dumpostor.Dumpers
{
    public class TranslationsDumper : IDumper
    {
        public string FileName => "translations.txt";

        public string Dump()
        {
            return TranslationController.Instance.Languages.Single(x => x.Name == "English").Data.text;
        }
    }
}
