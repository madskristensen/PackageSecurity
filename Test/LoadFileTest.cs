﻿using System;
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
        private Vulnerabilities vulners;

        [TestInitialize]
        public void Setup()
        {
            _file = new FileInfo(@"..\..\..\src\Resources\npmrepository.json").FullName;
            vulners = Vulnerabilities.LoadFromFile(_file).Result;
        }

        [TestMethod]
        public void LoadVulnerabilities()
        {
            Assert.IsTrue(vulners.List.Count() >= 107, "No vulnurabilies loaded");
        }

        [TestMethod]
        public void CheckPackageBelow()
        {
            var handlebars = vulners.CheckPackage("handlebars", "0.1.1");
            Assert.AreEqual(VulnerabilityLevel.High, handlebars.Severity);
        }

        [TestMethod]
        public void CheckPackageInfoLevel()
        {
            var npm1 = vulners.CheckPackage("npm", "3.0.0");
            Assert.AreEqual(VulnerabilityLevel.Info, npm1.Severity);

            var npm2 = vulners.CheckPackage("npm", "3.8.2");
            Assert.AreEqual(VulnerabilityLevel.Info, npm2.Severity);
        }

        [TestMethod]
        public void CheckPackageBelowAtOrAbove()
        {
            var ember1 = vulners.CheckPackage("ember", "1.3.1");
            Assert.AreEqual(VulnerabilityLevel.High, ember1.Severity);

            var ember2 = vulners.CheckPackage("ember", "1.3.0");
            Assert.AreEqual(VulnerabilityLevel.High, ember2.Severity);

            var ember3 = vulners.CheckPackage("ember", "1.4.0-beta.1");
            Assert.AreEqual(VulnerabilityLevel.Medium, ember3.Severity);
        }
    }
}
