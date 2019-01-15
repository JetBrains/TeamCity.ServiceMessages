namespace Simple
{
    using System;
    using System.Globalization;
    using System.IO;
    using JetBrains.TeamCity.ServiceMessages.Write.Special;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Creating the root writer
            using (var writer = new TeamCityServiceMessages().CreateWriter(Console.WriteLine))
            // Creating the build log block "Tests"
            using (var block = writer.OpenBlock("Tests"))
            // Creating the test suite "Tests"
            using (var testClass = block.OpenTestSuite("TestClass"))
            {
                // Creating the successful test
                using (var test = testClass.OpenTest("Successful"))
                {
                    test.WriteStdOutput("Some output");
                    test.WriteDuration(TimeSpan.FromSeconds(1));
                }

                // Creating the ignored test
                using (var test = testClass.OpenTest("Ignored"))
                {
                    test.WriteIgnored();
                }

                // Creating the failed test
                using (var test = testClass.OpenTest("Failed"))
                {
                    test.WriteFailed("Some message", "Details");
                }

                // Attaching an image to test
                using (var test = testClass.OpenTest("Image"))
                {
                    writer.PublishArtifact(Path.GetFullPath("TeamCity.png") + " => TestData");
                    test.WriteImage("TestData/TeamCity.png", "Team City Logo");
                }

                // Attaching a value to test
                using (var test = testClass.OpenTest("Value"))
                {
                    test.WriteValue(1234.56.ToString(CultureInfo.InvariantCulture), "Some Value");
                }

                // Attaching a file to test
                using (var test = testClass.OpenTest("File"))
                {
                    writer.PublishArtifact(Path.GetFullPath("TeamCity.png") + " => TestData");
                    test.WriteFile("TestData/TeamCity.png", "Team City Logo file");
                }

                // Attaching a link to test
                using (var test = testClass.OpenTest("Link"))
                {
                    test.WriteLink("https://www.jetbrains.com/", "JetBrains");
                }
            }
        }
    }
}
