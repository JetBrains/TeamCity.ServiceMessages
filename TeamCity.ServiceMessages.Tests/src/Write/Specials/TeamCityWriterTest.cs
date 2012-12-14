using System.Collections.Generic;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
  [TestFixture]
  public class TeamCityWriterTest : TeamCityWriterBaseTest<ITeamCityWriter>
  {
    private bool myIsDisposed;

    [Test]
    public void TestCustomServiceMessage_Simple()
    {
      DoTest(x=> x.WriteRawMessage(new SimpleServiceMessage()), "##teamcity[]");
    }

    [Test]
    public void TestCustomServiceMessage_Complex()
    {
      DoTest(x=> x.WriteRawMessage(new ComplexServiceMessage()), "##teamcity[]");
    }

    [Test]
    public void TestFlows_TwoFlow()
    {
      DoTest(x =>
               {
                 using(x)
                 {
                   var flow1 = x.OpenFlow("f1");
                   var flow2 = x.OpenFlow("f2");

                   flow1.WriteMessage("flow1 message");
                   flow2.WriteMessage("flow2 message");

                   flow1.Dispose();
                   flow2.Dispose();
                 }
               },
               "##teamcity[]" 
               );      
    }

    
    [Test]
    public void TestFlows_TwoBlocks()
    {
      DoTest(x =>
               {
                 using(x)
                 {
                   var flow1 = x.OpenFlow("f1");
                   var flow2 = x.OpenFlow("f2");

                   var block1 = flow1.OpenBlock("b1");
                   var block2 = flow2.OpenBlock("b2");
                   var blockR = x.OpenBlock("root");


                   x.WriteMessage("root");
                   block1.WriteMessage("flow1 message");
                   block2.WriteMessage("flow2 message");

                   block1.Dispose();
                   block2.Dispose();
                 }
               },
               "##teamcity[]" 
               );      
    }


    private class ComplexServiceMessage : IServiceMessage
    {
      public string Name
      {
        get { return "ThisIsTheName"; }
      }

      public string DefaultValue
      {
        get { return null; }
      }

      public IEnumerable<string> Keys
      {
        get { return new[] {"a", "b", "c"}; }
      }

      public string GetValue(string key)
      {
        return key;
      }
    }

    private class SimpleServiceMessage : IServiceMessage
    {
      public string Name
      {
        get { return "ThisIsTheSimple"; }
      }

      public string DefaultValue
      {
        get { return "Default"; }
      }

      public IEnumerable<string> Keys
      {
        get { return new string[0]; }
      }

      public string GetValue(string key)
      {
        return key;
      }
    }



    public override void SetUp()
    {
      base.SetUp();
      myIsDisposed = false;
    }

    protected override ITeamCityWriter Create(IServiceMessageProcessor proc)
    {
      return new TeamCityWriterImpl(proc, new DisposableDelegate(() => { myIsDisposed = true; }));
    }
  }
}
