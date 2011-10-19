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

using JetBrains.TeamCity.ServiceMessages.Write;
using NUnit.Framework;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Write
{
  [TestFixture]
  public class ServiceMessageFormatterTest
  {
    [Test]
    public void SupportAnonymousType()
    {
      Assert.AreEqual(
        "##teamcity[rulez Version='ver' Vika='678' Int='42']",
        ServiceMessageFormatter.FormatMessage("rulez", new
                                                         {
                                                           Version = "ver",
                                                           Vika = "678",
                                                           Int = 42
                                                         }));
    }

    [Test]
    public void SupportEscaping()
    {
      Assert.AreEqual(
        "##teamcity[rulez Attribute='\" |' |n |r |x |l |p || [ |]']",
        ServiceMessageFormatter.FormatMessage("rulez", new
                                                         {
                                                           Attribute = "\" ' \n \r \u0085 \u2028 \u2029 | [ ]",
                                                         }));
    }
    
 
    [Test]
    public void SimpleMessage()
    {
      Assert.AreEqual(
        "##teamcity[rulez 'qqq']",
        ServiceMessageFormatter.FormatMessage("rulez", "qqq"));
    }

    [Test]
    public void SupportArray()
    {
      Assert.AreEqual(
        "##teamcity[rulez qqq='ppp']",
        ServiceMessageFormatter.FormatMessage("rulez", new ServiceMessageProperty("qqq", "ppp")));
    }

    [Test]
    public void SupportArray2()
    {
      Assert.AreEqual(
        "##teamcity[rulez qqq='ppp' www='xxx']",
        ServiceMessageFormatter.FormatMessage("rulez", new ServiceMessageProperty("qqq", "ppp"), new ServiceMessageProperty("www", "xxx")));
    }

    [Test]
    public void SupportEnumerable()
    {
      Assert.AreEqual(
        "##teamcity[rulez qqq='ppp']",
        ServiceMessageFormatter.FormatMessage("rulez", new [] {new ServiceMessageProperty("qqq", "ppp")}));
    }

    [Test]
    public void SupportEnumerable2()
    {
      Assert.AreEqual(
        "##teamcity[rulez qqq='ppp' rrr='wqe']",
        ServiceMessageFormatter.FormatMessage("rulez", new [] {new ServiceMessageProperty("qqq", "ppp"), new ServiceMessageProperty("rrr", "wqe")}));
    }
  }
}