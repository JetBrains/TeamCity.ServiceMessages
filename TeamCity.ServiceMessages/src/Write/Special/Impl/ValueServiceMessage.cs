using System.Collections.Generic;

namespace JetBrains.TeamCity.ServiceMessages.Write.Special.Impl
{
  internal class ValueServiceMessage : IServiceMessage
  {
    private readonly string myName;
    private readonly string myValue;

    public ValueServiceMessage(string name, string value)
    {
      myName = name;
      myValue = value;
    }

    public string Name
    {
      get { return myName; }
    }

    public string DefaultValue
    {
      get { return myValue; }
    }

    public IEnumerable<string> Keys
    {
      get { yield break; }
    }

    public string GetValue(string key)
    {
      return null;
    }
  }
}