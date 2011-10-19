using System.Collections.Generic;
using JetBrains.Annotations;

namespace JetBrains.TeamCity.ServiceMessages.Read
{
  public class ServiceMessage
  {
    private readonly Dictionary<string, string> myProperties;

    [NotNull]
    public string Name { get; private set; }

    [CanBeNull]
    public string DefaultValue { get; private set; }

    public ServiceMessage([NotNull] string name, [CanBeNull] string defaultValue, [NotNull] Dictionary<string, string> properties)
    {
      Name = name;
      DefaultValue = defaultValue;
      myProperties = properties;
    }

    [CanBeNull]
    public string this[[NotNull] string key]
    {
      get
      {
        string s;
        return myProperties.TryGetValue(key, out s) ? s : null;
      }
    }

    public IEnumerable<string> Keys { get { return myProperties.Keys; } }
  }
}