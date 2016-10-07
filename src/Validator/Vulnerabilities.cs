using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return await LoadFromFile("").ConfigureAwait(false);
        }

        public static async Task<Vulnerabilities> LoadFromFile(string fileName)
        {
            var list = new Dictionary<string, IEnumerable<Vulnerability>>();

            using (var reader = new StreamReader(fileName))
            {
                var json = await reader.ReadToEndAsync().ConfigureAwait(false);
                var obj = JObject.Parse(json);

                foreach (var child in obj.Children<JProperty>())
                {
                    var content = child.Children().First().Values().First().ToString();
                    var vulners = JsonConvert.DeserializeObject<List<Vulnerability>>(content);
                    list.Add(child.Name, vulners);
                }

                return new Vulnerabilities(list);
            }
        }

        public VulnerabilityLevel CheckPackage(string packageName, string version)
        {
            if (!List.ContainsKey(packageName))
                return VulnerabilityLevel.None;

            var vuls = List[packageName];
            var sv = SemanticVersion.Parse(version.Trim());

            foreach (var vulnerability in vuls)
            {
                var atOrAbove = SemanticVersion.Parse(vulnerability.AtOrAbove);
                var below = SemanticVersion.Parse(vulnerability.Below);

                if (!string.IsNullOrEmpty(atOrAbove.OriginalText) && !string.IsNullOrEmpty(below.OriginalText))
                {
                    if (atOrAbove.CompareTo(sv) <= 0 && below.CompareTo(sv) > 0)
                        return GetLevel(vulnerability);
                }

                else if (!string.IsNullOrEmpty(below.OriginalText))
                {
                    if (below.CompareTo(sv) > 0)
                        return GetLevel(vulnerability);
                }

                else if (!string.IsNullOrEmpty(atOrAbove.OriginalText))
                {
                    if (atOrAbove.CompareTo(sv) <= 0)
                        return GetLevel(vulnerability);
                }
            }

            return VulnerabilityLevel.None;
        }

        private static VulnerabilityLevel GetLevel(Vulnerability vulnerability)
        {
            if (vulnerability.Severity == VulnerabilityLevel.None)
                return VulnerabilityLevel.Info;

                return vulnerability.Severity;
        }
    }
}
