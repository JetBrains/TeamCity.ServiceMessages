using JetBrains.TeamCity.ServiceMessages.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Writer
{
  public class TeamCityTestsWriter : BaseDisposableWriter, ITeamCityTestsWriter
  {
    [CanBeNull]
    private readonly string mySuiteName;

    public TeamCityTestsWriter(BaseWriter target, string suiteName = null) : base(target)
    {
      mySuiteName = suiteName;
      if (mySuiteName != null)
        PostMessage(new SimpleServiceMessage("testSuiteStarted") {{"name", mySuiteName}});
    }

    protected override void DisposeImpl()
    {
      if (mySuiteName != null)
        PostMessage(new SimpleServiceMessage("testSuiteFinished") { { "name", mySuiteName } });

    }

    public ITeamCityTestsWriter OpenTestSuite(string suiteName)
    {
      return new TeamCityTestsWriter(this, suiteName);
    }

    public ITeamCityTestWriter OpenTest(string testName)
    {
      return new TeamCityTestWriter(this, testName);
    }
  }
}