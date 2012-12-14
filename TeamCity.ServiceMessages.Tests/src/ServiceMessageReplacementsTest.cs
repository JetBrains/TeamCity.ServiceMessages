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

using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests
{
  [TestFixture]
  public class ServiceMessageReplacementsTest
  {
    [Test]
    public void TestEncode_0()
    {
      Assert.AreEqual("", ServiceMessageReplacements.Encode(""));
    }

    [Test]
    public void TestEncode_1()
    {
      Assert.AreEqual("a", ServiceMessageReplacements.Encode("a"));
    }

    [Test]
    public void TestEncode_1_special()
    {
      Assert.AreEqual("|n", ServiceMessageReplacements.Encode("\n"));
      Assert.AreEqual("|r", ServiceMessageReplacements.Encode("\r"));
      Assert.AreEqual("|]", ServiceMessageReplacements.Encode("]"));
      Assert.AreEqual("|'", ServiceMessageReplacements.Encode("'"));
      Assert.AreEqual("||", ServiceMessageReplacements.Encode("|"));
      Assert.AreEqual("|x", ServiceMessageReplacements.Encode("\u0085"));
      Assert.AreEqual("|l", ServiceMessageReplacements.Encode("\u2028"));
      Assert.AreEqual("|p", ServiceMessageReplacements.Encode("\u2029"));
    }

    [Test]
    public void TestDecode_0()
    {
      Assert.AreEqual("", ServiceMessageReplacements.Decode(""));
    }

    [Test]
    public void TestDecode_1()
    {
      Assert.AreEqual("a", ServiceMessageReplacements.Decode("a"));
    }

    [Test]
    public void TestDecode_1_special()
    {
      Assert.AreEqual(ServiceMessageReplacements.Decode("|n"), "\n");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|r"), "\r");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|]"), "]");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|'"), "'");
      Assert.AreEqual(ServiceMessageReplacements.Decode("||"), "|");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|x"), "\u0085");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|l"), "\u2028");
      Assert.AreEqual(ServiceMessageReplacements.Decode("|p"), "\u2029");
    }
  }
}