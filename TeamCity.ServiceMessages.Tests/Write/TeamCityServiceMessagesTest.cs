

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
    using System;
    using System.Text;
    using NUnit.Framework;
    using ServiceMessages.Write.Special;

    [TestFixture]
    public class TeamCityServiceMessagesTest
    {
        private void DevNull(string _)
        {
        }

        [Test]
        public void DoNotAllowDoubleDispose()
        {
            var writer = new TeamCityServiceMessages().CreateWriter(DevNull);
            writer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => writer.Dispose());
        }

        [Test]
        public void DoNotAllowReuseWhileChildren()
        {
            var writer = new TeamCityServiceMessages().CreateWriter(DevNull);
            writer.OpenBlock("aaa");
            Assert.Throws<InvalidOperationException>(() => writer.OpenBlock("qqq"));
        }

        [Test]
        public void DoNotAllowReuseWhileChildren2()
        {
            var writer = new TeamCityServiceMessages().CreateWriter(DevNull);
            writer.OpenBlock("aaa");
            Assert.Throws<InvalidOperationException>(() => writer.OpenCompilationBlock("qqq"));
        }

        [Test]
        public void DoNotAllowReuseWhileChildren3()
        {
            var writer = new TeamCityServiceMessages().CreateWriter(DevNull);
            writer.OpenCompilationBlock("qqq");
            Assert.Throws<InvalidOperationException>(() => writer.OpenTestSuite("aaa"));
        }

        [Test]
        public void DoNotAllowReuseWhileChildren4()
        {
            var writer = new TeamCityServiceMessages().CreateWriter(DevNull);
            writer.OpenCompilationBlock("qqq");
            Assert.Throws<InvalidOperationException>(() => writer.OpenTest("aaa"));
        }

        [Test]
        public void TestDumpsServiceMessages()
        {
            var builder = new StringBuilder();
            using (var writer = new TeamCityServiceMessages().CreateWriter(x => builder.AppendLine(x)))
            {
                using (var block = writer.OpenBlock("Big log from TeamCity Service Messages"))
                {
                    using (var suite = block.OpenTestSuite("siote"))
                    {
                        using (var test = suite.OpenTest("test3"))
                        {
                            test.WriteIgnored();
                        }
                        using (var test = suite.OpenTest("test3"))
                        {
                            test.WriteIgnored();
                        }
                    }
                }
            }

            Console.Out.WriteLine("log: \r\n{0}", builder.ToString().Replace("##", "$$"));
        }


        [Test]
        public void TestDumpsServiceMessages2()
        {
            var builder = new StringBuilder();
            using (var writer = new TeamCityServiceMessages().CreateWriter(x => builder.AppendLine(x)))
            {
                using (var block = writer.OpenBlock("Prepare binaties"))
                {
                    using (var compile = block.OpenCompilationBlock("Kotlin"))
                    {
                        // Do some stuff
                    }
                }

                using (var block = writer.OpenBlock("Tests"))
                {
                    using (var suite = block.OpenTestSuite("siote"))
                    {
                        using (var test = suite.OpenTest("test3"))
                        {
                            test.WriteIgnored();
                        }
                        using (var test = suite.OpenTest("test3"))
                        {
                            test.WriteIgnored();
                        }
                    }
                }
            }
            Console.Out.WriteLine("log: \r\n{0}", builder.ToString().Replace("##", "$$"));
        }
    }
}