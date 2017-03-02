namespace Simple
{
    using System;
    using JetBrains.TeamCity.ServiceMessages.Write.Special;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var writer = new TeamCityServiceMessages().CreateWriter())
            using (var block = writer.OpenBlock("Tests"))
            using (var testClass = block.OpenTestSuite("TestClass"))
            {
                using (var test = testClass.OpenTest("Test1"))
                {
                    test.WriteStdOutput("Some output");
                    test.WriteDuration(TimeSpan.FromSeconds(1));
                }

                using (var test = testClass.OpenTest("Test2"))
                {
                    test.WriteIgnored();
                }
            }
        }
    }
}
