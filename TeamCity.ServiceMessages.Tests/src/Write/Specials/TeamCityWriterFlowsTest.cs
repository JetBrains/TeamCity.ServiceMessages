using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
  [TestFixture]
  public class TeamCityWriterFlowsTest : TeamCityWriterBaseTest
  {

    [Test]
    public void TestFlows_OpenDispose()
    {
      DoTestWithoutAsseert(flowB =>
               {
                 using (flowB)
                 {
                   for (int i = 0; i < 100; i++)
                   {
                     using (flowB.OpenFlow())
                     {
                       //NOP
                     }
                   }
                 }
               });
    }

    [Test]
    public void TestFlows_OpenDispose_inverse_order()
    {
      DoTestWithoutAsseert(flowB =>
               {
                 using (flowB)
                 {
                   var list = new List<IDisposable>();
                   for (int i = 0; i < 100; i++)
                   {
                     list.Add(flowB.OpenFlow());
                   }
                   list.Reverse();
                   foreach (var d in list)
                   {
                     d.Dispose();
                   }
                 }
               });
    }


    [Test]
    public void TestFlows_OneFlow()
    {
      DoTest(flowB =>
               {
                 using(flowB)
                 {
                   using (var flow1 = flowB.OpenFlow())
                   {
                     flow1.WriteMessage("flow1 message");
                     flowB.WriteMessage("flowB message");
                     flow1.WriteMessage("flow1 message");
                   }
                 }
               },
               "##teamcity[flowStarted parent='1' flowId='2']",
               "##teamcity[message text='flow1 message' status='NORMAL' flowId='2']",
               "##teamcity[message text='flowB message' status='NORMAL' flowId='1']",
               "##teamcity[message text='flow1 message' status='NORMAL' flowId='2']",
               "##teamcity[flowFinished flowId='2']"
               );      
    }

    [Test]
    public void TestFlows_TwoFlow()
    {
      DoTest(flowB =>
               {
                 using(flowB)
                 {
                   var flow1 = flowB.OpenFlow();
                   var flow2 = flowB.OpenFlow();

                   flow1.WriteMessage("flow1 message");
                   flowB.WriteMessage("flowB message");
                   flow2.WriteMessage("flow2 message");
                   
                   flow2.Dispose();
                   flow1.Dispose();
                 }
               },
               "##teamcity[flowStarted parent='1' flowId='2']",
               "##teamcity[flowStarted parent='1' flowId='3']",
               "##teamcity[message text='flow1 message' status='NORMAL' flowId='2']",
               "##teamcity[message text='flowB message' status='NORMAL' flowId='1']",
               "##teamcity[message text='flow2 message' status='NORMAL' flowId='3']",
               "##teamcity[flowFinished flowId='3']",
               "##teamcity[flowFinished flowId='2']"
               );      
    }

    [Test, ExpectedException]
    public void TestDoNotAllow_MessageIfBlock()
    {
      DoTestWithoutAsseert(x =>
                             {
                               using (x.OpenBlock("b"))
                               {
                                 x.WriteMessage("msg");
                               }
                             });
    }

    
    [Test]
    public void TestFlows_TwoBlocks()
    {
      DoTest(x =>
               {
                 using(x)
                 {
                   var flow1 = x.OpenFlow();
                   var flow2 = x.OpenFlow();

                   var block1 = flow1.OpenBlock("b1");
                   var block2 = flow2.OpenBlock("b2");
                   var blockR = x.OpenBlock("root");

                   blockR.WriteMessage("root");
                   block1.WriteMessage("flow1 message");
                   block2.WriteMessage("flow2 message");


                   block1.Dispose();
                   blockR.Dispose();
                   block2.Dispose();


                   flow2.Dispose();
                   flow1.Dispose();                   
                 }
               },
               "##teamcity[flowStarted parent='1' flowId='2']",
               "##teamcity[flowStarted parent='1' flowId='3']",
               "##teamcity[blockOpened name='b1' flowId='2']",
               "##teamcity[blockOpened name='b2' flowId='3']",
               "##teamcity[blockOpened name='root' flowId='1']",
               "##teamcity[message text='root' status='NORMAL' flowId='1']",
               "##teamcity[message text='flow1 message' status='NORMAL' flowId='2']",
               "##teamcity[message text='flow2 message' status='NORMAL' flowId='3']",
               "##teamcity[blockClosed name='b1' flowId='2']",
               "##teamcity[blockClosed name='root' flowId='1']",
               "##teamcity[blockClosed name='b2' flowId='3']",
               "##teamcity[flowFinished flowId='3']",
               "##teamcity[flowFinished flowId='2']"
               );      
 +   }
 
  }
}
