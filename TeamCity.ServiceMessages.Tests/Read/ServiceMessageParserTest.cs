


namespace JetBrains.TeamCity.ServiceMessages.Tests.Read
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using ServiceMessages.Read;

    [TestFixture]
    public class ServiceMessageParserTest
    {
        [Test]
        public void BrokenStream_0()
        {
            var sb = new StringBuilder();
            for (int c = char.MinValue; c < char.MaxValue; sb.Append((char) c++)) ;
            for (int c = char.MinValue; c < char.MaxValue; sb.Append((char) c++)) ;

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_1()
        {
            var sb = new StringBuilder();
            sb.Append("##te");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_2()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_3()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[]");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_4()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[ ]");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_5()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[aa ]");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_6()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[aa '");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_7()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[aa ");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_8()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[aa");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_9()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[aa Z");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_A()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[aa Z =");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_B()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[aa Z = '");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_C()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[aa Z = 'fddd");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_Combinatorics()
        {
            var result = new List<int>();
            const string text =
                "##teamcity[name 'a'] ##teamcity[name 'a'] ##teamcity[name 'a']  ##teamcity[name 'a'] ##teamcity[name a  =  'a' b='qqq|n'  ]\t##teamcity[name a='z']\r\n##teamcity[name a='z']\r##teamcity[name a='z']##teamcity[package Id='CommonServiceLocator' Version='1.0' Authors='Microsoft' Description='The Common Service Locator library contains a shared interface for service location which application and framework developers can reference. The library provides an abstraction over IoC containers and service locators. Using the library allows an application to indirectly access the capabilities without relying on hard references. The hope is that using this library, third-party applications and frameworks can begin to leverage IoC/Service Location without tying themselves down to a specific implementation.' IsLatestVersion='true' LastUpdated='2011-10-21T16:34:09Z' LicenseUrl='http://commonservicelocator.codeplex.com/license' PackageHash='RJjv0yxm+Fk/ak/CVMTGr0ng7g/nudkVYos4eQrIDpth3BdE1j7J2ddRm8FXtOoIZbgDqTU6hKq5zoackwL3HQ==' PackageHashAlgorithm='SHA512' PackageSize='37216' ProjectUrl='http://commonservicelocator.codeplex.com/' RequireLicenseAcceptance='false' TeamCityBuildId='42' TeamCityDownloadUrl='/repository/download/bt/42:id/null']";
            for (var i = 0; i < text.Length; i++)
            for (var j = i; j < text.Length; j++)
            {
                var input = text.Substring(i, text.Length - j);
                var count = new ServiceMessageParser().ParseServiceMessages(input).ToArray().Length;
                result.Add(count);
            }
            Assert.IsTrue(result.Contains(9));
        }

        [Test]
        public void BrokenStream_D()
        {
            var sb = new StringBuilder();
            sb.Append("##teamcity[aa Z = 'fddd '   ");

            Assert.IsFalse(new ServiceMessageParser().ParseServiceMessages(new StringWrapper(sb.ToString())).ToArray().Any());
        }

        [Test]
        public void BrokenStream_E()
        {
            var sr = new StringWrapper("##teamcity[name 'a'] ");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(1, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(0, msg.Keys.Count());
            Assert.AreEqual("a", msg.DefaultValue);
        }

        [Test]
        public void BrokenStream_F()
        {
            var sr = new StringWrapper("##teamcity[name 'a']\r\n");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(1, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(0, msg.Keys.Count());
            Assert.AreEqual("a", msg.DefaultValue);
        }

        [Test]
        public void BrokenStream_H()
        {
            const string text = @"                 ##teamcity[package Id='CommonServiceLocator' Version='1.0' Authors='Microsoft' Description='The Common Service Locator library contains a shared interface for service location which application and framework developers can reference. The library provides an abstraction over IoC containers and service locators. Using the library allows an application to indirectly access the capabilities without relying on hard references. The hope is that using this library, third-party applications and frameworks can begin to leverage IoC/Service Location without tying themselves down to a specific implementation.' IsLatestVersion='true' LastUpdated='2011-10-21T16:34:09Z' LicenseUrl='http://commonservicelocator.codeplex.com/license' PackageHash='RJjv0yxm+Fk/ak/CVMTGr0ng7g/nudkVYos4eQrIDpth3BdE1j7J2ddRm8FXtOoIZbgDqTU6hKq5zoackwL3HQ==' PackageHashAlgorithm='SHA512' PackageSize='37216' ProjectUrl='http://commonservicelocator.codeplex.com/' RequireLicenseAcceptance='false' TeamCityBuildId='42' TeamCityDownloadUrl='/repository/download/bt/42:id/null']                  ";
            var bytes = Encoding.UTF8.GetBytes(text.ToCharArray());
            var stream = new MemoryStream(bytes);
            var rdr = new StreamReader(stream, Encoding.UTF8);
            var result = new ServiceMessageParser().ParseServiceMessages(rdr).ToArray();
            Assert.IsTrue(result.Length == 1);
        }

        [Test]
        public void BrokenStream_I()
        {
            var text =
                @"##teamcity[package Id='CommonServiceLocator' Version='1.0' Authors='Microsoft' Description='The Common Service Locator library contains a shared interface for service location which application and framework developers can reference. The library provides an abstraction over IoC containers and service locators. Using the library allows an application to indirectly access the capabilities without relying on hard references. The hope is that using this library, third-party applications and frameworks can begin to leverage IoC/Service Location without tying themselves down to a specific implementation.' IsLatestVersion='true' LastUpdated='2011-10-21T16:34:09Z' LicenseUrl='http://commonservicelocator.codeplex.com/license' PackageHash='RJjv0yxm+Fk/ak/CVMTGr0ng7g/nudkVYos4eQrIDpth3BdE1j7J2ddRm8FXtOoIZbgDqTU6hKq5zoackwL3HQ==' PackageHashAlgorithm='SHA512' PackageSize='37216' ProjectUrl='http://commonservicelocator.codeplex.com/' RequireLicenseAcceptance='false' TeamCityBuildId='42' TeamCityDownloadUrl='/repository/download/bt/42:id/null']
        ;";
            new ServiceMessageParser().ParseServiceMessages(new StringWrapper(text)).Where(x => true).OrderBy(x => x.Name).ToArray();
            new ServiceMessageParser().ParseServiceMessages(new StringWrapper(text.Trim())).Where(x => true).OrderBy(x => x.Name).ToArray();
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(text.Trim().ToCharArray()));
            new ServiceMessageParser().ParseServiceMessages(new StreamReader(memoryStream)).Where(x => true).OrderBy(x => x.Name).ToArray();
        }

        [Test]
        public void BrokenStream_J()
        {
            const string text = @"                 ##teamcity[package Id='CommonServiceLocator' Version='1.0' Authors='Microsoft' Description='The Common Service Locator library contains a shared interface for service location which application and framework developers can reference. The library provides an abstraction over IoC containers and service locators. Using the library allows an application to indirectly access the capabilities without relying on hard references. The hope is that using this library, third-party applications and frameworks can begin to leverage IoC/Service Location without tying themselves down to a specific implementation.' IsLatestVersion='true' LastUpdated='2011-10-21T16:34:09Z' LicenseUrl='http://commonservicelocator.codeplex.com/license' PackageHash='RJjv0yxm+Fk/ak/CVMTGr0ng7g/nudkVYos4eQrIDpth3BdE1j7J2ddRm8FXtOoIZbgDqTU6hKq5zoackwL3HQ==' PackageHashAlgorithm='SHA512' PackageSize='37216' ProjectUrl='http://commonservicelocator.codeplex.com/' RequireLicenseAcceptance='false' TeamCityBuildId='42' TeamCityDownloadUrl='/repository/download/bt/42:id/null']                  ";

            var path = Path.GetTempFileName();
            try
            {
                File.WriteAllText(path, text);

                using (var stream = File.OpenRead(path))
                {
                    var rdr = new StreamReader(stream, Encoding.UTF8);
                    var result = new ServiceMessageParser().ParseServiceMessages(rdr).ToArray();
                    Assert.IsTrue(result.Length == 1);
                }
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        public void ShouldParseService_complexMessage_escaping()
        {
            var sr = new StringWrapper("##teamcity[name    a='1\"|'|n|r|x|l|p|||[|]'     b='2\"|'|n|r|x|l|p|||[|]'   ]");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(1, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(2, msg.Keys.Count());
            Assert.AreEqual(new HashSet<string> {"a", "b"}, new HashSet<string>(msg.Keys));
            Assert.AreEqual(null, msg.DefaultValue);
            Assert.AreEqual("1\"'\n\r\u0085\u2028\u2029|[]", msg.GetValue("a"));
            Assert.AreEqual("2\"'\n\r\u0085\u2028\u2029|[]", msg.GetValue("b"));
        }

        [Test]
        public void ShouldParseService_complexMessage_multi()
        {
            var sr = new StringWrapper("  ##teamcity[name a='z']##teamcity[name a='z']##teamcity[name a='z']  ");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(3, result.Length);

            foreach (var msg in result)
            {
                Assert.AreEqual("name", msg.Name);
                Assert.AreEqual(1, msg.Keys.Count());
                Assert.AreEqual(new HashSet<string> {"a"}, new HashSet<string>(msg.Keys));
                Assert.AreEqual(null, msg.DefaultValue);
                Assert.AreEqual("z", msg.GetValue("a"));
            }
        }

        [Test]
        public void ShouldParseService_complexMessage_multi2()
        {
            var sr = new StringWrapper("  ##teamcity[name a='z']\r  ##teamcity[name a='z']\n##teamcity[name a='z']  ");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(3, result.Length);

            foreach (var msg in result)
            {
                Assert.AreEqual("name", msg.Name);
                Assert.AreEqual(1, msg.Keys.Count());
                Assert.AreEqual(new HashSet<string> {"a"}, new HashSet<string>(msg.Keys));
                Assert.AreEqual(null, msg.DefaultValue);
                Assert.AreEqual("z", msg.GetValue("a"));
            }
        }

        [Test]
        public void ShouldParseService_complexMessage0()
        {
            var sr = new StringWrapper("##teamcity[name a='a' b='z']");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(1, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(2, msg.Keys.Count());
            Assert.AreEqual(new HashSet<string> {"a", "b"}, new HashSet<string>(msg.Keys));
            Assert.AreEqual(null, msg.DefaultValue);
            Assert.AreEqual("a", msg.GetValue("a"));
            Assert.AreEqual("z", msg.GetValue("b"));
        }

        [Test]
        public void ShouldParseService_complexMessage1()
        {
            var sr = new StringWrapper("##teamcity[name a='a']");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(1, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(1, msg.Keys.Count());
            Assert.AreEqual(new HashSet<string> {"a"}, new HashSet<string>(msg.Keys));
            Assert.AreEqual(null, msg.DefaultValue);
            Assert.AreEqual("a", msg.GetValue("a"));
        }


        [Test]
        public void ShouldParseService_complexMessage2()
        {
            var sr = new StringWrapper("##teamcity[name    a='a'     b='z'   ]");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(1, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(2, msg.Keys.Count());
            Assert.AreEqual(new HashSet<string> {"a", "b"}, new HashSet<string>(msg.Keys));
            Assert.AreEqual(null, msg.DefaultValue);
            Assert.AreEqual("a", msg.GetValue("a"));
            Assert.AreEqual("z", msg.GetValue("b"));
        }

        [Test]
        public void ShouldParseService_complexMessage3()
        {
            var sr = new StringWrapper("  ##teamcity[name a  =  'a'   ]  ");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(1, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(1, msg.Keys.Count());
            Assert.AreEqual(new HashSet<string> {"a"}, new HashSet<string>(msg.Keys));
            Assert.AreEqual(null, msg.DefaultValue);
            Assert.AreEqual("a", msg.GetValue("a"));
        }

        [Test]
        public void ShouldParseService_multi()
        {
            var sr = new StringWrapper("  ##teamcity[name a='z']\r##teamcity[zzz 'z']\n##teamcity[name a='z']  ");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(3, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(1, msg.Keys.Count());
            Assert.AreEqual(new HashSet<string> {"a"}, new HashSet<string>(msg.Keys));
            Assert.AreEqual(null, msg.DefaultValue);
            Assert.AreEqual("z", msg.GetValue("a"));

            msg = result[1];
            Assert.AreEqual("zzz", msg.Name);
            Assert.AreEqual(0, msg.Keys.Count());
            Assert.AreEqual("z", msg.DefaultValue);

            msg = result[2];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(1, msg.Keys.Count());
            Assert.AreEqual(new HashSet<string> {"a"}, new HashSet<string>(msg.Keys));
            Assert.AreEqual(null, msg.DefaultValue);
            Assert.AreEqual("z", msg.GetValue("a"));
        }


        [Test]
        public void ShouldParseService_simpleMessage()
        {
            var sr = new StringWrapper("##teamcity[name 'a']");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(1, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(0, msg.Keys.Count());
            Assert.AreEqual("a", msg.DefaultValue);
        }

        [Test]
        public void ShouldParseService_simpleMessage_decode()
        {
            var sr = new StringWrapper("##teamcity[name '\"|'|n|r|x|l|p|||[|]']");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(1, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(0, msg.Keys.Count());
            Assert.AreEqual("\"'\n\r\u0085\u2028\u2029|[]", msg.DefaultValue);
        }

        [Test]
        public void ShouldParseService_simpleMessages_multi()
        {
            var sr = new StringWrapper("##teamcity[name 'a'] ##teamcity[name 'a'] ##teamcity[name 'a']  ##teamcity[name 'a']");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(4, result.Length);

            foreach (var msg in result)
            {
                Assert.AreEqual("name", msg.Name);
                Assert.AreEqual(0, msg.Keys.Count());
                Assert.AreEqual("a", msg.DefaultValue);
            }
        }

        [Test]
        public void ShouldParseService_simpleMessages_multi2()
        {
            var sr = new StringWrapper("##teamcity[name 'a']\r ##teamcity[name 'a']\n ##teamcity[name 'a'] \r\n ##teamcity[name 'a']");
            var result = new ServiceMessageParser().ParseServiceMessages(sr).ToArray();
            Assert.AreEqual(4, result.Length);

            foreach (var msg in result)
            {
                Assert.AreEqual("name", msg.Name);
                Assert.AreEqual(0, msg.Keys.Count());
                Assert.AreEqual("a", msg.DefaultValue);
            }
        }

        [Test]
        public void ShouldParseString()
        {
            var result = new ServiceMessageParser().ParseServiceMessages("##teamcity[name    a='1\"|'|n|r|x|l|p|||[|]'     b='2\"|'|n|r|x|l|p|||[|]'   ]").ToArray();
            Assert.AreEqual(1, result.Length);

            var msg = result[0];
            Assert.AreEqual("name", msg.Name);
            Assert.AreEqual(2, msg.Keys.Count());
            Assert.AreEqual(new HashSet<string> {"a", "b"}, new HashSet<string>(msg.Keys));
            Assert.AreEqual(null, msg.DefaultValue);
            Assert.AreEqual("1\"'\n\r\u0085\u2028\u2029|[]", msg.GetValue("a"));
            Assert.AreEqual("2\"'\n\r\u0085\u2028\u2029|[]", msg.GetValue("b"));
        }
    }
}