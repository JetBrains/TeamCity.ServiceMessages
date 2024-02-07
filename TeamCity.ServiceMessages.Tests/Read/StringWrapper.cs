

namespace JetBrains.TeamCity.ServiceMessages.Tests.Read
{
    using System;
    using System.IO;

    public class StringWrapper : TextReader
    {
        public StringWrapper(string s)
        {
            myString = s;
        }

        private readonly string myString;
        private int myOffset;

        public override int Peek()
        {
            throw new NotImplementedException();
        }

        public override int Read()
        {
            if (myOffset == myString.Length) return -1;
            if (myOffset > myString.Length) throw new IOException("Beyond end");
            return myString[myOffset++];
        }

        public override int Read(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override string ReadToEnd()
        {
            throw new NotImplementedException();
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override string ReadLine()
        {
            throw new NotImplementedException();
        }
    }
}