using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PackageSecurity.Test
{
    [TestClass]
    public class LoadFileTest
    {
        private string _file;
        private Vulnerabilities _vulners;

        [TestInitialize]
        public void Setup()
        {
            _file = new FileInfo(@"..\..\..\src\Resources\npmrepository.json").FullName;
            _vulners = Vulnerabilities.LoadFromFile(_file).Result;
        }

        [TestMethod]
        public void LoadVulnerabilities()
        {
            Assert.IsTrue(_vulners.List.Count() >= 107, "No vulnurabilies loaded");
        }

        [TestMethod]
        public void CheckPackageBelow()
        {
            Vulnerability handlebars = _vulners.CheckPackage("handlebars", "0.1.1");
            Assert.AreEqual(VulnerabilityLevel.Medium, handlebars.Severity);
        }

        [TestMethod]
        public void CheckPackageInfoLevel()
        {
            Vulnerability npm1 = _vulners.CheckPackage("npm", "3.0.0");
            Assert.AreEqual(VulnerabilityLevel.High, npm1.Severity);

            Vulnerability npm2 = _vulners.CheckPackage("npm", "3.8.2");
            Assert.AreEqual(VulnerabilityLevel.None, npm2.Severity);
        }

        [TestMethod]
        public void CheckPackageBelowAtOrAbove()
        {
            Vulnerability ember1 = _vulners.CheckPackage("ember", "1.3.1");
            Assert.AreEqual(VulnerabilityLevel.High, ember1.Severity);

            Vulnerability ember2 = _vulners.CheckPackage("ember", "1.3.0");
            Assert.AreEqual(VulnerabilityLevel.High, ember2.Severity);

            Vulnerability ember3 = _vulners.CheckPackage("ember", "1.4.0-beta.1");
            Assert.AreEqual(VulnerabilityLevel.Medium, ember3.Severity);
        }
    }
}
