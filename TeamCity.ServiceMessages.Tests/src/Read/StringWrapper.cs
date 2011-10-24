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