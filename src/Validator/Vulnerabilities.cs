using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PackageSecurity
{
    public class Vulnerabilities
    {
        private Vulnerabilities(Dictionary<string, IEnumerable<Vulnerability>> list)
        {
            List = list;
        }

        public Dictionary<string, IEnumerable<Vulnerability>> List { get; }

        public static async Task<Vulnerabilities> LoadFromExtensionPath()
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            string dir = Path.GetDirectoryName(assembly);
            string file = Path.Combine(dir, "Resources", "npmrepository.json");

            return await LoadFromFile(file).ConfigureAwait(false);
        }

        public static async Task<Vulnerabilities> LoadFromFile(string fileName)
        {
            var list = new Dictionary<string, IEnumerable<Vulnerability>>();

            using (var reader = new StreamReader(fileName))
            {
                string json = await reader.ReadToEndAsync().ConfigureAwait(false);
                var obj = JObject.Parse(json);

                foreach (JProperty child in obj.Children<JProperty>())
                {
                    string content = child.Children().First().Values().First().ToString();
                    List<Vulnerability> vulners = JsonConvert.DeserializeObject<List<Vulnerability>>(content);
                    list.Add(child.Name, vulners);
                }

                return new Vulnerabilities(list);
            }
        }

        public Vulnerability CheckPackage(string packageName, string version)
        {
            if (!List.ContainsKey(packageName))
                return Vulnerability.Empty;

            IEnumerable<Vulnerability> vuls = List[packageName];
            var sv = SemanticVersion.Parse(version.Trim());

            foreach (Vulnerability vulnerability in vuls)
            {
                var atOrAbove = SemanticVersion.Parse(vulnerability.AtOrAbove);
                var below = SemanticVersion.Parse(vulnerability.Below);

                if (!string.IsNullOrEmpty(atOrAbove.OriginalText) && !string.IsNullOrEmpty(below.OriginalText))
                {
                    if (atOrAbove.CompareTo(sv) <= 0 && below.CompareTo(sv) > 0)
                        return AdjustSeverity(vulnerability);
                }

                else if (!string.IsNullOrEmpty(below.OriginalText))
                {
                    if (below.CompareTo(sv) > 0)
                        return AdjustSeverity(vulnerability);
                }

                else if (!string.IsNullOrEmpty(atOrAbove.OriginalText))
                {
                    if (atOrAbove.CompareTo(sv) <= 0)
                        return AdjustSeverity(vulnerability);
                }
            }

            return Vulnerability.Empty;
        }

        private static Vulnerability AdjustSeverity(Vulnerability vulnerability)
        {
            if (vulnerability.Severity == VulnerabilityLevel.None)
                vulnerability.Severity = VulnerabilityLevel.Info;

                return vulnerability;
        }
    }
}
