/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Text;
using JetBrains.TeamCity.ServiceMessages.Write;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using NUnit.Framework;
using System.Linq;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write.Specials
{
  public abstract class TeamCityWriterBaseTest<T>
  {
    protected abstract T Create(IServiceMessageProcessor proc);
    protected virtual ToStringProcessor CreateProcessor()
    {
      return new ToStringProcessor();
    }

    protected void DoTest(Action<T> action, params string[] golds)
    {
      if (golds == null || golds.Any(x => x == null)) throw new ArgumentNullException("golds");
      DoTestImpl(action, golds);
    }

    protected void DoTestWithoutAsseert(Action<T> action)
    {
      DoTestImpl(action, null);
    }

    private void DoTestImpl(Action<T> action, string[] golds)
    {
      var proc = CreateProcessor();
      var myWriter = Create(proc);
      
      action(myWriter);

      if (golds == null) return;

      Func<string, string[]> preprocess = s => s.Split("\r\n".ToCharArray()).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
      var actual = preprocess(proc.Buffer);
      var actualText = "\r\n" + string.Join("\r\n", actual);
      var expected =  preprocess(string.Join("\r\n", golds));

      if (actual.Count() != expected.Count())
      {
        Assert.Fail("Incorrect number of messages. Was: " + actualText);
      }

      for(int i = 0;  i < actual.Count(); i++)
      {
        Assert.AreEqual(actual[i], expected[i], "Message {0} does not match. Was: {1}", i, actualText);
      }
    }

    protected class ToStringProcessor : IServiceMessageProcessor
    {      
      private readonly StringBuilder myBuffer = new StringBuilder();
      public string Buffer { get { return myBuffer.ToString(); } }

      public virtual void AddServiceMessage(IServiceMessage serviceMessage)
      {
        myBuffer.AppendLine(new ServiceMessageFormatter().FormatMessage(serviceMessage));
      }
    }
    
  }
}