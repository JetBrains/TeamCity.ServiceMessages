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

using System.IO;

namespace JetBrains.TeamCity.ServiceMessages.Tests.Read
{
  public class StringWrapper : TextReader
  {
    private readonly string myString;
    private int myOffset = 0;

    public StringWrapper(string s)
    {
      myString = s;
    }

    public override int Peek()
    {
      throw new System.NotImplementedException();
    }

    public override int Read()
    {
      if (myOffset == myString.Length) return -1;
      if (myOffset > myString.Length) throw new IOException("Beyond end");
      return myString[myOffset++];
    }

    public override int Read(char[] buffer, int index, int count)
    {
      throw new System.NotImplementedException();
    }

    public override string ReadToEnd()
    {
      throw new System.NotImplementedException();
    }

    public override int ReadBlock(char[] buffer, int index, int count)
    {
      throw new System.NotImplementedException();
    }

    public override string ReadLine()
    {
      throw new System.NotImplementedException();
    }
  }
}